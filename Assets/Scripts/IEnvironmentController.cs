using System;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    public interface IEnvironmentController
    {
        public event Action<Cell> CellClicked;
        public event Action<Cell> CellSelected;

        public IEnumerable<UnitView> Units { get; }
        public IEnumerable<Cell> Cells { get; }

        public UnitView GetUnitAt(Cell cell);

        public void SetAllCellsTo(CellState state);

        public Cell GetCellAt(Vector2 position);
    }
}
