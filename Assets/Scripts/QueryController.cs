using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Runtime
{
    public class QueryController
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
            Debug.Log($"Next Unit - {nextUnit.name}");

            nextUnit.State = UnitState.Active;
        }

        private UnitView GetNextInitiativeUnit()
        {
            var nextUnit = GetNextUnhandledUnit();

            if (nextUnit is null)
            {
                _handledInitiativeUnits.Clear();

                nextUnit = GetBiggerInitiativeUnit();

                if (nextUnit is null)
                    nextUnit = GetLowestInitiativeUnit();

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

        private UnitView GetLowestInitiativeUnit()
        {
            return _environmentController.Units.OrderBy(unit => unit.Initiative).First();
        }

        private UnitView GetBiggerInitiativeUnit()
        {
            return _environmentController.Units.FirstOrDefault(unit => unit.Initiative > _initiative);
        }
    }
}
