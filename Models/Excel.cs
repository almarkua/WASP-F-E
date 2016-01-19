using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Resources;
using WASP_F_E.ViewModels;
using Exc = Microsoft.Office.Interop.Excel;
using System.Drawing;
using Color = System.Drawing.Color;

namespace WASP_F_E.Models
{
    class Excel
    {
        public static void WriteStudyToExcel(Study study,string file)
        {
            bool fileSaved = false;
            Exc.Application application = null;
            Exc.Workbook workbook = null;
            Exc.Worksheet worksheet = null;
            try
            {
                application = new Exc.Application();
                workbook = application.Workbooks.Add();
                worksheet = workbook.Sheets[1];

                application.Cells[1, 1] = study.Name;
                application.Cells[2, 1] = study.Date;
                MakeTopHeader(worksheet,1,2,1);
                int offset = 3;
                for (int i = 0; i < study.Scenarios.Count; i++)
                {
                    application.Cells[offset, 1] = Headers.Year;
                    application.Cells[offset, 2] =  study.Scenarios[i].Year;
                    application.Cells[offset, 3] = Headers.Period;
                    application.Cells[offset, 4] = study.Scenarios[i].Period;
                    application.Cells[offset, 5] = Headers.Hydrocondition;
                    application.Cells[offset, 6] = study.Scenarios[i].Hydrocondition;
                    MakeHeader(application, offset, 1, 1);
                    MakeHeader(application, offset, 3, 3);
                    MakeHeader(application, offset, 5, 5);
                    offset++;
                    application.Cells[offset, 1] = Headers.PlantName;
                    application.Cells[offset, 2] = Headers.CapacityBase;
                    application.Cells[offset, 3] = Headers.CapacityPeak;
                    application.Cells[offset, 4] = Headers.CapacityTotal;
                    application.Cells[offset, 5] = Headers.EnergyBase;
                    application.Cells[offset, 6] = Headers.EnergyPeak;
                    application.Cells[offset, 7] = Headers.EnergyTotal;
                    application.Cells[offset, 8] = Headers.PeakMineng;
                    application.Cells[offset, 9] = Headers.EnergySpilled;
                    application.Cells[offset, 10] = Headers.EnergyShortage;
                    application.Cells[offset, 11] = Headers.OaM;
                    application.Cells[offset, 12] = Headers.CapacityFactor;
                    MakeHeader(application, offset, 1, 12);
                    for (int j = 0; j < study.Scenarios[i].HPlants.Count; j++)
                    {
                        offset++;
                        application.Cells[offset, 1] = study.Scenarios[i].HPlants[j].Name;
                        application.Cells[offset, 2] = study.Scenarios[i].HPlants[j].CapacityBase;
                        application.Cells[offset, 3] = study.Scenarios[i].HPlants[j].CapacityPeak;
                        application.Cells[offset, 4] = study.Scenarios[i].HPlants[j].CapacityTotal;
                        application.Cells[offset, 5] = study.Scenarios[i].HPlants[j].EnergyBase;
                        application.Cells[offset, 6] = study.Scenarios[i].HPlants[j].EnergyPeak;
                        application.Cells[offset, 7] = study.Scenarios[i].HPlants[j].EnergyTotal;
                        application.Cells[offset, 8] = study.Scenarios[i].HPlants[j].PeakMineng;
                        application.Cells[offset, 9] = study.Scenarios[i].HPlants[j].EnergySpilled;
                        application.Cells[offset, 10] = study.Scenarios[i].HPlants[j].EnergyShortage;
                        application.Cells[offset, 11] = study.Scenarios[i].HPlants[j].OaM;
                        application.Cells[offset, 12] = study.Scenarios[i].HPlants[j].CapacityFactor;
                    }

                    offset+=2;
                    application.Cells[offset, 1] = Headers.PlantName;
                    application.Cells[offset, 2] = Headers.NumberOfUnits;
                    application.Cells[offset, 3] = Headers.CapacityBase;
                    application.Cells[offset, 4] = Headers.CapacityPeak;
                    application.Cells[offset, 5] = Headers.CapacityTotal;
                    application.Cells[offset, 6] = Headers.EnergyBase;
                    application.Cells[offset, 7] = Headers.EnergyPeak;
                    application.Cells[offset, 8] = Headers.EnergyTotal;
                    application.Cells[offset, 9] = Headers.FuelDomestic;
                    application.Cells[offset, 10] = Headers.FuelForeign;
                    application.Cells[offset, 11] = Headers.FuelTotal;
                    application.Cells[offset, 12] = Headers.OaM;
                    application.Cells[offset, 13] = Headers.MainProbability;
                    application.Cells[offset, 14] = Headers.ForCell;
                    application.Cells[offset, 15] = Headers.CapacityFactor;
                    MakeHeader(application, offset, 1, 15);
                    foreach (var tPlant in study.Scenarios[i].TPlants)
                    {
                        offset++;
                        application.Cells[offset, 1] = tPlant.Name;
                        application.Cells[offset, 2] = tPlant.NumberOfUnits;
                        application.Cells[offset, 3] = tPlant.CapacityBase;
                        application.Cells[offset, 4] = tPlant.CapacityPeak;
                        application.Cells[offset, 5] = tPlant.CapacityTotal;
                        application.Cells[offset, 6] = tPlant.EnergyBase;
                        application.Cells[offset, 7] = tPlant.EnergyPeak;
                        application.Cells[offset, 8] = tPlant.EnergyTotal;
                        application.Cells[offset, 9] = tPlant.FuelDomestic;
                        application.Cells[offset, 10] = tPlant.FuelForeign;
                        application.Cells[offset, 11] = tPlant.FuelTotal;
                        application.Cells[offset, 12] = tPlant.OaM;
                        application.Cells[offset, 13] = tPlant.MainProbability;
                        application.Cells[offset, 14] = tPlant.ForCell;
                        application.Cells[offset, 15] = tPlant.CapacityFactor;
                    }
                    offset++;
                }
                workbook.SaveAs(file);
                fileSaved = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при роботі із Excel: "+ex.Message,"Помилка!",MessageBoxButton.OK,MessageBoxImage.Error);
            }
            finally
            {
                workbook.Close();
                application.Quit();
                workbook = null;
                worksheet = null;
                application = null;
                GC.Collect();
                if (fileSaved) Process.Start(file);
            }
        }

