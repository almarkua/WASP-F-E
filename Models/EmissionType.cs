using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WASP_F_E.Models
{
    public class EmissionType
    {
        public int EmissionTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int StudyId { get; set; }

        public virtual Study Study { get; set; }
        public virtual List<EmissionValue> EmissionsValues { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
