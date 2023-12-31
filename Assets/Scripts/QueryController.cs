using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Runtime
{
    public class QueryController : IQueryController
    {
        public QueryController(IEnvironmentController environmentController, IGameView gameView)
        {
            _environmentController = environmentController;
            _gameView = gameView;
        }

        private IGameView _gameView;
        private IEnvironmentController _environmentController;
        private List<UnitView> _handledInitiativeUnits = new List<UnitView>();
        private IQueryObservable _queryObservable;
        private int _initiative;

        public void Next()
        {
            if (IsEndOfGame())
            {
                _gameView.ShowResult();
                return;
            }

            var nextUnit = GetNextInitiativeUnit();

            Debug.Log($"{nextUnit.name} Turn");

            _queryObservable = nextUnit.Presenter;
            _queryObservable?.Activate(() => RequestNext(nextUnit.Presenter));
        }

        private bool IsEndOfGame()
        {
            return _environmentController.Units.Any(unit => unit.Presenter.Team == Team.Left) == false ||
                _environmentController.Units.Any(unit => unit.Presenter.Team == Team.Right) == false;
        }

        private UnitView GetNextInitiativeUnit()
        {
            var nextUnit = GetNextUnhandledUnit();

            if (nextUnit is null)
            {
                _handledInitiativeUnits.Clear();

                nextUnit = GetLessInitiativeUnit();

                if (nextUnit is null)
                    nextUnit = GetBiggestInitiativeUnit();

                _initiative = nextUnit.Initiative;
            }
            
            _handledInitiativeUnits.Add(nextUnit);

            return nextUnit;
        }

        private UnitView GetNextUnhandledUnit()
        {
            return _environmentController.Units.FirstOrDefault(unit => unit.Initiative == _initiative &&
            _handledInitiativeUnits.Contains(unit) == false);
        }

        private UnitView GetBiggestInitiativeUnit()
        {
            return _environmentController.Units.OrderByDescending(unit => unit.Initiative).First();
        }

        private UnitView GetLessInitiativeUnit()
        {
            return _environmentController.Units.FirstOrDefault(unit => unit.Initiative < _initiative);
        }

        private void RequestNext(IQueryObservable queryObservable)
        {
            if (queryObservable != _queryObservable)
                throw new InvalidOperationException("Expired request");

            Next();
        }
    }
}
