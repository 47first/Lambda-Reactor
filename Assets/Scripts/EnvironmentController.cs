using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Runtime
{
    public sealed class EnvironmentController : MonoBehaviour, IEnvironmentController
    {
        private List<UnitView> _units = new();
        private List<Cell> _cells = new();

        private ICellClickObserver _cellClickObserver;
        private ICellSelectObserver _cellSelectObserver;

        public IEnumerable<UnitView> Units => _units.Where(unit => unit.gameObject != null && unit.gameObject.activeSelf);
        public IEnumerable<Cell> Cells => _cells;

        public UnitView GetUnitAt(Cell cell) => Units.FirstOrDefault(unit => unit.Cell == cell);

        private void Start()
        {
            _cells.AddRange(Object.FindObjectsOfType<Cell>());
            _units.AddRange(Object.FindObjectsOfType<UnitView>());

            foreach (var cell in _cells)
            {
                cell.Clicked += OnCellClicked;
                cell.Selected += OnCellSelected;
            }
        }

        public void SetCellObserver<T>(T cellObserver)
        {
            if(cellObserver is ICellClickObserver clickObserver)
                _cellClickObserver = clickObserver;

            if(cellObserver is ICellSelectObserver selectObserver)
                _cellSelectObserver = selectObserver;
        }

        public void ResetCellObserver()
        {
            _cellClickObserver = null;
            _cellSelectObserver = null;
        }

        public void SetAllCellsTo(CellState state)
        {
            foreach (var cell in _cells)
                cell.SetState(state);
        }

        private void OnCellClicked(Cell cell) => _cellClickObserver?.CellClicked(cell);

        private void OnCellSelected(Cell cell) => _cellSelectObserver?.CellSelected(cell);

        public Cell GetCellAt(Vector2 position) => _cells.FirstOrDefault(cell => cell.Position == position);

        public void RegisterUnit(UnitView unit) => _units.Add(unit);
    }
}
