using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WASP_F_E.Models
{
    class StudyInitializer:DropCreateDatabaseIfModelChanges<StudyContext>
    {
        protected override void Seed(StudyContext context)
        {
            context.SaveChanges();
        }
    }
}
