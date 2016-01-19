using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WASP_F_E.Models
{
    public class EmissionValue
    {
        public int EmissionValueId { get; set; }
        public EmissionCalculationType CalculationType { get; set; }
        public double Value { get; set; }

        public virtual Plant Plant { get; set; }
        public virtual EmissionType Type { get; set; }
    }

    public enum EmissionCalculationType
    {
        FromFuel,FromEnergy
    }
}
