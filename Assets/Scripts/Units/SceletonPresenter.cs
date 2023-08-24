using System.Linq;
using UnityEngine;

namespace Runtime
{
    public sealed class SceletonPresenter : UnitPresenter, IDamageable, ICellClickObserver
    {
        private int _health = 6;
        private float _range = 3;
        private float _damage = 6;
        private UnitView _sceletonPrefab;

        public SceletonPresenter(UnitView view,
            IEnvironmentController environmentController,
            IQueryController queryController,
            IGameView gameView,
            Team team,
            int stack,
            UnitView bonesPrefab)
            : base(view, environmentController, queryController, gameView, team, stack)
        {
            Initiative = 6;
            _sceletonPrefab = bonesPrefab;
        }

        public void SetStats(float health, float range, float damage)
        {
            _range = Mathf.RoundToInt(health);
            _range = Mathf.Round(range);
            _range = Mathf.Round(damage);
        }

        private int _childCount;
        public void ReceiveDamage(float damage)
        {
            int damagedStacks = (int)(damage / _health);

            Stack -= damagedStacks;

            if(_sceletonPrefab is not null && _childCount < 3)
                SpawnSceleton(damagedStacks);

            Debug.Log($"{View.name} received {damagedStacks} damage");

            if (Stack <= 0)
                View.Disapear();
        }

        private void SpawnSceleton(int stackCount)
        {
            var freeCell = GetFreeCell();

            if (freeCell is null)
                return;

            _childCount++;

            var sceleton = Object.Instantiate(_sceletonPrefab);

            sceleton.transform.position = freeCell.transform.position;
            sceleton.MoveTo(freeCell);
            sceleton.Setup(EnvironmentController, QueryController, GameView);
            sceleton.Presenter.Stack = stackCount;

            if (sceleton.Presenter is SceletonPresenter sceletonPresenter)
                sceletonPresenter.SetStats(_health * (0.3f * _childCount),
                    _range * (0.3f * _childCount),
                    _damage * (0.3f * _childCount));

            EnvironmentController.RegisterUnit(sceleton);
        }

        private Cell GetFreeCell()
        {
            return EnvironmentController.Cells.FirstOrDefault(cell =>
            (View.Cell.Distance(cell) < 2) && EnvironmentController.GetUnitAt(cell) is null);
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
                .Where(cell => cell.Distance(View.Cell) < _range);

            foreach (var cell in cellsInRange)
                cell.SetState(CellState.Active);

            View.Cell.SetState(CellState.Highligthed);
        }

        public void CellClicked(Cell cell)
        {
            var isCellValid = cell == View.Cell || View.Cell.Distance(cell) > _range;

            if (isCellValid)
                return;

            var unitAtClickedCell = EnvironmentController.GetUnitAt(cell);

            if (unitAtClickedCell is null)
            {
                View.MoveTo(cell);
                Deactivate();
            }

            else if (unitAtClickedCell.Presenter is IDamageable damageable && unitAtClickedCell.Presenter.Team != Team)
            {
                damageable?.ReceiveDamage(_damage * Stack);
                Deactivate();
            }
        }

        private void Deactivate()
        {
            EnvironmentController.ResetCellObserver();

            ExecuteNext();
        }
    }
}
