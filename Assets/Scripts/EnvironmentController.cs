using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Runtime
{
    public sealed class EnvironmentController : MonoBehaviour, IEnvironmentController
    {
        private List<UnitView> _units = new();
        private List<Cell> _cells = new();

        public IEnumerable<UnitView> Units => _units.Where(unit => unit != null && unit.gameObject.activeSelf);
        public IEnumerable<Cell> Cells => _cells/*.Where(cell => cell != null && cell.gameObject.activeSelf)*/;


        public event Action<Cell> CellClicked;
        public event Action<Cell> CellSelected;

        public UnitView GetUnitAt(Cell cell)
        {
            return Units.FirstOrDefault(unit => unit.Cell == cell);
        }

        private void Start()
        {
            _cells.AddRange(Resources.FindObjectsOfTypeAll<Cell>());
            _units.AddRange(Resources.FindObjectsOfTypeAll<UnitView>());

            foreach (var cell in _cells)
            {
                cell.Clicked += () => CellClicked?.Invoke(cell);
                cell.Selected += () => CellSelected?.Invoke(cell);
            }
        }

        public void SetAllCellsTo(CellState state)
        {
            foreach (var cell in _cells)
                cell.SetState(state);

            //_cells.ForEach(cell => cell.SetState(state));
        }

        public Cell GetCellAt(Vector2 position) => _cells.FirstOrDefault(cell => cell.Position == position);
    }
}
