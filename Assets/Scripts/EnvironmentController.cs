using System;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    public sealed class EnvironmentController : MonoBehaviour
    {
        [SerializeField] private List<Cell> _cells;

        public event Action<Cell> CellClicked;
    }
}