        public static void WriteDataGridToExcel(List<TypesAdditionalVm> data , string file)
        {
            bool fileSaved = false;
            Exc.Application application = null;
            Exc.Workbook workbook = null;
            Exc.Worksheet worksheet = null;
            try
            {
                application = new Exc.Application();
                workbook = application.Workbooks.Add();
                worksheet = workbook.Sheets[1];
                var myFieldInfo =
                    (typeof (TypesAdditionalVm)).GetFields(BindingFlags.NonPublic | BindingFlags.Instance |
                                                           BindingFlags.Public);
                int offset = 1;
                for (int i = 0; i < myFieldInfo.Count(); i++)
                {
                    worksheet.Cells[offset, i + 1] = myFieldInfo[i].Name.Split('>')[0].Substring(1);
                }
                MakeHeader(application, offset, 1, myFieldInfo.Count());
                
                foreach (TypesAdditionalVm item in data)
                {
                    offset++;
                    worksheet.Cells[offset, 1] = item.StudyName;
                    worksheet.Cells[offset, 2] = item.Year;
                    worksheet.Cells[offset, 3] = item.PlantType;
                    worksheet.Cells[offset, 4] = item.Value;
                }
                workbook.SaveAs(file);
                fileSaved = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Excel error: " + ex.Message);
            }
            finally
            {
                workbook.Close();
                application.Quit();
                workbook = null;
                worksheet = null;
                application = null;
                GC.Collect();
                if (fileSaved) Process.Start(file);
            }
        }

