using static Ex03.GarageLogic.Fuel;
using Ex03.GarageLogic;
using System;

namespace Ex03.GarageLogic
{
    public abstract class EnergySource
    {
        private float m_EnergyPercentage;
        private float m_MaxEnergy;

        public float EnergyPercentage
        {
            get => m_EnergyPercentage;
            set
            {
                if (value < 0 || value > m_MaxEnergy)
                {
                    throw new ValueOutOfRangeException(0, m_MaxEnergy, "Energy percentage is out of range.");
                }

                m_EnergyPercentage = value;
            }
        }

        public float MaxEnergy
        {
            get => m_MaxEnergy;
        }

        public void SetMaxEnergy(float i_MaxEnergy)
        {
            m_MaxEnergy = i_MaxEnergy;
        }

        public virtual void AddEnergy(float i_AmountOfEnergyToAdd, eFuelType i_FuelType = eFuelType.None)
        {
            if (i_AmountOfEnergyToAdd < 0)
            {
                throw new ArgumentException("Amount of energy to add must be a positive value.");
            }

            if (m_EnergyPercentage + i_AmountOfEnergyToAdd > m_MaxEnergy)
            {
                throw new ValueOutOfRangeException(0, m_MaxEnergy - m_EnergyPercentage, "Energy addition exceeds maximum capacity.");
            }

            m_EnergyPercentage += i_AmountOfEnergyToAdd;
        }

        public override string ToString()
        {
            string msg;
            msg = $"Energy Left: {m_EnergyPercentage}, Max Energy: {m_MaxEnergy}";

            return msg;
        }
    }
}
