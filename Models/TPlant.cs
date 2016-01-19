namespace WASP_F_E.Models
{
    public class TPlant
    {
        public int TPlantId { get; set; }
        public string Name { get; set; }
        public int NumberOfUnits { get; set; }
        public double CapacityBase { get; set; }
        public double CapacityPeak { get; set; }
        public double CapacityTotal { get; set; }
        public double EnergyBase { get; set; }
        public double EnergyPeak { get; set; }
        public double EnergyTotal { get; set; }
        public double FuelDomestic { get; set; }
        public double FuelForeign { get; set; }
        public double FuelTotal { get; set; }
        public double MainProbability { get; set; }
        public double CapacityFactor { get; set; }
        public double OaM { get; set; }
        public double ForCell { get; set; }

        public virtual Scenario Scenario { get; set; }
    }
}
