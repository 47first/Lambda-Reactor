using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Runtime
{
    public sealed class ShooterPresenter : UnitPresenter, IDamageable,
        ICellClickObserver, ICellSelectObserver, IDropdownObserver
    {
        private const int health = 10;
        private const float range = 6;
        private const float minFistsDamage = 1;
        private const float maxFistsDamage = 6;
        private const float minBowDamage = 6;
        private const float maxBowDamage = 15;
        private BowMode _bowMode;

        public ShooterPresenter(UnitView view,
            IEnvironmentController environmentController,
            IQueryController queryController,
            IGameView gameView,
            Team team,
            int stack)
            : base(view, environmentController, queryController, gameView, team, stack)
        {
            Initiative = 6;
        }

        public void ReceiveDamage(float damage)
        {
            int damagedStacks = (int)(damage / health);

            Stack -= damagedStacks;

            Debug.Log($"{View.name} received {damagedStacks} damage");

            if (Stack <= 0)
                View.Disapear();
        }

        protected override void OnActivate()
        {
            EnvironmentController.SetCellObserver(this);

            GameView.SetOptions(new string[] { "Movement", "Single", "Multiple" });
            GameView.ShowDropdown();

            _bowMode = (BowMode)GameView.GetDropdownValue();

            UpdateCellStates();
        }

        public void DropdownValueChanged(int index) => _bowMode = (BowMode)index;

        private void UpdateCellStates()
        {
            EnvironmentController.SetAllCellsTo(CellState.Unactive);

            if (_bowMode == BowMode.None)
                ActivateInRangeCells();

            View.Cell.SetState(CellState.Highligthed);
        }

        private void ActivateInRangeCells()
        {
            var cellsInRange = EnvironmentController.Cells
                .Where(cell => cell.Distance(View.Cell) < range);

            foreach (var cell in cellsInRange)
                cell.SetState(CellState.Active);
        }

        public void CellSelected(Cell selectedCell)
        {
            UpdateCellStates();

            if (_bowMode == BowMode.None)
                return;

            foreach (var cell in GetSelectedCells(selectedCell))
                cell.SetState(CellState.Highligthed);
        }

        private IEnumerable<Cell> GetSelectedCells(Cell cell)
        {
            if (_bowMode == BowMode.Single)
                return new Cell[] { cell };
            else
                return GetMultipleBowCells(cell.Position);
        }

        private IEnumerable<Cell> GetMultipleBowCells(Vector2 startPosition)
        {
            if (startPosition.y % 2 != 0)
                startPosition = startPosition + Vector2.up;

            List<Vector2> positions = new()
            {
                startPosition,
                startPosition + new Vector2(0, 2),
                startPosition + new Vector2(1, 2),
                startPosition + new Vector2(0, 1),
                startPosition + new Vector2(1, 1),
                startPosition + new Vector2(2, 1),
                startPosition + new Vector2(1, 0),
                startPosition + new Vector2(0, -1),
                startPosition + new Vector2(1, -1),
                startPosition + new Vector2(2, -1),
                startPosition + new Vector2(0, -2),
                startPosition + new Vector2(1, -2),
            };

            return GetCellsAt(positions);
        }

        private IEnumerable<Cell> GetCellsAt(IEnumerable<Vector2> positions)
        {
            return EnvironmentController.Cells.Where(cell => positions.Contains(cell.Position));
        }

        public void CellClicked(Cell cell)
        {
            if (_bowMode == BowMode.None)
                MoveToCell(cell);

            else
                ShootAt(GetSelectedCells(cell));
        }

        private void MoveToCell(Cell cell)
        {
            if (cell == View.Cell || EnvironmentController.GetUnitAt(cell) is not null)
                return;

            View.MoveTo(cell);

            Deactivate();
        }

        private void ShootAt(IEnumerable<Cell> cells)
        {
            var unitsToAttack = cells.Select(cell => EnvironmentController.GetUnitAt(cell))
                .Where(unit => unit?.Presenter is IDamageable && unit?.Presenter.Team != Team);

            if (unitsToAttack.Any() == false)
                return;

            var damage = (Random.Range(minBowDamage, maxBowDamage) / (_bowMode == BowMode.Multiple ? 2 : 1)) * Stack;

            foreach (IDamageable unit in unitsToAttack.Select(unit => unit.Presenter))
                unit.ReceiveDamage(damage);

            Deactivate();
        }

        private void Deactivate()
        {
            EnvironmentController.ResetCellObserver();

            GameView.HideDropdown();

            ExecuteNext();
        }
    }
}
