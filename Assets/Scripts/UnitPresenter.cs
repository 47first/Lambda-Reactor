using System;

namespace Runtime
{
    public class UnitPresenter
    {
        private UnitView _view;
        private UnitState _state;
        private IEnvironmentController _environmentController;
        private QueryController _queryController;

        public UnitPresenter(UnitView view,
            IEnvironmentController environmentController,
            QueryController queryController)
        {
            _view = view;
            _environmentController = environmentController;
            _queryController = queryController;
        }

        public void Activate()
        {
            _environmentController.CellClicked += OnCellClicked;
        }

        private void OnCellClicked(Cell cell)
        {
            if (cell == _view.Cell)
                return;

            _environmentController.CellClicked -= OnCellClicked;
            _view.MoveTo(cell);
            _queryController.Next();
        }
    }
}
