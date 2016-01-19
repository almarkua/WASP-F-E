using System;
using System.Collections.Generic;

namespace WASP_F_E.Models
{
    public class Study
    {
        public int StudyId { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }

        public virtual List<Scenario> Scenarios { get; set; }
        public virtual List<PlantType> PlantTypes { get; set; }
        public virtual List<EmissionType> Emissions { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
