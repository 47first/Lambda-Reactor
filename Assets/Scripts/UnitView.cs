using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Runtime
{
    public sealed class UnitView : MonoBehaviour
    {
        [Inject] private IEnvironmentController _environmentController;
        [Inject] private IQueryController _queryController;

        [field: SerializeField] public Cell Cell { get; private set; }

        public UnitPresenter Presenter { get; private set; }

        public int Initiative { get; set; }

        public void Activate() => Presenter?.Activate();

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
