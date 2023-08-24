using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Runtime
{
    public class QueryController : IQueryController
    {
        public QueryController(IEnvironmentController environmentController)
        {
            _environmentController = environmentController;
        }

        private IEnvironmentController _environmentController;
        private List<UnitView> _handledInitiativeUnits = new List<UnitView>();
        private int _initiative;

        public void Next()
        {
            var nextUnit = GetNextInitiativeUnit();
            Debug.Log($"{nextUnit.name} Turn");
            nextUnit.Activate();
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
    }
}
