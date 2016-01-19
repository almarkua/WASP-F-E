using System.Collections.Generic;
using System.Dynamic;
using System.Windows.Documents;

namespace WASP_F_E.Models
{
    public class Plant
    {
        public int PlantId { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }
        public double AverageEfficiency { get; set; }
        public double HeatValue { get; set; }

        public virtual PlantType Type { get; set; }

        public virtual List<EmissionValue> Emissions { get; set; }

        public override string ToString()
        {
            return ShortName + " | " + Name;
        }
    }
}
