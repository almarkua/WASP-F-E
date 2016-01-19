using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WASP_F_E.Models
{
    class MainContext
    {
        private static StudyContext _context;

        public static StudyContext GetInstance()
        {
            if (_context == null) _context = new StudyContext();
            return _context;
        }

        public static void SaveChanges()
        {
            Thread thread = new Thread(DoSaveChanges);
            thread.Start();
        }

        private static void DoSaveChanges()
        {
            _context.SaveChangesAsync();
        }
    }
}
