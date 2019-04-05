﻿using System;
using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
using System.Linq;
using System.Data;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using MahApps.Metro.Controls;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Windows.Controls.DataVisualization.Charting;

namespace CS3750_FinalProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        #region Fields

        public List<College> Colleges;
        public DataTable Inversiontable;

        public int totalInversion { get; private set; }

        #endregion

        #region Constructor

        public MainWindow(string fileName = "")
        {
            InitializeComponent();
            Colleges = new List<College>();
            if (!string.IsNullOrEmpty(fileName))
                ParseFile(fileName);
        }
        public MainWindow()
        {
            InitializeComponent();
            Colleges = new List<College>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Opens a file dialog for the user to choose a CSV file. 
        /// Upon opening the CSV file, and assuming it's in the correct format, an Employee object will be created
        ///     and added to the global _employees list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Open_File(object sender, RoutedEventArgs e)
        {
            var filedialog = new OpenFileDialog
            {
                // Only allows .csv files 
                Filter = "CSV (*.csv)|*.csv"
            };

            if (filedialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string file = filedialog.FileName;
                ParseFile(file);
            }
        }

        private void ParseFile(string file)
        {
            BrushConverter brush = new BrushConverter();
            DataGridButton.Background = (Brush)brush.ConvertFrom("#bb33ff");
            DataGridButton.Foreground = new SolidColorBrush(Colors.White);
            TextFieldParser parser = new TextFieldParser(file) { HasFieldsEnclosedInQuotes = true };
            parser.SetDelimiters(",");

            List<string> headers = new List<string>();

            while (!parser.EndOfData)
            {
                var fields = parser.ReadFields();
                if (fields[0] == "")
                    continue;

                if (fields[0] == "CLG" || fields[0] == "ID")
                {
                    foreach (var field in fields)
                        headers.Add(field);
                    continue;
                }

                var employee = new Employee
                {
                    College = fields[headers.IndexOf("CLG")],
                    Department = fields[headers.IndexOf("DEPT.")],
                    Name = fields[headers.IndexOf("NAME")],
                    Rank = fields[headers.IndexOf("RNK")],
                    SalaryAmount = int.Parse(fields[headers.IndexOf("9MSALARY")])
                };

                if (!Colleges.Any(a => a.CollegeName == fields[headers.IndexOf("CLG")]))
                    Colleges.Add(new College(fields[headers.IndexOf("CLG")]));

                if (!Colleges.FirstOrDefault(a => a.CollegeName == fields[headers.IndexOf("CLG")]).
                        Departments.Any(a => a.DepartmentName == fields[headers.IndexOf("DEPT.")]))
                    Colleges.FirstOrDefault(a => a.CollegeName == fields[headers.IndexOf("CLG")]).
                        Departments.Add(new Department(fields[headers.IndexOf("DEPT.")]));

                Colleges.FirstOrDefault(a => a.CollegeName == fields[headers.IndexOf("CLG")]).
                    Departments.First(a => a.DepartmentName == fields[headers.IndexOf("DEPT.")]).Employees.Add(employee);
            }

            InversionCalculator.CalcInversion(Colleges);

            DataGridCollege.ItemsSource = Colleges;
            LoadLineChartATFData();
            LoadLineChartNOIData();
            LoadPieChartData();
            HomeScreen.Visibility = Visibility.Hidden;
            InversionDataView.Visibility = Visibility.Visible;
        }
        
         private void LoadLineChartNOIData()
        {
            List<KeyValuePair<string, int>> NumberOfInversions = new List<KeyValuePair<string, int>>();

            for (int i = 0; i < Colleges.Count(); i++)
            {
                var keyToAdd = Colleges[i].CollegeName;

                totalInversion += Colleges[i].AssistantLessThanInstructor;
                totalInversion += Colleges[i].AssociateLessThanAssistant;
                totalInversion += Colleges[i].AssociateLessThanInstructor;
                totalInversion += Colleges[i].FullLessThanAssistant;
                totalInversion += Colleges[i].FullLessThanAssociate;
                totalInversion += Colleges[i].FullLessThanInstructor;

                var valueToAdd = totalInversion;
                NumberOfInversions.Add(new KeyValuePair<string, int>(keyToAdd, valueToAdd));
                totalInversion = 0;
            }

          ((LineSeries)lineChartNOI.Series[0]).ItemsSource = NumberOfInversions;

        }

        private void LoadLineChartATFData()
        {
            List<KeyValuePair<string, int>> AmountToFixList = new List<KeyValuePair<string, int>>();


            for (int i = 0; i < Colleges.Count(); i++)
            {
                var keyToAdd = Colleges[i].CollegeName;
                var valueToAdd = Colleges[i].TotalAmountToFix;
                AmountToFixList.Add(new KeyValuePair<string, int>(keyToAdd, valueToAdd));
            }

            ((LineSeries)lineChartATF.Series[0]).ItemsSource = AmountToFixList;

        }

        private List<KeyValuePair<string, int>> LoadPieChartData()
        {
            var key = Colleges[0].Departments[0].DepartmentName;
            var value = Colleges[0].Departments[0].TotalAmountToFix;

            List<KeyValuePair<string, int>> kvpList = new List<KeyValuePair<string, int>>();


            for (int i = 0; i < Colleges.Count(); i++)
            {
                for (int j = 0; j < Colleges[i].Departments.Count(); j++)
                {
                    var keyToAdd = Colleges[i].Departments[j].DepartmentName;
                    var valueToAdd = Colleges[i].Departments[j].TotalAmountToFix;
                    kvpList.Add(new KeyValuePair<string, int>(keyToAdd, valueToAdd));
                }
            }
            ((PieSeries)pieChart.Series[0]).ItemsSource = kvpList;
            return kvpList;

        }


        private void ExpandRow(object sender, RoutedEventArgs e)
        {
            DependencyObject obj = (DependencyObject)e.OriginalSource;
            while (!(obj is DataGridRow) && obj != null) obj = VisualTreeHelper.GetParent(obj);

            if (obj is DataGridRow)
            {
                if ((obj as DataGridRow).DetailsVisibility == Visibility.Visible)
                {
                    (obj as DataGridRow).DetailsVisibility = Visibility.Collapsed;
                    System.Windows.Controls.Button btn = new System.Windows.Controls.Button();
                    btn = sender as System.Windows.Controls.Button;
                    btn.Content = "+";
                }
                else
                {
                    (obj as DataGridRow).DetailsVisibility = Visibility.Visible;
                    System.Windows.Controls.Button btn = new System.Windows.Controls.Button();
                    btn = sender as System.Windows.Controls.Button;
                    btn.Content = "-";
                }
            }
        }

        private void ListViewScrollViewer_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }
        private void HandleMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            MainScroller.ScrollToVerticalOffset(MainScroller.VerticalOffset - e.Delta);
        }

        private void Export(object sender, RoutedEventArgs e)
        {

            var dlg = new SaveFileDialog
            {
                FileName = "InversionExport", // Default file name
                DefaultExt = ".xlsx", // Default file extension
                Filter = "Spreadsheet (.xlsx)|*.xlsx" // Filter files by extension
            };

            //dlg.ShowDialog();

            IWorkbook workbook = new XSSFWorkbook();

            ISheet sheet1 = workbook.CreateSheet("InversionsByCollege");

            /*sheet1.CreateRow(0).CreateCell(0).SetCellValue("Colleges");
            sheet1.CreateRow(0).CreateCell(1).SetCellValue("TotalAmtToFix");
            sheet1.CreateRow(0).CreateCell(2).SetCellValue("Asst<Instr");
            sheet1.CreateRow(0).CreateCell(3).SetCellValue("Assoc<Instr");
            sheet1.CreateRow(0).CreateCell(0).SetCellValue("Colleges");*/


            var x = 1;

            foreach (var college in Colleges)
            {
                var row = sheet1.CreateRow(x++);

                row.CreateCell(0).SetCellValue(college.CollegeName);
                row.CreateCell(1).SetCellValue(college.TotalAmountToFix);
                row.CreateCell(2).SetCellValue(college.AssistantLessThanInstructor);
                row.CreateCell(3).SetCellValue(college.AssociateLessThanInstructor);
                row.CreateCell(4).SetCellValue(college.AssociateLessThanAssistant);
                row.CreateCell(5).SetCellValue(college.FullLessThanInstructor);
                row.CreateCell(6).SetCellValue(college.FullLessThanAssistant);
                row.CreateCell(7).SetCellValue(college.FullLessThanAssociate);
            }

            /*for (int i = 0; i <= Colleges.Count; i++)
            {
                IRow row = sheet1.CreateRow(i);

                row.CreateCell(i).SetCellValue(x++);

                for (int j = 0; j < 15; j++)
                {
                    row.CreateCell(j).SetCellValue(x++);
                }
            }*/

            FileStream sw = File.Create("test.xls");

            workbook.Write(sw);

            sw.Close();
        }

        private void ShowDataGrid(object sender, RoutedEventArgs e)
        {
            Brush ogColor = DataGridButton.Background;
            SummaryButton.Background = ogColor;
            BrushConverter brush = new BrushConverter();
            DataGridButton.Background = (Brush)brush.ConvertFrom("#bb33ff");
            DataGridButton.Foreground = new SolidColorBrush(Colors.White);
            SummaryButton.Foreground = new SolidColorBrush(Colors.Black);
            HomeScreen.Visibility = Visibility.Hidden;
            InversionDataView.Visibility = Visibility.Visible;
            SummaryView.Visibility = Visibility.Collapsed;
        }

        private void ShowSummary(object sender, RoutedEventArgs e)
        {
            Brush ogColor = SummaryButton.Background;
            DataGridButton.Background = ogColor;
            BrushConverter brush = new BrushConverter();
            SummaryButton.Background = (Brush)brush.ConvertFrom("#bb33ff");
            DataGridButton.Foreground = new SolidColorBrush(Colors.Black);
            SummaryButton.Foreground = new SolidColorBrush(Colors.White);
            HomeScreen.Visibility = Visibility.Hidden;
            SummaryView.Visibility = Visibility.Visible;
            InversionDataView.Visibility = Visibility.Collapsed;
        }

        private void Quit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        #endregion

    }

}