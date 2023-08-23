using DG.Tweening;
using System;
using UnityEngine;
using Zenject;

namespace Runtime
{
    public sealed class UnitView : MonoBehaviour
    {
        [Inject] private IEnvironmentController _environmentController;

        [field: SerializeField] public Cell Cell { get; private set; }

        public void MoveTo(Cell cell)
        {
            Cell.Selected -= OnCellSelected;

            Cell = cell;

            Cell.Selected += OnCellSelected;
            transform.DOMove(cell.transform.position, 0.5f);
        }

        private void Start()
        {
            MoveTo(Cell);
        }

        private void OnCellSelected()
        {
            Debug.Log("Cell selected");
            _environmentController.CellClicked += OnCellClicked;
        }

        private void OnCellClicked(Cell cell)
        {
            Debug.Log("Cell clicked");
            _environmentController.CellClicked -= OnCellClicked;
            MoveTo(cell);
        }
    }
}
