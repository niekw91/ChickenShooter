using ChickenShooter.Model.UnitStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChickenShooter.Model
{
    public class BaseUnitState
    {
        public string Name { get; set; }

        public BaseUnitState(string name)
        {
            this.Name = name;
            StateFactory.Assign(this);
        }
    }
}
