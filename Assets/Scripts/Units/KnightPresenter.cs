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
            {
                View.Cell.Highlighted = false;
                View.Disapear();
            }
            else
                UpdateStackValue();

            Debug.Log($"{View.name} Received {damage} Damage");
        }

        public override void Activate()
        {
            View.Cell.Highlighted = true;
            EnvironmentController.CellClicked += OnCellClicked;
        }

        private void OnCellClicked(Cell cell)
        {
            if (cell == View.Cell)
                return;

            var unitAtClickedCell = EnvironmentController.GetUnitAt(cell);

            EnvironmentController.CellClicked -= OnCellClicked;

            if (unitAtClickedCell is null)
            {
                View.Cell.Highlighted = false;
                View.MoveTo(cell);
                QueryController.Next();
            }
            else
            {
                if (unitAtClickedCell.Presenter is IDamageable damageable)
                    damageable?.ReceiveDamage(Random.Range(_minDamage, _maxDamage));

                View.Cell.Highlighted = false;
                QueryController.Next();
            }
        }
    }
}
