using DG.Tweening;
using System;
using UnityEngine;
using Zenject;

namespace Runtime
{
    public enum UnitState
    {
        Wait,
        Dead,
        Active
    }

    public sealed class UnitView : MonoBehaviour
    {
        [Inject] private IEnvironmentController _environmentController;
        private UnitState _state;
        [Inject] private QueryController _queryController;

        [field: SerializeField] public Cell Cell { get; private set; }

        public UnitPresenter Presenter { get; private set; }

        public UnitState State
        {
            get => UnitState.Active;
            set
            {
                Presenter.Activate();
            }
        }

        public int Initiative { get; set; }

        public void MoveTo(Cell cell)
        {
            Cell = cell;

            transform.DOMove(cell.transform.position, 0.5f);
        }

        private void Awake()
        {
            Presenter = new UnitPresenter(this, _environmentController, _queryController);
        }

        private void Start()
        {
            MoveTo(Cell);
        }
    }
}
