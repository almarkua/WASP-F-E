namespace WASP_F_E.Models
{
    public class HPlant
    {
        public int HPlantId { get; set; }
        public string Name { get; set; }
        public double LordPosPl { get; set; }
        public double U { get; set; }
        public double CapacityBase { get; set; }
        public double CapacityPeak { get; set; }
        public double CapacityTotal { get; set; }
        public double EnergyBase { get; set; }
        public double EnergyPeak { get; set; }
        public double EnergyTotal { get; set; }
        public double PeakMineng { get; set; }
        public double EnergySpilled { get; set; }
        public double EnergyShortage { get; set; }
        public double CapacityFactor { get; set; }
        public double OaM { get; set; }

        public virtual Scenario Scenario { get; set; }
    }
}
