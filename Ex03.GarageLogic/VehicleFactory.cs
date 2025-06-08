using System;
using System.Collections.Generic;

namespace Ex03.GarageLogic
{
    public sealed class VehicleFactory
    {
        public static readonly Dictionary<int, string> sr_VehicleTypes = new Dictionary<int, string>
        {
            { 1, "Electric Car" },
            { 2, "Fuel Powered Car" },
            { 3, "Electric Motorcycle" },
            { 4, "Fuel Powered Motorcycle" },
            { 5, "Truck" }
        };

        public static void CreateANewVehicle(string i_LicensePlateNumber, string i_VehicleType, ref Garage.RegisteredVehicle io_RegisteredVehicle)
        {
            switch (i_VehicleType)
            {
                case "Electric Car":
                    io_RegisteredVehicle.Vehicle = new Car(i_LicensePlateNumber, new Battery());
                    break;
                case "Fuel Powered Car":
                    io_RegisteredVehicle.Vehicle = new Car(i_LicensePlateNumber, new Fuel(Fuel.eFuelType.Octan95));
                    break;
                case "Electric Motorcycle":
                    io_RegisteredVehicle.Vehicle = new Motorcycle(i_LicensePlateNumber, new Battery());
                    break;
                case "Fuel Powered Motorcycle":
                    io_RegisteredVehicle.Vehicle = new Motorcycle(i_LicensePlateNumber, new Fuel(Fuel.eFuelType.Octan98));
                    break;
                case "Truck":
                    io_RegisteredVehicle.Vehicle = new Truck(i_LicensePlateNumber, new Fuel(Fuel.eFuelType.Soler));
                    break;
                default:
                    throw new ArgumentException($"Invalid vehicle type: {i_VehicleType}");
            }
        }

    }
}
