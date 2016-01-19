using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.TextFormatting;

namespace WASP_F_E.Models
{
    class Fixsys
    {
        string _file;

        public Fixsys(string file)
        {
            _file = file;
        }
        
        /*
        Parse data from fixsys file

        Example of table:
                                                  HEAT RATE      FUEL COSTS         S  FRCD                           FLD     UNIT GENERATION
                       NO. MIN.   CAP-   BASE   AVGE       CENTS/           P OUTAGE DAYS  MAIN  O&M   O&M  HEAT RT    COSTS ($/MWH)
          SEQ.         OF  LOAD   CITY   LOAD   INCR    MILLION KCAL  FUEL  R  RATE  SCHL  CLAS (FIX) (VAR)  KCAL/  BASE BASE  FLD FLD   FLD
          NO.    NAME SETS  MW     MW     KCAL/ KWH    DMSTC   FORGN  TYPE  %    %   MAIN   MW  $/KWM $/MWH   KWH   DOM  FRGN  DOM FRGN  TOT
         
           3     zp1    4   31.   300.   2700.  2335. 2353.0     0.0    3   2   0.0   30   300.  4.00  5.00  2373.  68.5  0.0 60.8  0.0 60.8
           4     kr1   10    6.   282.   2574.  2145. 2364.0     0.0    3  10   0.0   30   282.  4.00  5.00  2154.  65.8  0.0 55.9  0.0 55.9
           5     pd1    4   13.   150.   2800.  2450. 2516.0     0.0    3   2   0.0   30   150.  4.00  5.00  2479.  75.4  0.0 67.4  0.0 67.4
           6     pd2    4   22.   300.   2940.  2450. 2516.0     0.0    3  10   0.0   30   300.  4.00  5.00  2486.  79.0  0.0 67.5  0.0 67.5
           7     sl1    1    1.    80.   2914.  2429. 2407.0     0.0    3  10 100.0   30    80.  4.00  5.00  2435.  75.1  0.0 63.6  0.0 63.6
        */
        public List<Plant> Parse()
        {
            List<Plant> result = new List<Plant>();
            string[] allFileLines = File.ReadAllLines(_file);
            bool start = false;
            for (int i = 0; i < allFileLines.Length; i++)
            {
                if (allFileLines[i].IndexOf("NGROUPLM IPNLT PNLTLOLP PNLTENS") >= 0) break;
                if (start)
                {
                    string[] tmp = allFileLines[i].Trim().Split(' ');
                    tmp = Mersim.DeleteEmpty(tmp);
                    Plant tmpPlant = new Plant();
                    tmpPlant.ShortName = tmp[0];
                    tmpPlant.Name = tmp[0];
                    double averageEff, heatValue;
                    Double.TryParse(tmp[5].Replace('.',','), out averageEff);
                    Double.TryParse(tmp[15].Replace('.',','), out heatValue);
                    tmpPlant.AverageEfficiency = averageEff;
                    tmpPlant.HeatValue = heatValue;
                    result.Add(tmpPlant);
                    continue;
                }
                if (allFileLines[i].IndexOf("NAME SETS  MW     MW") >= 0)
                {
                    start = true;
                    i++;
                }
            }
            return result;
        }
    }
}
