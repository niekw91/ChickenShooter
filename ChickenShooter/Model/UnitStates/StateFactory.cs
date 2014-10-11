using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChickenShooter.Model.UnitStates
{
    public class StateFactory
    {
        private static Dictionary<string, BaseUnitState> stateMap = new Dictionary<string, BaseUnitState>();

        public static void Assign(BaseUnitState state)
        {
            if (state != null)
                stateMap.Add(state.Name, state);
        }

        public static BaseUnitState GetState(string name)
        {
            if (stateMap.ContainsKey(name))
                return stateMap[name];
            return null;
        }
    }
}
