﻿using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    public interface IEnvironmentController
    {
        public void RegisterUnit(UnitView unit);

        public void SetCellObserver<T>(T cellObserver);

        public void ResetCellObserver();

        public IEnumerable<UnitView> Units { get; }

        public IEnumerable<Cell> Cells { get; }

        public UnitView GetUnitAt(Cell cell);

        public Cell GetCellAt(Vector2 position);

        public void SetAllCellsTo(CellState state);
    }
}
