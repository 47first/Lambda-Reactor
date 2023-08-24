using System;

namespace Runtime
{
    public abstract class UnitPresenter : IQueryObservable, IPassObserver
    {
        private int _stack;
        protected IEnvironmentController EnvironmentController { get; private set; }
        protected IQueryController QueryController { get; private set; }
        protected IGameView GameView { get; private set; }
        protected UnitView View { get; private set; }
        public int Stack
        {
            get => _stack;
            protected set
            {
                _stack = value;
                View.UpdateStackValue(Stack);
            }
        }

        public Team Team { get; protected set; }
        public int Health { get; protected set; }
        public int Range { get; protected set; }
        public int MinDamage { get; protected set; }
        public int MaxDamage { get; protected set; }
        public int Initiative { get; protected set; }

        public UnitPresenter(UnitView view,
            IEnvironmentController environmentController,
            IQueryController queryController,
            IGameView gameView,
            Team team,
            int stack)
        {
            EnvironmentController = environmentController;
            QueryController = queryController;
            GameView = gameView;

            View = view;
            Stack = stack;
            Team = team;
        }

        private System.Action _next;
        public void Activate(Action next)
        {
            _next = next;

            GameView.SetObserver(this);

            OnActivate();
        }

        protected virtual void OnActivate() => ExecuteNext();

        protected void ExecuteNext()
        {
            GameView.ResetObserver();

            _next?.Invoke();

            _next = null;
        }

        public void Pass() => ExecuteNext();
    }
}
