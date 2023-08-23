using System;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    public sealed class EnvironmentController : MonoBehaviour
    {
        [SerializeField] private List<Cell> _cells;

        public event Action<Cell> CellClicked;

        private void Start()
        {
            _cells.AddRange(Resources.FindObjectsOfTypeAll<Cell>());

            foreach (var cell in _cells)
            {
                cell.Clicked += () => CellClicked?.Invoke(cell);
                //cell.Selected += ;
            }
        }
    }
}
