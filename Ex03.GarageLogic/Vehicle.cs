using Ex03.GarageLogic;
using System.Collections.Generic;
using System;

public abstract class Vehicle
{
    public enum eNumOfTires { MotorcycleTiresNum = 2, CarTiresNum = 5, TruckTiresNum = 14 };
    private string m_Model;
    private readonly string r_LicensePlateNumber;
    private List<Tire> m_Tires;
    private readonly EnergySource r_EnergySource;
    private readonly List<string> r_VehicleDetails;

    public string Model
    {
        get { return m_Model; }
        set { m_Model = value; }
    }

    public string LicensePlateNumber { get => r_LicensePlateNumber; }
    public List<Tire> Tires { get => m_Tires; set { m_Tires = value; } }
    public EnergySource EnergySource { get => r_EnergySource; }
    public List<string> VehicleDetails { get => r_VehicleDetails; }
    protected abstract void SetMaxEnergy();
    protected abstract void InitializeTires();

    public Vehicle(string i_LicensePlateNumber, EnergySource i_EnergySource)
    {

        if (!int.TryParse(i_LicensePlateNumber, out int _))
        {
            throw new ArgumentException("License Plate must be numeric.");
        }
        r_LicensePlateNumber = i_LicensePlateNumber;
        InitializeTires();
        r_EnergySource = i_EnergySource;
        SetMaxEnergy();
        r_VehicleDetails = new List<string>();
        SetVehicleDetailsList();
    }

    private void setTires(string i_Manufacturer, float i_CurrentPressure)
    {
        foreach (Tire tire in m_Tires)
        {
            if (i_CurrentPressure < 0 || i_CurrentPressure > tire.MaxAirPressure)
            {
                throw new ValueOutOfRangeException(0, tire.MaxAirPressure, "Tire air pressure is out of range.");
            }

            tire.Manufacturer = i_Manufacturer;
            tire.CurrentAirPressure = i_CurrentPressure;
        }
    }

    public override string ToString()
    {
        string energySourceInfo = string.Empty;
        string tiresInfo = string.Empty;
        uint i = 1;

        foreach (Tire tire in Tires)
        {
            tiresInfo += $"{i}) {tire.ToString()} Max Air Pressure: {tire.MaxAirPressure}\n";
            i++;
        }

        if (EnergySource is Fuel fuel)
        {
            energySourceInfo = $"Fuel Type: {fuel.FuelType}\n";
        }

        string msg = string.Format(
      "Model Name: {0}\n" +
      "License Plate: {1}\n" +
      "Energy Source: {2}\n" +
      "Number of Tires: {3}\n" +
      "Tires Info:\n{4}",
      m_Model,
      r_LicensePlateNumber,
      EnergySource,
      Tires.Count,
      tiresInfo
  );
        return msg;
    }

    protected virtual void SetVehicleDetailsList()
    {
        VehicleDetails.Add("model of vehicle");
        VehicleDetails.Add("fuel or battery percentage");
        VehicleDetails.Add("current tire air pressure");
        VehicleDetails.Add("tire manufacturer");
    }

    public virtual void UpdateVehicleDetails(List<string> i_VehicleDetails)
    {
        if (i_VehicleDetails.Count >= 4)
        {
            Model = i_VehicleDetails[0];
            if (float.TryParse(i_VehicleDetails[1], out float energyPercentage))
            {
                if (energyPercentage < 0 || energyPercentage > EnergySource.MaxEnergy)
                {
                    throw new ValueOutOfRangeException(0, EnergySource.MaxEnergy, "Energy percentage is out of range.");
                }

                EnergySource.EnergyPercentage = energyPercentage;
            }
            else
            {
                throw new FormatException("Invalid input format for energy percentage.");
            }

            if (float.TryParse(i_VehicleDetails[2], out float currentTireAirPressure))
            {
                if (currentTireAirPressure < 0 || currentTireAirPressure > Tires[0].MaxAirPressure)
                {
                    throw new ValueOutOfRangeException(0, Tires[0].MaxAirPressure, "Tire air pressure is out of range.");
                }

                string tireManufacturer = i_VehicleDetails[3];
                setTires(tireManufacturer, currentTireAirPressure);
            }
            else
            {
                throw new FormatException("Invalid input format for tire air pressure.");
            }
        }
        else
        {
            throw new ArgumentException("Insufficient details provided to update the vehicle.");
        }
    }
}
