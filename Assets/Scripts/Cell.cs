using DG.Tweening;
using System;
using UnityEngine;

namespace Runtime
{
    public sealed class Cell : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        private Tween _colorTween;
        private CellState _state = CellState.Active;

        public event Action<Cell> Clicked;
        public event Action<Cell> Selected;

        [field: SerializeField] public Vector2 Position { get; set; }

        public void SetState(CellState state)
        {
            if (_state == state)
                return;

            Debug.Log($"{_state} | {state}");

            _state = state;

            _colorTween?.Kill();
            _colorTween = _state switch
            {
                CellState.Active => spriteRenderer.DOColor(Color.white, 0.2f),
                CellState.Unactive => spriteRenderer.DOColor(Color.gray, 0.2f),
                CellState.Highligthed => spriteRenderer.DOColor(Color.cyan, 0.2f),
                _ => throw null
            };
        }

        public float Distance(Cell cell) => Vector2.Distance(Position, cell.Position);

        private void OnMouseDown()
        {
            if (Input.GetMouseButtonDown(0))
                Clicked?.Invoke(this);
        }

        private void OnMouseEnter() => Selected?.Invoke(this);

        private void OnDestroy() => _colorTween?.Kill();
    }
}
