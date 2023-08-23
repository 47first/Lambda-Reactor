using System;

namespace Runtime
{
    public interface IEnvironmentController
    {
        public event Action<Cell> CellClicked;
    }
}
