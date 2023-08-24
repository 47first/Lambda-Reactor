using System.Linq;
using UnityEngine;

namespace Runtime
{
    public sealed class KnightPresenter : UnitPresenter, IDamageable, ICellEventsObserver
    {
        public KnightPresenter(UnitView view,
            IEnvironmentController environmentController,
            IQueryController queryController,
            Team team,
            int stack)
            : base(view, environmentController, queryController, team, stack)
        {
            Health = 20;
            Range = 5;
            MinDamage = 6;
            MaxDamage = 10;
            Initiative = 4;
        }

        private int _receivedDamageRequestCount = 0;
        public void ReceiveDamage(float damage)
        {
            var absorbedDamaged = GetAbsorbedDamage(damage);

            int damagedStacks = (int)((damage - absorbedDamaged) / Health);

            Stack -= damagedStacks;
            _receivedDamageRequestCount++;

            Debug.Log($"{View.name} received {damagedStacks} damage");

            if (Stack <= 0)
                View.Disapear();
        }

        private float GetAbsorbedDamage(float damage) => _receivedDamageRequestCount switch
        {
            0 => GetPercentOf(damage, 99),
            1 => GetPercentOf(damage, 66),
            2 => GetPercentOf(damage, 33),
            _ => 0
        };

        private float GetPercentOf(float y, float percent) => (y * percent) / 100;

        private System.Action _next;
        public override void Activate(System.Action next)
        {
            _next = next;

            EnvironmentController.SetCellObserver(this);

            UpdateCellStates();
        }

        private void UpdateCellStates()
        {
            EnvironmentController.SetAllCellsTo(CellState.Unactive);

            var cellsInRange = EnvironmentController.Cells
                .Where(cell => cell.Distance(View.Cell) < Range);

            foreach (var cell in cellsInRange)
                cell.SetState(CellState.Active);

            View.Cell.SetState(CellState.Highligthed);
        }

        private bool _extraStepInvoked = false;
        public void CellClicked(Cell cell)
        {
            if (cell == View.Cell || View.Cell.Distance(cell) > Range)
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
                    damageable?.ReceiveDamage(Random.Range(MinDamage, MaxDamage) * Stack);
                    canInvokeNext = true;
                }
            }

            canInvokeNext = canInvokeNext && _extraStepInvoked && (Random.Range(0, 100) < 45);

            if (canInvokeNext)
                CallNext();
            else
                _extraStepInvoked = true;
        }

        private void CallNext()
        {
            _extraStepInvoked = false;

            EnvironmentController.ResetCellObserver();

            _next?.Invoke();
        }

        public void CellSelected(Cell cell)
        {
        }
    }
}
