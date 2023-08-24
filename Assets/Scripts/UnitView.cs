using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

namespace Runtime
{
    public sealed class UnitView : MonoBehaviour
    {
        [Inject] private IEnvironmentController _environmentController;
        [Inject] private IQueryController _queryController;

        [field: Header("View Settings")]
        [field: SerializeField] public Cell Cell { get; private set; }
        [SerializeField] private TextMeshProUGUI _stackLabel;

        [Header("Unit Settings")]
        [SerializeField] private UnitType _type;
        [SerializeField] private Team _team;
        [SerializeField] private int _stack;

        public UnitPresenter Presenter { get; private set; }

        public int Initiative { get => Presenter.Initiative; }

        public void Activate() => Presenter?.Activate();

        public void MoveTo(Cell cell)
        {
            Cell = cell;
            transform.DOMove(cell.transform.position, 0.5f);
        }

        public void UpdateStackValue(int stackCount) => _stackLabel.text = stackCount.ToString();

        private void Awake() => InitializePresenter();

        private void InitializePresenter()
        {
            Presenter = _type switch
            {
                UnitType.Knight => new KnightPresenter(this, _environmentController, _queryController, _team, _stack),
                UnitType.Shooter => throw null,
                UnitType.MrBeast => throw null,
                UnitType.Sceleton => throw null,
                UnitType.Zombie => throw null,
                UnitType.Gus => throw null
            };
        }

        private void Start() => MoveTo(Cell);

        internal void Disapear() => gameObject.SetActive(false);
    }
}
