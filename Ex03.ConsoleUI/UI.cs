using System;
using System.Collections.Generic;
using Ex03.GarageLogic;

namespace Ex03.ConsoleUI
{
    public sealed class UI
    {
        private static readonly Garage sr_Garage = new Garage();
        private enum eOptions { AddVehicle = 1,
                                ListVehicles = 2, 
                                UpdateVehicleStatus = 3,
                                InflateTiresToMax = 4, 
                                Refuel = 5, 
                                Recharge = 6,
                                ViewDetails = 7, 
                                Exit = 8 };

        public static void StartProgram()
        {
            string input;
            int option;

            while (true)
            {
                try
                {
                    Console.Clear();
                    string menu = string.Format(
                        "=== Garage Management System ==={0}" +
                        "1. Add Vehicle to Garage{0}" +
                        "2. List Vehicles in Garage{0}" +
                        "3. Update Vehicle Status{0}" +
                        "4. Inflate Vehicle Tires to Maximum{0}" +
                        "5. Refuel Vehicle{0}" +
                        "6. Recharge Electric Vehicle{0}" +
                        "7. View Vehicle Details{0}" +
                        "8. Exit",
                        Environment.NewLine
                    );
                    Console.WriteLine(menu);
                    Console.Write("\nSelect an option: ");
                    input = Console.ReadLine();

                    if (!int.TryParse(input, out option))
                    {
                        throw new FormatException("Invalid input. Please enter a valid number.");
                    }

                    switch ((eOptions)option) 
                    {
                        case eOptions.AddVehicle:
                            addVehicle();
                            break;
                        case eOptions.ListVehicles:
                            listVehicles();
                            break;
                        case eOptions.UpdateVehicleStatus:
                            updateVehicleStatus();
                            break;
                        case eOptions.InflateTiresToMax:
                            inflateTiresToMax();
                            break;
                        case eOptions.Refuel:
                            refuelVehicle();
                            break;
                        case eOptions.Recharge:
                            rechargeVehicle();
                            break;
                        case eOptions.ViewDetails:
                            viewVehicleDetails();
                            break;
                        case eOptions.Exit:
                            Console.WriteLine("Exiting... Goodbye!");
                            return;
                        default:
                            throw new FormatException("Invalid option. Please select a valid menu option.");
                    }

                    Console.WriteLine("\nPress any key to return to main menu...");
                    Console.ReadKey();
                }
                catch (ValueOutOfRangeException ex)
                {
                    Console.WriteLine($"{ex.Message}");
                    Console.WriteLine("Press any key to try again...");
                    Console.ReadKey();
                }
                catch (FormatException ex)
                {
                    Console.WriteLine($"{ex.Message}");
                    Console.WriteLine("Press any key to try again...");
                    Console.ReadKey();
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"{ex.Message}");
                    Console.WriteLine("Press any key to try again...");
                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                    Console.WriteLine("Press any key to try again...");
                    Console.ReadKey();
                }
            }
        }

        private static void addVehicle()
        {
            Garage.RegisteredVehicle newRegisteredVehicle;

            Console.Clear();
            Console.WriteLine("=== Add Vehicle to Garage ===\n");
            Console.Write("Enter Vehicle License Number: ");
            string licenseNumber = Console.ReadLine();

            if (sr_Garage.IsVehicleInGarage(licenseNumber))
            {
                Console.WriteLine("The vehicle is already in the garage");
                newRegisteredVehicle = sr_Garage.GetVehicleFromGarageRegistry(licenseNumber);
                newRegisteredVehicle.VehicleStatus = Garage.RegisteredVehicle.eVehicleStatus.UnderRepair;
            }
            else
            {
                newRegisteredVehicle = new Garage.RegisteredVehicle();
                getRegisteredVehicleDetails(newRegisteredVehicle);
                string vehicleType = getVehicleTypeFromUser();

                VehicleFactory.CreateANewVehicle(licenseNumber, vehicleType, ref newRegisteredVehicle);

                getVehicleDetailsFromUser(newRegisteredVehicle.Vehicle);
                sr_Garage.AddNewVehicleToGarage(newRegisteredVehicle);
                Console.WriteLine("Vehicle successfully added to garage");
            }
        }

        private static void listVehicles()
        {
            Console.Clear();
            string output = string.Format("=== List of Vehicles in Garage ===\n");

            output += string.Format("Filter by status:\n");
            output += string.Format("1. {0}\n", "Under Repair");
            output += string.Format("2. {0}\n", "Repaired");
            output += string.Format("3. {0}\n", "Payment Received");
            output += string.Format("4. {0}\n", "All");
            output += string.Format("Enter choice: ");

            Console.Write(output);

            string choice = Console.ReadLine();
            Garage.RegisteredVehicle.eVehicleStatus? statusFilter = null;

            if (choice == "1")
            {
                statusFilter = Garage.RegisteredVehicle.eVehicleStatus.UnderRepair;
            }
            else if (choice == "2")
            {
                statusFilter = Garage.RegisteredVehicle.eVehicleStatus.Repaired;
            }
            else if (choice == "3")
            {
                statusFilter = Garage.RegisteredVehicle.eVehicleStatus.PaymentReceived;
            }
            else if (choice != "4")
            {
                throw new FormatException("Invalid choice.");
            }

            var filteredVehicles = sr_Garage.FilterVehiclesByStatus(statusFilter);
            if (filteredVehicles.Count == 0)
            {
                Console.WriteLine("No vehicles found for the selected status.");
            }
            else
            {
                foreach (var vehicle in filteredVehicles)
                {
                    Console.WriteLine(vehicle);
                }
            }
        }

