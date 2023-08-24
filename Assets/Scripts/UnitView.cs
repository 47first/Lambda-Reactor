using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace Runtime
{
    public sealed class UnitView : MonoBehaviour
    {
        [Inject] private IEnvironmentController _environmentController;
        [Inject] private IQueryController _queryController;
        [Inject] private IGameView _gameView;

        [field: Header("View Settings")]
        [field: SerializeField] public Cell Cell { get; private set; }
        [SerializeField] private TextMeshProUGUI _stackLabel;

        [Header("Unit Settings")]
        [SerializeField] private UnitType _type;
        [SerializeField] private Team _team;
        [SerializeField] private int _stack;

        [Header("Sceleton")]
        [SerializeField] private UnitView _sceletonPrefab;

        public UnitPresenter Presenter { get; private set; }

        public int Initiative => Presenter.Initiative;

        public void Setup(IEnvironmentController environmentController,
            IQueryController queryController,
            IGameView gameView)
        {
            _environmentController = environmentController;
            _queryController = queryController;
            _gameView = gameView;

            InitializePresenter();
        }

        public void MoveTo(Cell cell)
        {
            Cell = cell;
            transform.DOMove(cell.transform.position, 0.5f);
        }

        public void UpdateStackValue(int stackCount) => _stackLabel.text = stackCount.ToString();

        internal void Disapear() => gameObject.SetActive(false);

        private void Awake() => InitializePresenter();

        private void InitializePresenter()
        {
            Presenter = _type switch
            {
                UnitType.Knight => new KnightPresenter(this, _environmentController,
                _queryController, _gameView, _team, _stack),

                UnitType.Shooter => new ShooterPresenter(this, _environmentController,
                _queryController, _gameView, _team, _stack),

                UnitType.MrBeast => throw null,

                UnitType.Sceleton => new SceletonPresenter(this, _environmentController,
                _queryController, _gameView, _team, _stack, _sceletonPrefab),

                UnitType.Zombie => new ZombiePresenter(this, _environmentController,
                _queryController, _gameView, _team, _stack),

                UnitType.Gus => throw null
            };
        }
    }
}
