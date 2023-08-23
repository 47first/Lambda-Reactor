using DG.Tweening;
using System;
using UnityEngine;

namespace Runtime
{
    public sealed class Cell : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        private Tween _colorTween;

        public event Action Clicked;

        private bool _highlighted;
        public bool Highlighted
        {
            get => _highlighted;
            set
            {
                if (_highlighted == value)
                    return;

                _highlighted = value;

                _colorTween?.Kill();
                if (_highlighted)
                    _colorTween = spriteRenderer.DOColor(Color.cyan, 0.2f);
                else
                    _colorTween = spriteRenderer.DOColor(Color.white, 0.2f);
            }
        }

        private void OnMouseEnter()
        {
            if (_highlighted)
                return;

            _colorTween?.Kill();
            _colorTween = spriteRenderer.DOColor(Color.gray, 0.2f);
        }


        private void OnMouseExit()
        {
            if (_highlighted)
                return;

            _colorTween?.Kill();
            _colorTween = spriteRenderer.DOColor(Color.white, 0.2f);
        }

        private void OnMouseDown()
        {
            if(Input.GetMouseButtonDown(0))
                Clicked?.Invoke();
        }

        private void OnDestroy()
        {
            _colorTween?.Kill();
        }
    }
}
