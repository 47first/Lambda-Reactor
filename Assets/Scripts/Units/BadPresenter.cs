using System.Linq;
using UnityEngine;

namespace Runtime
{
    public sealed class BadPresenter : UnitPresenter, IDamageable, ICellClickObserver
    {
        private const float extraStepChance = 100;
        private int health = 1000;
        private const float range = 5;
        private const float minDamage = 350;
        private const float maxDamage = 700;

        public BadPresenter(UnitView view,
            IEnvironmentController environmentController,
            IQueryController queryController,
            IGameView gameView,
            Team team,
            int stack)
            : base(view, environmentController, queryController, gameView, team, stack)
        {
            Initiative = 10;
        }

        public void ReceiveDamage(float damage)
        {
            health -= (int)damage;

            Debug.Log($"{View.name} received {damage} damage");

            if (health <= 0)
                View.Disapear();
        }

        protected override void OnActivate()
        {
            EnvironmentController.SetCellObserver(this);

            UpdateCellStates();
        }

        private void UpdateCellStates()
        {
            EnvironmentController.SetAllCellsTo(CellState.Unactive);

            var cellsInRange = EnvironmentController.Cells
                .Where(cell => cell.Distance(View.Cell) < range);

            foreach (var cell in cellsInRange)
                cell.SetState(CellState.Active);

            View.Cell.SetState(CellState.Highligthed);
        }

        private bool _extraStepInvoked = false;

        public void CellClicked(Cell cell)
        {
            var isCellValid = cell == View.Cell || View.Cell.Distance(cell) > range;

            if (isCellValid)
                return;

            var unitAtClickedCell = EnvironmentController.GetUnitAt(cell);
            var canInvokeNext = false;

            if (unitAtClickedCell is null)
            {
                View.MoveTo(cell);
                UpdateCellStates();
                canInvokeNext = true;
            }

            if (unitAtClickedCell is not null)
            {
                if (unitAtClickedCell.Presenter is IDamageable damageable && unitAtClickedCell.Presenter.Team != Team)
                {
                    damageable?.ReceiveDamage(Random.Range(minDamage, maxDamage) * Stack);
                    canInvokeNext = true;
                }
            }

            canInvokeNext = canInvokeNext && _extraStepInvoked;

            _extraStepInvoked = true;

            if (canInvokeNext)
                Deactivate();
        }

        private void Deactivate()
        {
            _extraStepInvoked = false;

            EnvironmentController.ResetCellObserver();

            ExecuteNext();
        }
    }
}
