using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Runtime
{
    public class QueryController : MonoBehaviour
    {
        [Inject] private IEnvironmentController _environmentController;
        private List<UnitView> _handledInitiativeUnits = new List<UnitView>();
        private int _initiative;

        private UnitView GetNextInitiativeUnit(int curInitiative)
        {
            var nextUnit = GetNextUnhandledUnit();

            if (nextUnit is null)
            {
                _handledInitiativeUnits.Clear();

                nextUnit = GetBiggerInitiativeUnit();

                if (nextUnit is null)
                {
                    nextUnit = GetLowestInitiativeUnit();
                }

                _initiative = nextUnit.Initiative;
            }

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