        /*
            Виділення заголовків таблиць
        */
        private static void MakeHeader(Exc.Application application, int row, int cellStart, int cellEnd)
        {
            application.Range[GetCell(cellStart) + row, GetCell(cellEnd) + row].HorizontalAlignment = Exc.XlHAlign.xlHAlignCenter;
            application.Range[GetCell(cellStart) + row, GetCell(cellEnd) + row].Font.Bold = true;
            application.Range[GetCell(cellStart) + row, GetCell(cellEnd) + row].Interior.Color = ColorTranslator.ToOle(Color.LightGray);
            application.Range[GetCell(cellStart) + row, GetCell(cellEnd) + row].Borders.Weight = 2;
            application.Range[GetCell(cellStart) + row, GetCell(cellEnd) + row].Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            for (int i = cellStart; i <= cellEnd; i++)
            {
                var l = (double)application.Cells[row, i].ToString().Length * 1.4;
                application.Range[GetCell(i) + row, GetCell(i) + row].ColumnWidth =
                    (double)(application.Cells[row, i].Value2.ToString().Length * 1.4);
            }
        }


        public static void WriteEmissionsToExcel(Study study, Dictionary<EmissionType,Dictionary<int,double>> dict, string file)
        {
            bool fileSaved = false;
            Exc.Application application = null;
            Exc.Workbook workbook = null;
            Exc.Worksheet worksheet = null;
            try
            {
                application = new Exc.Application();
                workbook = application.Workbooks.Add();
                worksheet = workbook.Sheets[1];
                worksheet.Cells[1, 1] = study.Name;
                worksheet.Cells[2, 1] = study.Date.ToShortDateString();
                MakeTopHeader(worksheet,1,3,1);
                
                int offset = 4;
                worksheet.Cells[offset, 1] = Headers.Emission;
                worksheet.Cells[offset, 2] = Headers.Year;
                worksheet.Cells[offset, 3] = Headers.ValueKG;
                MakeHeader(application, offset, 1, 3);
                foreach (var eType in dict.Keys)
                {
                    foreach (var year in dict[eType].Keys)
                    {
                        offset++;
                        worksheet.Cells[offset, 1] = eType.Name;
                        worksheet.Cells[offset, 2] = year;
                        worksheet.Cells[offset, 3] = dict[eType][year];
                    }
                    offset++;
                }

                workbook.SaveAs(file);
                fileSaved = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Excel error: " + ex.Message);
            }
            finally
            {
                workbook.Close();
                application.Quit();
                workbook = null;
                worksheet = null;
                application = null;
                GC.Collect();
                if (fileSaved) Process.Start(file);
            }
        }

        //Виділення звичайних загаловків
        private static void MakeTopHeader(Exc.Worksheet worksheet, int cellStart, int cellEnd, int row)
        {
            worksheet.get_Range(GetCell(cellStart)+row,GetCell(cellEnd)+row).Merge(Type.Missing);
            worksheet.get_Range(GetCell(cellStart) + row, GetCell(cellEnd) + row).Font.Bold = true;
            worksheet.get_Range(GetCell(cellStart) + row, GetCell(cellEnd) + row).Font.Size = 16;
            worksheet.get_Range(GetCell(cellStart) + row, GetCell(cellEnd) + row).HorizontalAlignment = Exc.XlHAlign.xlHAlignCenter;
            row++;
            worksheet.get_Range(GetCell(cellStart) + row, GetCell(cellEnd) + row).Merge(Type.Missing);
            worksheet.get_Range(GetCell(cellStart) + row, GetCell(cellEnd) + row).Font.Bold = true;
            worksheet.get_Range(GetCell(cellStart) + row, GetCell(cellEnd) + row).HorizontalAlignment = Exc.XlHAlign.xlHAlignCenter;
        }

        private static string GetCell(int number)
        {
            string result = "";
            int count = 0;
            while (number > 26)
            {
                number -= 26;
                count++;
            }
            if (count > 0) result += GetCell(count);
            if (number > 0)
            {
                result += (char)(64 + number);
            }
            return result;
        }
    }
}