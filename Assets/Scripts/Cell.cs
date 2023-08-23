using DG.Tweening;
using System;
using UnityEngine;

namespace Runtime
{
    public enum CellState
    {
        Default,
        Selected,
        Highligthed
    }

    public sealed class Cell : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        private Tween _colorTween;

        public event Action Selected;
        public event Action Clicked;

        private void OnMouseEnter()
        {
            SetState(CellState.Selected);
        }

        private void OnMouseExit()
        {
            SetState(CellState.Default);
        }

        private void OnMouseDown()
        {
            Clicked?.Invoke();
            Selected?.Invoke();
            SetState(CellState.Highligthed);
        }

        public void SetState(CellState state)
        {
            _colorTween = state switch
            {
                CellState.Default => spriteRenderer.DOColor(Color.white, 0.2f),
                CellState.Selected => spriteRenderer.DOColor(Color.gray, 0.2f),
                CellState.Highligthed => spriteRenderer.DOColor(Color.cyan, 0.2f),
                _ => throw null
            };
        }
    }
}
