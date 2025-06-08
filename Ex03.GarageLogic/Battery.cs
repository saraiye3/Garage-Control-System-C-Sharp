using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Ex03.GarageLogic.Fuel;

namespace Ex03.GarageLogic
{
    sealed public class Battery : EnergySource
    {
        public override string ToString()
        {
            return
                $"{base.ToString()} (Battery Powered)";
        }
    }
}
