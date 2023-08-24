using System.Linq;
using UnityEngine;

namespace Runtime
{
    public sealed class KnightPresenter : UnitPresenter, IDamageable
    {
        private float _health = 20;
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
            Stack = Mathf.Max(Stack - (int)damage, 0);

            if (Stack <= 0)
                View.Disapear();
            else
                UpdateStackValue();

            Debug.Log($"{View.name} Received {damage} Damage");
        }

        public override void Activate()
        {
            EnvironmentController.SetAllCellsTo(CellState.Unactive);

            foreach (var cell in EnvironmentController.Cells)
            {
                if (Vector2.Distance(View.Cell.Position, cell.Position) < 3)
                {
                    Debug.Log("In range");
                    cell.SetState(CellState.Active);
                }
            }

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

            if (unitAtClickedCell is null && Vector2.Distance(View.Cell.Position, cell.Position) < 3)
            {
                EnvironmentController.CellClicked -= OnCellClicked;

                View.MoveTo(cell);
                QueryController.Next();
            }
            
            if(unitAtClickedCell is not null)
            {
                EnvironmentController.CellClicked -= OnCellClicked;

                if (unitAtClickedCell.Presenter is IDamageable damageable)
                    damageable?.ReceiveDamage(Random.Range(_minDamage, _maxDamage));

                QueryController.Next();
            }
        }
    }
}
