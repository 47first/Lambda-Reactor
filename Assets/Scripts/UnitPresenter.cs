using System;
using UnityEngine;

namespace Runtime
{
    public class UnitPresenter : IDamageable
    {
        private UnitView _view;
        private IEnvironmentController _environmentController;
        private IQueryController _queryController;
        private Team _team;
        private float _damage;
        private int _stack;

        public UnitPresenter(UnitView view,
            IEnvironmentController environmentController,
            IQueryController queryController)
        {
            _view = view;
            _environmentController = environmentController;
            _queryController = queryController;

            _stack = 30;
            _damage = 15;
            UpdateStackValue();
        }

        private void UpdateStackValue() => _view.UpdateStackValue(_stack);

        public void Activate()
        {
            _view.Cell.Highlighted = true;
            _environmentController.CellClicked += OnCellClicked;
        }

        public void ReceiveDamage(float damage)
        {
            _stack = Mathf.Max(_stack - (int)damage, 0);

            Debug.Log("Receive Damage");

            if (_stack <= 0)
                _view.Disapear();
            else
                UpdateStackValue();
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
            else
            {
                if (unitAtClickedCell.Presenter is IDamageable damageable)
                    damageable?.ReceiveDamage(_damage);

                _view.Cell.Highlighted = false;
                _queryController.Next();
            }
        }
    }
}
