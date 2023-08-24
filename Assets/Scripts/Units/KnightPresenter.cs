using System.Linq;
using UnityEngine;

namespace Runtime
{
    public sealed class KnightPresenter : UnitPresenter, IDamageable
    {
        private const int _health = 20;
        private const int _range = 5;
        private const int _minDamage = 6;
        private const int _maxDamage = 10;
        private const int initiative = 4;

        public KnightPresenter(UnitView view,
            IEnvironmentController environmentController,
            IQueryController queryController,
            Team team,
            int stack) : base(view, environmentController, queryController, team, initiative, stack)
        {
        }

        public void ReceiveDamage(float damage)
        {
            int damagedStacks = (int)damage / _health;

            Stack -= damagedStacks;

            if (Stack <= 0)
                View.Disapear();
        }

        public override void Activate()
        {
            EnvironmentController.SetAllCellsTo(CellState.Unactive);

            var cellsInRange = EnvironmentController.Cells
                .Where(cell => Vector2.Distance(View.Cell.Position, cell.Position) < _range);

            foreach (var cell in cellsInRange)
                cell.SetState(CellState.Active);

            View.Cell.SetState(CellState.Highligthed);

            EnvironmentController.CellClicked += OnCellClicked;
            EnvironmentController.CellSelected += OnCellSelected;
        }

        private void OnCellSelected(Cell cell)
        {
            if (cell == View.Cell)
                return;
        }

        private void OnCellClicked(Cell cell)
        {
            if (cell == View.Cell || Vector2.Distance(View.Cell.Position, cell.Position) > _range)
                return;

            var unitAtClickedCell = EnvironmentController.GetUnitAt(cell);

            if (unitAtClickedCell is null)
            {
                EnvironmentController.CellClicked -= OnCellClicked;
                EnvironmentController.CellSelected -= OnCellSelected;

                View.MoveTo(cell);

                QueryController.Next();
            }
            
            if(unitAtClickedCell is not null)
            {
                if (unitAtClickedCell.Presenter is IDamageable damageable && unitAtClickedCell.Presenter.Team != Team)
                {
                    EnvironmentController.CellClicked -= OnCellClicked;
                    EnvironmentController.CellSelected -= OnCellSelected;

                    damageable?.ReceiveDamage(Random.Range(_minDamage, _maxDamage) * Stack);

                    QueryController.Next();
                }
            }
        }
    }
}
