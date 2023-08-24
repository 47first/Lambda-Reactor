using System.Linq;
using UnityEngine;

namespace Runtime
{
    public sealed class KnightPresenter : UnitPresenter, IDamageable
    {
        private int _health = 20;
        private int _range = 5;
        private int _minDamage = 6;
        private int _maxDamage = 10;
        private int _initiative = 4;

        public KnightPresenter(UnitView view,
            IEnvironmentController environmentController,
            IQueryController queryController,
            Team team,
            int stack) : base(view, environmentController, queryController, team, stack)
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
            if (cell == View.Cell)
                return;

            var unitAtClickedCell = EnvironmentController.GetUnitAt(cell);

            if (unitAtClickedCell is null && Vector2.Distance(View.Cell.Position, cell.Position) < _range)
            {
                EnvironmentController.CellClicked -= OnCellClicked;

                View.MoveTo(cell);

                QueryController.Next();
            }
            
            if(unitAtClickedCell is not null && unitAtClickedCell.Presenter.Team != Team)
            {
                if (unitAtClickedCell.Presenter is IDamageable damageable)
                {
                    EnvironmentController.CellClicked -= OnCellClicked;
                    damageable?.ReceiveDamage(Random.Range(_minDamage, _maxDamage) * Stack);
                    QueryController.Next();
                }
            }
        }
    }
}
