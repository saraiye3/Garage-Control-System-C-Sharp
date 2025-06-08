using System;
using System.Collections.Generic;

namespace Ex03.GarageLogic
{
    public sealed class Motorcycle : Vehicle
    {
        const int k_TireAirPressure = 32;
        const float k_MaxFuel = 6.2f;
        const float k_MaxBattery = 2.9f;
        public enum eLicenseType { A1, A2, B1, B2 };
        private eLicenseType m_LicenseType;
        private int m_EngineDisplacement;

        public eLicenseType LicenseType
        {
            get => m_LicenseType;
            set => m_LicenseType = value;
        }

        public int EngineDisplacement
        {
            get => m_EngineDisplacement;
            set
            {
                if (value <= 0 || value > 3000) 
                {
                    throw new ValueOutOfRangeException(1, 3000, "Engine displacement is out of range.");
                }

                m_EngineDisplacement = value;
            }
        }

        public Motorcycle(string i_LicensePlateNumber, EnergySource i_EnergySource)
            : base(i_LicensePlateNumber, i_EnergySource)
        {
        }

        public override string ToString()
        {
            return string.Format(
                "{0}License Type: {1}\nEngine Displacement: {2} cc",
                base.ToString(),
                m_LicenseType,
                m_EngineDisplacement
            );
        }

        protected override void SetVehicleDetailsList()
        {
            base.SetVehicleDetailsList();
            VehicleDetails.Add("license type");
            VehicleDetails.Add("engine displacement");
        }

        public override void UpdateVehicleDetails(List<string> i_VehicleDetails)
        {
            base.UpdateVehicleDetails(i_VehicleDetails);

            if (i_VehicleDetails.Count >= 6)
            {
                if (Enum.TryParse(i_VehicleDetails[4], true, out eLicenseType licenseType))
                {
                    m_LicenseType = licenseType;
                }
                else
                {
                    throw new FormatException($"Invalid input format for license type: {i_VehicleDetails[4]}.");
                }

                if (int.TryParse(i_VehicleDetails[5], out int engineDisplacement))
                {
                    if (engineDisplacement <= 0 || engineDisplacement > 3000)
                    {
                        throw new ValueOutOfRangeException(1, 3000, "Engine displacement is out of range.");
                    }

                    m_EngineDisplacement = engineDisplacement;
                }
                else
                {
                    throw new FormatException($"Invalid input format for engine displacement: {i_VehicleDetails[5]}.");
                }
            }
            else
            {
                throw new ArgumentException("Insufficient details provided to update the motorcycle.");
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
            Tires = new List<Tire>((int)eNumOfTires.MotorcycleTiresNum);
            for (int i = 0; i < (int)eNumOfTires.MotorcycleTiresNum; ++i)
            {
                Tires.Add(new Tire(k_TireAirPressure));
            }
        }
    }
}
