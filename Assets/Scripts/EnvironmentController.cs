using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Runtime
{
    public sealed class EnvironmentController : MonoBehaviour, IEnvironmentController
    {
        private List<UnitView> _units = new();
        private List<Cell> _cells = new();
        private ICellEventsObserver _cellObserver;

        public IEnumerable<UnitView> Units => _units.Where(unit => unit.gameObject != null && unit.gameObject.activeSelf);
        public IEnumerable<Cell> Cells => _cells/*.Where(cell => cell != null && cell.gameObject.activeSelf)*/;

        public UnitView GetUnitAt(Cell cell) => Units.FirstOrDefault(unit => unit.Cell == cell);

        private void Start()
        {
            _cells.AddRange(Resources.FindObjectsOfTypeAll<Cell>());
            _units.AddRange(Resources.FindObjectsOfTypeAll<UnitView>());

            foreach (var cell in _cells)
            {
                cell.Clicked += OnCellClicked;
                cell.Selected += OnCellSelected;
            }
        }

        public void SetCellObserver(ICellEventsObserver cellObserver) => _cellObserver = cellObserver;

        public void ResetCellObserver() => _cellObserver = null;

        public void SetAllCellsTo(CellState state)
        {
            foreach (var cell in _cells)
                cell.SetState(state);
        }

        private void OnCellClicked(Cell cell) => _cellObserver?.CellClicked(cell);

        private void OnCellSelected(Cell cell) => _cellObserver?.CellSelected(cell);

        public Cell GetCellAt(Vector2 position) => _cells.FirstOrDefault(cell => cell.Position == position);
    }
}