        private static void updateVehicleStatus()
        {
            Console.Clear();
            Console.WriteLine("=== Update Vehicle Status ===\n");
            Console.Write("Enter Vehicle License Number: ");
            string licenseNumber = Console.ReadLine();

            if (!sr_Garage.IsVehicleInGarage(licenseNumber))
            {
                Console.WriteLine("Vehicle not found in garage.");
                return;
            }

            Console.WriteLine("Select New Status:");
            Console.WriteLine("1. Under Repair");
            Console.WriteLine("2. Repaired");
            Console.WriteLine("3. Payment Received");
            Console.Write("Enter choice: ");
            string choice = Console.ReadLine();

            Garage.RegisteredVehicle.eVehicleStatus newStatus;
            switch (choice)
            {
                case "1":
                    newStatus = Garage.RegisteredVehicle.eVehicleStatus.UnderRepair;
                    break;
                case "2":
                    newStatus = Garage.RegisteredVehicle.eVehicleStatus.Repaired;
                    break;
                case "3":
                    newStatus = Garage.RegisteredVehicle.eVehicleStatus.PaymentReceived;
                    break;
                default:
                    throw new FormatException("Invalid status choice.");
            }

            Garage.RegisteredVehicle vehicleInGarage = sr_Garage.GetVehicleFromGarageRegistry(licenseNumber);
            vehicleInGarage.VehicleStatus = newStatus;

            sr_Garage.UpdateVehicleStatus(licenseNumber, newStatus);

            Console.WriteLine("Vehicle status successfully updated");
        }

        private static void inflateTiresToMax()
        {
            Console.Clear();
            Console.WriteLine("=== Inflate Tires ===\n");
            Console.Write("Enter Vehicle License Number: ");
            string licenseNumber = Console.ReadLine();

            if (!sr_Garage.IsVehicleInGarage(licenseNumber))
            {
                Console.WriteLine("Vehicle not found in garage.");
                return;
            }

            sr_Garage.InflateTiresToMaxPressure(licenseNumber);
            Console.WriteLine("Tires successfully inflated to their maximum air pressure");
        }

        private static void refuelVehicle()
        {
            Console.Clear();
            Console.WriteLine("=== Refuel Vehicle ===\n");
            Console.Write("Enter Vehicle License Number: ");
            string licenseNumber = Console.ReadLine();

            if (!sr_Garage.IsVehicleInGarage(licenseNumber))
            {
                Console.WriteLine("Vehicle not found in garage.");
                return;
            }

            Console.Write("Enter amount of fuel to add: ");
            float fuelAmount = float.Parse(Console.ReadLine());
            Console.WriteLine("Enter fuel type: ");
            string input = Console.ReadLine();
            const bool v_IgnoreCase = true;

            if (Enum.TryParse(input, v_IgnoreCase, out Fuel.eFuelType o_fuelType))
            {
                sr_Garage.FuelUpOrChargeVehicle(licenseNumber, fuelAmount, o_fuelType);
                Console.WriteLine("Vehicle successfully refueled");
            }
            else
            {
                throw new ArgumentException("Invalid fuel type");
            }
        }

        private static void rechargeVehicle()
        {
            Console.Clear();
            Console.WriteLine("=== Recharge Vehicle ===\n");
            Console.Write("Enter Vehicle License Number: ");
            string licenseNumber = Console.ReadLine();

            if (!sr_Garage.IsVehicleInGarage(licenseNumber))
            {
                Console.WriteLine("Vehicle not found in garage.");
                return;
            }

            Console.Write("Enter number of minutes: ");
            float numOfMinutes = float.Parse(Console.ReadLine());

            sr_Garage.FuelUpOrChargeVehicle(licenseNumber, numOfMinutes, Fuel.eFuelType.None);
            Console.WriteLine("Vehicle successfully recharged");
        }

        private static void viewVehicleDetails()
        {
            Console.Clear();
            Console.WriteLine("=== View Vehicle Details ===\n");
            Console.Write("Enter Vehicle License Number: ");
            string licenseNumber = Console.ReadLine();

            if (!sr_Garage.IsVehicleInGarage(licenseNumber))
            {
                Console.WriteLine("Vehicle not found in garage.");
                return;
            }

            var vehicle = sr_Garage.GetVehicleFromGarageRegistry(licenseNumber);
            Console.WriteLine(vehicle);
        }

        private static void getRegisteredVehicleDetails(Garage.RegisteredVehicle i_RegisteredVehicle)
        {
            Console.WriteLine("Please enter the owner's name for the vehicle: ");
            string ownersName = Console.ReadLine();
            Console.WriteLine("Please enter the owner's phone number: ");
            string ownersPhoneNumber = Console.ReadLine();
            sr_Garage.UpdateRegisteredVehicleDetails(i_RegisteredVehicle, ownersName, ownersPhoneNumber);
        }

        private static void getVehicleDetailsFromUser(Vehicle i_Vehicle)
        {
            List<string> vehicleDetails = i_Vehicle.VehicleDetails;
            List<String> usersAnswers = new List<String>();

            foreach (string detail in vehicleDetails)
            {
                Console.WriteLine("Please enter {0}", detail);
                usersAnswers.Add(Console.ReadLine());
            }

            i_Vehicle.UpdateVehicleDetails(usersAnswers);
        }

        private static string getVehicleTypeFromUser()
        {
            int usersChoice;
            string vehicleType;

            Console.WriteLine("Please select the vehicle type:");
            foreach (var vehicle in VehicleFactory.sr_VehicleTypes)
            {
                Console.WriteLine($"{vehicle.Key}. {vehicle.Value}");
            }

            usersChoice = int.Parse(Console.ReadLine());

            if (VehicleFactory.sr_VehicleTypes.TryGetValue(usersChoice, out vehicleType))
            {
                return vehicleType;
            }
            else
            {
                throw new ArgumentException("Invalid vehicle type number.");
            }
        }
    }
}

