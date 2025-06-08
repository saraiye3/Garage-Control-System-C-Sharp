using System;

namespace Ex03.GarageLogic
{
    public sealed class Tire
    {
        private string m_Manufacturer;
        private float m_MaxAirPressure;
        private float m_CurrentAirPressure;

        public string Manufacturer
        {
            get => m_Manufacturer;
            set => m_Manufacturer = value;
        }

        public float MaxAirPressure
        {
            get => m_MaxAirPressure;
        }

        public float CurrentAirPressure
        {
            get => m_CurrentAirPressure;
            set
            {
                if (value < 0 || value > m_MaxAirPressure)
                {
                    throw new ValueOutOfRangeException(0, m_MaxAirPressure, "Air pressure is out of range.");
                }

                m_CurrentAirPressure = value;
            }
        }

        public Tire(float i_MaxAirPressure)
        {
            if (i_MaxAirPressure <= 0)
            {
                throw new ValueOutOfRangeException(0.1f, float.MaxValue, "Max air pressure must be greater than zero.");
            }

            m_MaxAirPressure = i_MaxAirPressure;
        }

        public void InflateTire(float i_AirToAdd)
        {
            if (m_CurrentAirPressure + i_AirToAdd > m_MaxAirPressure)
            {
                throw new ValueOutOfRangeException(0, m_MaxAirPressure - m_CurrentAirPressure, "Cannot inflate tire beyond its maximum air pressure.");
            }

            m_CurrentAirPressure += i_AirToAdd;
        }

        public override string ToString()
        {
            return $"Manufacturer: {Manufacturer}, Current Air Pressure: {CurrentAirPressure}";
        }
    }
}
