using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WASP_F_E.Models
{
    public class PlantType
    {
        public int PlantTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int StudyId { get; set; }

        public virtual List<Plant> Plants { get; set; }
        public virtual Study Study { get; set; }

        public PlantType()
        {
            Name = String.Empty;
        }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            if (obj != null)
                if (obj.GetType() == GetType()) return Name.Equals((obj as PlantType).Name);
            return false;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
