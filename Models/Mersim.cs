using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WASP_F_E.Models
{
    class Mersim
    {
        private string file;

        public Mersim(string tmpFile)
        {
            file = tmpFile;
        }
        /*
        Parse datatables from mersim files

        Example of table:
            3 zp1    4     1.0  300.0  1200.0    14.4  3359.2   3373.6  185475.6       0.0  185475.6  74467.8  58.9   0.0  32.1
            4 kr1   10     1.0  282.0  2820.0    18.2  4058.7   4077.0  206918.5       0.0  206918.5 155744.8  79.2   0.0  16.5
            5 pd1    4     1.0  150.0   600.0    11.8   419.2    431.0   26673.6       0.0   26673.6  30955.2  66.3   0.0   8.2
            6 pd2    4   200.0  300.0  1200.0  1171.2    36.8   1207.9   88899.1       0.0   88899.1  63639.7  83.3   0.0  11.5
        */

        public List<Scenario> Parse()
        {
            List<Scenario> scenarios = new List<Scenario>();
            List<string> fileLines = File.ReadAllLines(file).ToList();
            for (int i = 0; i < fileLines.Count; i++)
            {
                if (fileLines[i].Trim().IndexOf("HYDROPLANTS OPERATIONAL SUMMARY") >= 0)
                {
                    Scenario currentScenario = new Scenario();
                    currentScenario.HPlants = new List<HPlant>();
                    currentScenario.TPlants = new List<TPlant>();
                    string[] tmpHeaderData = DeleteEmpty(fileLines[i - 2].Trim().Split(' '));
                    int period, year, hydrocondition, probability;
                    int.TryParse(tmpHeaderData[1].Replace('.', ','), out period);
                    int.TryParse(tmpHeaderData[4].Replace('.', ','), out year);
                    tmpHeaderData = DeleteEmpty(fileLines[i - 1].Trim().Split(' '));
                    int.TryParse(tmpHeaderData[1].Replace('.', ','), out hydrocondition);
                    int.TryParse(tmpHeaderData[3].Replace('.', ','), out probability);
                    currentScenario.Period = period;
                    currentScenario.Year = year;
                    currentScenario.Hydrocondition = hydrocondition;
                    currentScenario.Probability = probability;

                    i += 5;
                    int numberOfUnits;
                    double lordPosPl,
                        u,
                        capacityBase,
                        capacityPeak,
                        capacityTotal,
                        energyBase,
                        energyPeak,
                        energyTotal,
                        peakMineng,
                        energySpilled,
                        energyShortage,
                        capacityFactor,
                        oaM,
                        fuelDomestic,
                        fuelForeign,
                        fuelTotal,
                        mainProbability,
                        forCell;
                    for (int j = i; j < i + 2; j++)
                    {
                        string[] tmp = DeleteEmpty(fileLines[j].Trim().Replace('.', ',').Split(' '));
                        HPlant tmpHPlant = new HPlant();

                        int.TryParse(tmp[2], out numberOfUnits);
                        double.TryParse(tmp[3], out lordPosPl);
                        double.TryParse(tmp[4], out u);
                        double.TryParse(tmp[5], out capacityBase);
                        double.TryParse(tmp[6], out capacityPeak);
                        double.TryParse(tmp[7], out capacityTotal);
                        double.TryParse(tmp[8], out energyBase);
                        double.TryParse(tmp[9], out energyPeak);
                        double.TryParse(tmp[10], out energyTotal);
                        double.TryParse(tmp[11], out peakMineng);
                        double.TryParse(tmp[12], out energySpilled);
                        double.TryParse(tmp[13], out energyShortage);
                        double.TryParse(tmp[14], out oaM);
                        double.TryParse(tmp[15], out capacityFactor);

                        tmpHPlant.Name = tmp[1];
                        tmpHPlant.LordPosPl = lordPosPl;
                        tmpHPlant.U = u;
                        tmpHPlant.CapacityBase = capacityBase;
                        tmpHPlant.CapacityPeak = capacityPeak;
                        tmpHPlant.CapacityTotal = capacityTotal;
                        tmpHPlant.EnergyBase = energyBase;
                        tmpHPlant.EnergyPeak = energyPeak;
                        tmpHPlant.EnergyTotal = energyTotal;
                        tmpHPlant.PeakMineng = peakMineng;
                        tmpHPlant.EnergySpilled = energySpilled;
                        tmpHPlant.EnergyShortage = energyShortage;
                        tmpHPlant.OaM = oaM;
                        tmpHPlant.CapacityFactor = capacityFactor;

                        currentScenario.HPlants.Add(tmpHPlant);
                    }
                    i += 8;

                    while (fileLines[i].Trim().IndexOf("TOTALS") < 0)
                    {
                        TPlant tmpTPlant = new TPlant();
                        string[] tmp = DeleteEmpty(fileLines[i].Trim().Replace('.',',').Split(' '));
                        int.TryParse(tmp[2], out numberOfUnits);
                        double.TryParse(tmp[3], out capacityBase);
                        double.TryParse(tmp[4], out capacityPeak);
                        double.TryParse(tmp[5], out capacityTotal);
                        double.TryParse(tmp[6], out energyBase);
                        double.TryParse(tmp[7], out energyPeak);
                        double.TryParse(tmp[8], out energyTotal);
                        double.TryParse(tmp[9], out fuelDomestic);
                        double.TryParse(tmp[10], out fuelForeign);
                        double.TryParse(tmp[11], out fuelTotal);
                        double.TryParse(tmp[12], out oaM);
                        double.TryParse(tmp[13], out mainProbability);
                        double.TryParse(tmp[14], out forCell);
                        double.TryParse(tmp[15], out capacityFactor);

                        tmpTPlant.Name = tmp[1];
                        tmpTPlant.NumberOfUnits = numberOfUnits;
                        tmpTPlant.CapacityBase = capacityBase;
                        tmpTPlant.CapacityPeak = capacityPeak;
                        tmpTPlant.CapacityTotal = capacityTotal;
                        tmpTPlant.EnergyBase = energyBase;
                        tmpTPlant.EnergyPeak = energyPeak;
                        tmpTPlant.EnergyTotal = energyTotal;
                        tmpTPlant.FuelDomestic = fuelDomestic;
                        tmpTPlant.FuelForeign = fuelForeign;
                        tmpTPlant.FuelTotal = fuelTotal;
                        tmpTPlant.OaM = oaM;
                        tmpTPlant.MainProbability = mainProbability;
                        tmpTPlant.ForCell = forCell;
                        tmpTPlant.CapacityFactor = capacityFactor;
                        i++;
                        currentScenario.TPlants.Add(tmpTPlant);
                    }

                    i += 17;
                    double totalCapacity,
                            peakLoad,
                            minimumLoad,
                            maintenanceSpace,
                            reserveCapacity,
                            totalGeneration,
                            energyDemand,
                            unservedEnergy,
                            energyBalance,
                            lossOfLoadProbability,
                            energyPumped;
                    string[] tmpTotals = DeleteEmpty(fileLines[i].Trim().Split(' '));
                    double.TryParse(tmpTotals[3].Replace('.', ','), out totalCapacity);
                    double.TryParse(tmpTotals[7].Replace('.', ','), out totalGeneration);
                    i++;
                    tmpTotals = DeleteEmpty(fileLines[i].Trim().Split(' '));
                    double.TryParse(tmpTotals[3].Replace('.', ','), out peakLoad);
                    double.TryParse(tmpTotals[7].Replace('.', ','), out energyDemand);
                    i++;
                    tmpTotals = DeleteEmpty(fileLines[i].Trim().Split(' '));
                    double.TryParse(tmpTotals[3].Replace('.', ','), out minimumLoad);
                    double.TryParse(tmpTotals[7].Replace('.', ','), out unservedEnergy);
                    i++;
                    tmpTotals = DeleteEmpty(fileLines[i].Trim().Split(' '));
                    double.TryParse(tmpTotals[3].Replace('.', ','), out maintenanceSpace);
                    double.TryParse(tmpTotals[7].Replace('.', ','), out energyBalance);
                    i++;
                    tmpTotals = DeleteEmpty(fileLines[i].Trim().Split(' '));
                    double.TryParse(tmpTotals[3].Replace('.', ','), out reserveCapacity);
                    double.TryParse(tmpTotals[7].Replace('.', ','), out lossOfLoadProbability);
                    i++;
                    tmpTotals = DeleteEmpty(fileLines[i].Trim().Split(' '));
                    double.TryParse(tmpTotals[3].Replace('.', ','), out energyPumped);

                    currentScenario.CapacityTotal = totalCapacity;
                    currentScenario.GenerationTotal = totalGeneration;
                    currentScenario.LoadPeak = peakLoad;
                    currentScenario.EnergyDemand = energyDemand;
                    currentScenario.LoadMinimum = minimumLoad;
                    currentScenario.EnergyUnserved = unservedEnergy;
                    currentScenario.MaintenanceSpace = maintenanceSpace;
                    currentScenario.EnergyBalance = energyBalance;
                    currentScenario.CapacityReserve = reserveCapacity;
                    currentScenario.LossOfLoadProbability = lossOfLoadProbability;
                    currentScenario.EnergyPumped = energyPumped;
                    i++;
                    scenarios.Add(currentScenario);
                }
            }
            return scenarios;
        }
        
        public static string[] DeleteEmpty(string[] tmp)
        {
            List<string> result = new List<string>();
            for (int i = 0; i < tmp.Length; i++)
            {
                if (tmp[i] != "")
                {
                    result.Add(tmp[i]);
                }
            }
            return result.ToArray();
        }
    }
}
