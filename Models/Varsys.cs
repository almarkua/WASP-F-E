using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WASP_F_E.Models
{
    class Varsys
    {
        string _file;
        public Varsys(string file) {
            _file = file;
        }

        //parse new plants from varsys file
        public  List<Plant> Parse() { 
            List<Plant> result=new List<Plant>();
            string[] allFileLines = File.ReadAllLines(_file);
            bool start = false;
            for (int i = 0; i < allFileLines.Length; i++) {
                if (allFileLines[i].IndexOf("NGROUPLM   EMISNAME    MEASIND") >= 0) break;
                if (start)
                {
                    Plant tmpPlant = new Plant();
                    string[] tmp = Mersim.DeleteEmpty(allFileLines[i].Trim().Split(' '));
                    double averageEfficiency, heatValue;
                    Double.TryParse(tmp[5].Replace('.', ','), out averageEfficiency);
                    Double.TryParse(tmp[15].Replace('.', ','), out heatValue);

                    tmpPlant.ShortName = tmp[0];
                    tmpPlant.Name = tmp[0];
                    tmpPlant.AverageEfficiency = averageEfficiency;
                    tmpPlant.HeatValue = heatValue;

                    result.Add(tmpPlant);
                    continue;
                }
                if (allFileLines[i].IndexOf("NAME SETS  MW     MW     KCAL/ KWH") >= 0)
                {
                    start = true;
                    i++;
                }
            }
            return result;
        }
    }
}
