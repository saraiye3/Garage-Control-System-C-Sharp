using Ex03.GarageLogic;
using System.Collections.Generic;
using System;

namespace Ex03.GarageLogic
{
    public sealed class Truck : Vehicle
    {
        const int k_MaxFuel = 125;
        const int k_TireAirPressure = 29;
        private bool m_IsCoolingTruck;
        private float m_LoadCapacity;

        public bool IsCoolingTruck
        {
            get => m_IsCoolingTruck;
            set => m_IsCoolingTruck = value;
        }

        public float LoadCapacity
        {
            get => m_LoadCapacity;
            set
            {
                if (value < 0 || value > 50000)
                {
                    throw new ValueOutOfRangeException(0, 50000, "Load capacity is out of range.");
                }

                m_LoadCapacity = value;
            }
        }

        public Truck(string i_LicensePlateNumber, EnergySource i_EnergySource)
            : base(i_LicensePlateNumber, i_EnergySource)
        {
        }

        public override string ToString()
        {
            return string.Format(
    "{0}\nCooling Truck: {1}\nLoad Capacity: {2} kg",
    base.ToString(),
    IsCoolingTruck,
    LoadCapacity
);
        }

        protected override void SetVehicleDetailsList()
        {
            base.SetVehicleDetailsList();
            VehicleDetails.Add("true if truck is a cooling truck and false otherwise");
            VehicleDetails.Add("load capacity");
        }

        public override void UpdateVehicleDetails(List<string> i_VehicleDetails)
        {
            base.UpdateVehicleDetails(i_VehicleDetails);

            if (i_VehicleDetails.Count >= 6)
            {
                if (bool.TryParse(i_VehicleDetails[4], out bool isCooling))
                {
                    m_IsCoolingTruck = isCooling;
                }
                else
                {
                    throw new FormatException("Invalid input format for cooling truck indicator.");
                }

                if (float.TryParse(i_VehicleDetails[5], out float loadCapacity))
                {
                    if (loadCapacity < 0 || loadCapacity > 50000)
                    {
                        throw new ValueOutOfRangeException(0, 50000, "Load capacity is out of range.");
                    }

                    m_LoadCapacity = loadCapacity;
                }
                else
                {
                    throw new FormatException("Invalid input format for load capacity.");
                }
            }
            else
            {
                throw new ArgumentException("Insufficient details provided to update the truck.");
            }
        }

        protected override void SetMaxEnergy()
        {
            EnergySource.SetMaxEnergy(k_MaxFuel);
        }

        protected override void InitializeTires()
        {
            Tires = new List<Tire>((int)eNumOfTires.TruckTiresNum);
            for (int i = 0; i < (int)eNumOfTires.TruckTiresNum; ++i)
            {
                Tires.Add(new Tire(k_TireAirPressure));
            }
        }
    }
}
