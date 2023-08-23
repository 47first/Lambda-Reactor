using System;
using System.Collections;
using System.Collections.Generic;

namespace Runtime
{
    public interface IEnvironmentController
    {
        public event Action<Cell> CellClicked;

        public IEnumerable<UnitView> Units { get; }

        public UnitView GetUnitAt(Cell cell);
    }
}
