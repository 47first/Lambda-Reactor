using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Runtime
{
    public sealed class EnvironmentController : MonoBehaviour, IEnvironmentController
    {
        private List<UnitView> _units = new();
        private List<Cell> _cells = new();

        public IEnumerable<UnitView> Units => _units;


        public event Action<Cell> CellClicked;

        private void Start()
        {
            _cells.AddRange(Resources.FindObjectsOfTypeAll<Cell>());
            _units.AddRange(Resources.FindObjectsOfTypeAll<UnitView>());

            foreach (var cell in _cells)
            {
                cell.Clicked += () => CellClicked?.Invoke(cell);;
            }
        }
    }
}
