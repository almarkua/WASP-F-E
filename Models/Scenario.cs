using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WASP_F_E.Models
{
    public class Scenario
    {
        public int ScenarioId { get; set; }
        public int Period { get; set; }
        public int Year { get; set; }
        public int Hydrocondition { get; set; }
        public double Probability { get; set; }
        public double CapacityTotal { get; set; }
        public double CapacityReserve { get; set; }
        public double LoadMinimum { get; set; }
        public double LoadPeak { get; set; }
        public double MaintenanceSpace { get; set; }
        public double GenerationTotal { get; set; }
        public double EnergyDemand { get; set; }
        public double EnergyUnserved { get; set; }
        public double EnergyBalance { get; set; }
        public double EnergyPumped { get; set; }
        public double LossOfLoadProbability { get; set; }
        public int StudyId { get; set; }

        public virtual List<HPlant> HPlants { get; set; }
        public virtual List<TPlant> TPlants { get; set; }
        public virtual Study Study { get; set; }

       
    }
}
