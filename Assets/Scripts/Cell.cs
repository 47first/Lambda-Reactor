using DG.Tweening;
using System;
using UniRx;
using UniRx.Triggers;
using Unity.VisualScripting;
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
            SetState(CellState.Highligthed);
        }

        private void Start()
        {
            Debug.Log("Start");
        }

        public void SetState(CellState state)
        {
            _colorTween = state switch
            {
                CellState.Default => spriteRenderer.DOColor(Color.white, 0.5f),
                CellState.Selected => spriteRenderer.DOColor(Color.gray, 0.5f),
                CellState.Highligthed => spriteRenderer.DOColor(Color.cyan, 0.5f),
                _ => throw null
            };
        }
    }
}
