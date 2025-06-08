using Ex03.GarageLogic;
using System.Collections.Generic;
using System;

namespace Ex03.GarageLogic
{
    public sealed class Car : Vehicle
    {
        const float k_MaxBattery = 5.4f;
        const int k_MaxFuel = 52;
        const int k_TireAirPressure = 34;
        public enum eColor { Black, Blue, White, Gray };
        public enum eNumOfDoors { Two = 2, Three = 3, Four = 4, Five = 5 };
        private eColor m_Color;
        private eNumOfDoors m_NumOfDoors;

        public eColor Color { get => m_Color; }
        public eNumOfDoors NumOfDoors { get => m_NumOfDoors; }

        public Car(string i_LicensePlateNumber, EnergySource i_EnergySource)
            : base(i_LicensePlateNumber, i_EnergySource)
        {
        }

        public override string ToString()
        {
            return string.Format(
                "{0}\nColor: {1}\nNumber of Doors: {2}",
                base.ToString(),
                Color,
                NumOfDoors
            );
        }

        protected override void SetVehicleDetailsList()
        {
            base.SetVehicleDetailsList();
            VehicleDetails.Add("color of car");
            VehicleDetails.Add("number of doors");
        }

        public override void UpdateVehicleDetails(List<string> i_VehicleDetails)
        {
            const bool ignoreCase = true;

            base.UpdateVehicleDetails(i_VehicleDetails);
            if (i_VehicleDetails.Count >= 6)
            {
                Model = i_VehicleDetails[0];

                if (!Enum.TryParse(i_VehicleDetails[4], ignoreCase, out eColor color))
                {
                    throw new FormatException($"Invalid input format for car color: {i_VehicleDetails[4]}.");
                }

                if (!Enum.TryParse(i_VehicleDetails[5], out eNumOfDoors doorsNum) ||
                    (doorsNum < eNumOfDoors.Two || doorsNum > eNumOfDoors.Five))
                {
                    throw new FormatException($"Invalid input format for number of doors: {i_VehicleDetails[5]}.");
                }

                m_Color = color;
                m_NumOfDoors = doorsNum;
            }
            else
            {
                throw new ArgumentException("Insufficient details provided to update the car.");
            }
        }

        protected override void SetMaxEnergy()
        {
            if (EnergySource is Fuel)
            {
                EnergySource.SetMaxEnergy(k_MaxFuel);
            }
            else if (EnergySource is Battery)
            {
                EnergySource.SetMaxEnergy(k_MaxBattery);
            }
        }

        protected override void InitializeTires()
        {
            Tires = new List<Tire>((int)eNumOfTires.CarTiresNum);
            for (int i = 0; i < (int)eNumOfTires.CarTiresNum; ++i)
            {
                Tires.Add(new Tire(k_TireAirPressure));
            }
        }
    }
}
