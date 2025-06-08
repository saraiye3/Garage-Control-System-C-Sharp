using System;
using System.Collections.Generic;
using System.Linq;

namespace Ex03.GarageLogic
{
    public class Garage
    {
        private readonly Dictionary<string, RegisteredVehicle> r_VehicleRegistry = new Dictionary<string, RegisteredVehicle>();

        public class RegisteredVehicle
        {
            public enum eVehicleStatus { UnderRepair, Repaired, PaymentReceived }
            private string m_OwnerName;
            private string m_OwnerPhoneNumber;
            private eVehicleStatus m_VehicleStatus;

            public string OwnerName
            {
                get => m_OwnerName;
                set
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        throw new ArgumentException("Owner's name cannot be null, empty, or whitespace.");
                    }
                    m_OwnerName = value;
                }
            }

            public string OwnerPhoneNumber
            {
                get => m_OwnerPhoneNumber;
                set
                {
                    if (value.Length != 10 || !value.All(char.IsDigit))
                    {
                        throw new ArgumentException("Owner's phone number must be exactly 10 digits.");
                    }

                    m_OwnerPhoneNumber = value;
                }
            }

            public eVehicleStatus VehicleStatus
            {
                get => m_VehicleStatus;
                set
                {
                    if (!Enum.IsDefined(typeof(eVehicleStatus), value))
                    {
                        throw new ArgumentException("Invalid vehicle status.");
                    }
                    m_VehicleStatus = value;
                }
            }

            public Vehicle Vehicle { get; set; }

            public override string ToString()
            {
                return string.Format(
                    "Owner's Name: {0}\n" +
                    "Owner's Phone Number: {1}\n" +
                    "Status: {2}\n" +
                    "Vehicle Details:\n{3}",
                    OwnerName,
                    OwnerPhoneNumber,
                    VehicleStatus,
                    Vehicle
                );
            }
        }

        public void AddNewVehicleToGarage(RegisteredVehicle i_RegisteredVehicle)
        {
            if (i_RegisteredVehicle == null || string.IsNullOrEmpty(i_RegisteredVehicle.Vehicle.LicensePlateNumber))
            {
                throw new ArgumentException("Invalid vehicle details.");
            }

            if (r_VehicleRegistry.ContainsKey(i_RegisteredVehicle.Vehicle.LicensePlateNumber))
            {
                throw new InvalidOperationException("The vehicle is already in the garage.");
            }

            r_VehicleRegistry.Add(i_RegisteredVehicle.Vehicle.LicensePlateNumber, i_RegisteredVehicle);
        }

        public bool IsVehicleInGarage(string i_LicensePlateNumber)
        {
            if (string.IsNullOrEmpty(i_LicensePlateNumber))
            {
                throw new ArgumentException("License plate number cannot be null or empty.");
            }

            return r_VehicleRegistry.ContainsKey(i_LicensePlateNumber);
        }

        public RegisteredVehicle GetVehicleFromGarageRegistry(string i_LicensePlateNumber)
        {
            if (string.IsNullOrEmpty(i_LicensePlateNumber))
            {
                throw new ArgumentException("License plate number cannot be null or empty.");
            }

            if (!r_VehicleRegistry.TryGetValue(i_LicensePlateNumber, out RegisteredVehicle registeredVehicle))
            {
                throw new KeyNotFoundException("The vehicle is not found in the garage.");
            }

            return registeredVehicle;
        }

        public void UpdateVehicleStatus(string i_LicensePlateNumber, RegisteredVehicle.eVehicleStatus i_NewStatus)
        {
            RegisteredVehicle registeredVehicle = GetVehicleFromGarageRegistry(i_LicensePlateNumber);
            registeredVehicle.VehicleStatus = i_NewStatus;
        }

        public void InflateTiresToMaxPressure(string i_LicensePlateNumber)
        {
            RegisteredVehicle registeredVehicle = GetVehicleFromGarageRegistry(i_LicensePlateNumber);

            foreach (Tire tire in registeredVehicle.Vehicle.Tires)
            {
                tire.CurrentAirPressure = tire.MaxAirPressure;
            }
        }

        public void FuelUpOrChargeVehicle(string i_LicensePlateNumber, float i_AmountOfEnergyToAdd, Fuel.eFuelType i_FuelType)
        {
            RegisteredVehicle registeredVehicle = GetVehicleFromGarageRegistry(i_LicensePlateNumber);

            if (registeredVehicle.Vehicle.EnergySource is Fuel fuel)
            {
                if (fuel.FuelType != i_FuelType)
                {
                    throw new ArgumentException("Fuel type does not match the vehicle's fuel type.");
                }

                fuel.AddEnergy(i_AmountOfEnergyToAdd, i_FuelType);
            }
            else if (registeredVehicle.Vehicle.EnergySource is Battery battery)
            {
                if (i_FuelType != Fuel.eFuelType.None)
                {
                    throw new ArgumentException("Battery-powered vehicles do not run on fuel.");
                }

                battery.AddEnergy(i_AmountOfEnergyToAdd);
            }
            else
            {
                throw new InvalidOperationException("Unknown energy source type.");
            }
        }

        public List<string> FilterVehiclesByStatus(RegisteredVehicle.eVehicleStatus? i_VehicleStatus)
        {
            List<string> filteredLicensePlates = new List<string>();

            foreach (KeyValuePair<string, RegisteredVehicle> entry in r_VehicleRegistry)
            {
                if (i_VehicleStatus == null || entry.Value.VehicleStatus == i_VehicleStatus)
                {
                    filteredLicensePlates.Add(entry.Key);
                }
            }

            return filteredLicensePlates;
        }

        public void UpdateRegisteredVehicleDetails(RegisteredVehicle i_RegisteredVehicle, string i_OwnerName, string i_OwnerPhoneNumber)
        {
            if (string.IsNullOrEmpty(i_OwnerName))
            {
                throw new ArgumentException("Owner's name cannot be null or empty.");
            }

            if (string.IsNullOrEmpty(i_OwnerPhoneNumber))
            {
                throw new ArgumentException("Owner's phone number cannot be null or empty.");
            }

            i_RegisteredVehicle.OwnerName = i_OwnerName;
            i_RegisteredVehicle.OwnerPhoneNumber = i_OwnerPhoneNumber;
        }
    }
}
