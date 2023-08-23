using UniRx;
using UnityEngine;

namespace Runtime
{
    public class PlayerController : MonoBehaviour
    {
        private Camera _camera;
        private Cell _selectedCell;

        private void Start()
        {
            Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButtonDown(0))
                .Subscribe(OnMouseDown);
        }

        private void OnMouseDown(long obj)
        {
            var screenRay = _camera.ScreenPointToRay(Input.mousePosition);

            //screenRay.
        }
    }
}
