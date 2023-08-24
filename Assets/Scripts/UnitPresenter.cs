namespace Runtime
{
    public abstract class UnitPresenter
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
                UpdateStackValue();
            }
        }
        public Team Team { get; set; }

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

            UpdateStackValue();
        }

        public abstract void Activate();

        private void UpdateStackValue() => View.UpdateStackValue(Stack);
    }
}
