using System;

namespace Runtime
{
    public abstract class UnitPresenter : IQueryObservable
    {
        private int _stack;
        protected IEnvironmentController EnvironmentController { get; private set; }
        protected IQueryController QueryController { get; private set; }
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
            Team team,
            int stack)
        {
            EnvironmentController = environmentController;
            QueryController = queryController;

            View = view;
            Stack = stack;
            Team = team;
        }

        public virtual void Activate(Action next) => next?.Invoke();
    }
}
