using DG.Tweening;
using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Runtime
{
    public sealed class Cell : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        private Tween _colorTween;
        private CellState _state = CellState.Active;

        public event Action Clicked;
        public event Action Selected;

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
            };
        }

        private void OnMouseDown()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Clicked?.Invoke();
            }
        }

        private void OnMouseEnter()
        {
            Selected?.Invoke();
        }

        private void OnDestroy()
        {
            _colorTween?.Kill();
        }

        private void Start()
        {
            SetState(_state);
        }
    }
}
