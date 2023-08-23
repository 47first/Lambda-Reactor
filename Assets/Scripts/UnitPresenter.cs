using System.Diagnostics;

namespace Runtime
{
    public class UnitPresenter
    {
        private UnitView _view;
        private IEnvironmentController _environmentController;
        private IQueryController _queryController;
        private Team _team;

        public UnitPresenter(UnitView view,
            IEnvironmentController environmentController,
            IQueryController queryController)
        {
            _view = view;
            _environmentController = environmentController;
            _queryController = queryController;
        }

        public void Activate()
        {
            _view.Cell.Highlighted = true;
            _environmentController.CellClicked += OnCellClicked;
        }

        private void OnCellClicked(Cell cell)
        {
            if (cell == _view.Cell)
                return;

            var unitAtClickedCell = _environmentController.GetUnitAt(cell);

            if (unitAtClickedCell is null)
            {
                _environmentController.CellClicked -= OnCellClicked;

                _view.Cell.Highlighted = false;
                _view.MoveTo(cell);
                _queryController.Next();
            }
        }
    }
}
