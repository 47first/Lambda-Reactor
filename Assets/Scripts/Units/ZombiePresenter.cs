using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Runtime
{
    public sealed class ZombiePresenter : UnitPresenter, IDamageable, ICellClickObserver
    {
        private const float range = 2;
        private int health = 3000;

        public ZombiePresenter(UnitView view,
            IEnvironmentController environmentController,
            IQueryController queryController,
            IGameView gameView,
            Team team,
            int stack)
            : base(view, environmentController, queryController, gameView, team, stack)
        {
            Initiative = 4;
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

        private (UnitView unit, Team initialTeam) _infected;
        public void CellClicked(Cell cell)
        {
            var isCellValid = cell == View.Cell || View.Cell.Distance(cell) > range;

            if (isCellValid)
                return;

            if (_infected.unit is not null)
            {
                _infected.unit.Presenter.Team = _infected.initialTeam;
                _infected = new(null, default);
            }

            var unitAtClickedCell = EnvironmentController.GetUnitAt(cell);

            if (unitAtClickedCell is null)
                View.MoveTo(cell);

            else if (unitAtClickedCell.Presenter.Team != Team)
            {
                _infected = new(unitAtClickedCell, unitAtClickedCell.Presenter.Team);
                unitAtClickedCell.Presenter.Team = Team;
            }

            Deactivate();
        }

        private void Deactivate()
        {
            EnvironmentController.ResetCellObserver();

            ExecuteNext();
        }
    }
}
