using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ex03.GarageLogic
{
    public sealed class Fuel : EnergySource
    {
        public enum eFuelType { Octan95, Octan96, Octan98, Soler, None };
        private eFuelType m_FuelType;

        public eFuelType FuelType { get => m_FuelType; }

        public Fuel(eFuelType i_FuelType) : base()
        {
            if (Enum.IsDefined(typeof(eFuelType), i_FuelType))
            {
                this.m_FuelType = i_FuelType;
            }
            else
            {
                throw new ArgumentException("Invalide type of fuel.");
            }

        }

        public override void AddEnergy(float i_amoutOfEnergyToAdd, eFuelType i_FuelType)
        {
            if (i_FuelType != this.m_FuelType)
            {
                throw new ArgumentException($"Invalid fuel type. Expected: {this.m_FuelType}, but got: {i_FuelType}.");
            }

            base.AddEnergy(i_amoutOfEnergyToAdd, i_FuelType);
        }

        public override string ToString()
        {
            return $"{base.ToString()}, Fuel Type: {m_FuelType}";
        }
    }
}
