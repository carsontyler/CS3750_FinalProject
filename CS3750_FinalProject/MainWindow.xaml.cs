using Microsoft.VisualBasic.FileIO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Forms;
using System.Windows.Media;

namespace CS3750_FinalProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        #region Fields

        public List<College> Colleges;
        public bool InvalidCsvFile = true;

        public int TotalInversion { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Colleges = new List<College>();
        }

        /// <summary>
        /// Constructor which gets called by the SplashScreen.
        /// </summary>
        /// <param name="fileName">The CSV to be opened.</param>
        public MainWindow(string fileName = "")
        {
            InitializeComponent();
            Colleges = new List<College>();
            if (!string.IsNullOrEmpty(fileName))
                ParseFile(fileName);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Opens a file when the 'Open' button is pressed in the menu bar.
        /// A file dialog is opened for the user to choose a CSV file. 
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
                if (DataGridCollege.HasItems)
                {
                    var result = System.Windows.MessageBox.Show("Would you like to overwrite the data?", "Overwrite Data", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.No)
                        return;
                    ClearData();
                }
                string file = filedialog.FileName;
                ParseFile(file);
            }
        }

        /// <summary>
        /// Parse through the data to be displayed. Sorts the data into various groupings and adds it to the data grid on the DETAILS page
        /// Also creates the associated graphs on the CHARTS page
        /// </summary>
        /// <param name="file">The CSV file to be parsed through</param>
        private void ParseFile(string file)
        {
            BrushConverter brush = new BrushConverter();
            ShowDataGridExecute();
            TextFieldParser parser = new TextFieldParser(file) { HasFieldsEnclosedInQuotes = true };
            parser.SetDelimiters(",");

            List<string> headers = new List<string>();

            // Begins the looping of the data, separates into Colleges and Deparmtnets. 
            try
            {
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

                    if (!Colleges.FirstOrDefault(a => a.CollegeName == fields[headers.IndexOf("CLG")]).Departments.Any(a => a.DepartmentName == fields[headers.IndexOf("DEPT.")]))
                        Colleges.FirstOrDefault(a => a.CollegeName == fields[headers.IndexOf("CLG")]).Departments.Add(new Department(fields[headers.IndexOf("DEPT.")]));

                    Colleges.FirstOrDefault(a => a.CollegeName == fields[headers.IndexOf("CLG")]).
                        Departments.First(a => a.DepartmentName == fields[headers.IndexOf("DEPT.")]).Employees.Add(employee);

                    Colleges.FirstOrDefault(a => a.CollegeName == fields[headers.IndexOf("CLG")]).
                        Departments.First(a => a.DepartmentName == fields[headers.IndexOf("DEPT.")]).OrderEmployees();
                }
            }
            catch (Exception)
            {
                var popup = System.Windows.MessageBox.Show("Invalid CSV File! Please try again.", "Invalid File");
                return;
            }
            InvalidCsvFile = false;
            InversionCalculator.CalcInversion(Colleges);
            
            DataGridCollege.ItemsSource = Colleges;
            
            LoadLineChartATFData();
            LoadLineChartNOIData();
            LoadPieChartData();
            //HomeScreen.Visibility = Visibility.Hidden;
            //InversionDataView.Visibility = Visibility.Visible;
            //SummaryView.Visibility = Visibility.Collapsed;
        }

        private void LoadLineChartNOIData()
        {
            List<KeyValuePair<string, int>> NumberOfInversions = new List<KeyValuePair<string, int>>();

            for (int i = 0; i < Colleges.Count(); i++)
            {
                var keyToAdd = Colleges[i].CollegeName;

                TotalInversion += Colleges[i].AssistantLessThanInstructor;
                TotalInversion += Colleges[i].AssociateLessThanAssistant;
                TotalInversion += Colleges[i].AssociateLessThanInstructor;
                TotalInversion += Colleges[i].FullLessThanAssistant;
                TotalInversion += Colleges[i].FullLessThanAssociate;
                TotalInversion += Colleges[i].FullLessThanInstructor;

                var valueToAdd = TotalInversion;
                NumberOfInversions.Add(new KeyValuePair<string, int>(keyToAdd, valueToAdd));
                TotalInversion = 0;
            }

          ((ColumnSeries)lineChartNOI.Series[0]).ItemsSource = NumberOfInversions;

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

            ((ColumnSeries)lineChartATF.Series[0]).ItemsSource = AmountToFixList;

        }

        private List<KeyValuePair<string, int>> LoadPieChartData()
        {
            List<KeyValuePair<string, int>> kvpList = new List<KeyValuePair<string, int>>();

            if (Colleges.Any())
            {
                var key = Colleges[0].Departments[0].DepartmentName;
                var value = Colleges[0].Departments[0].TotalAmountToFix;

                for (int i = 0; i < Colleges.Count(); i++)
                {
                    for (int j = 0; j < Colleges[i].Departments.Count(); j++)
                    {
                        var keyToAdd = Colleges[i].Departments[j].DepartmentName;
                        var valueToAdd = Colleges[i].Departments[j].TotalAmountToFix;
                        kvpList.Add(new KeyValuePair<string, int>(keyToAdd, valueToAdd));
                    }
                }
            }
            kvpList = kvpList.OrderByDescending(a => a.Value).ToList();
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

            dlg.ShowDialog();

            IWorkbook workbook = new XSSFWorkbook();

            /*COLLEGE SHEET*/
            ISheet sheet1 = workbook.CreateSheet("InversionsByCollege");

            IRow row0 = sheet1.CreateRow(0);
            row0.CreateCell(0).SetCellValue("College");
            row0.CreateCell(1).SetCellValue("Total Amount To Fix");
            row0.CreateCell(2).SetCellValue("Assistant < Instructor");
            row0.CreateCell(3).SetCellValue("Associate < Instructor");
            row0.CreateCell(4).SetCellValue("Associate < Assistant");
            row0.CreateCell(5).SetCellValue("Full < Instructor");
            row0.CreateCell(6).SetCellValue("Full < Assistant");
            row0.CreateCell(7).SetCellValue("Full < Associate");

            var x = 1;
            foreach (var college in Colleges) //loop colleges
            {
                var row = sheet1.CreateRow(x++);
                row.CreateCell(0).SetCellValue(college.CollegeName);
                row.CreateCell(1).SetCellValue(college.TotalAmountToFix);
                row.CreateCell(2).SetCellValue(college.AssistantLessThanInstructorToFix);
                row.CreateCell(3).SetCellValue(college.AssociateLessThanInstructorToFix);
                row.CreateCell(4).SetCellValue(college.AssociateLessThanAssistantToFix);
                row.CreateCell(5).SetCellValue(college.FullLessThanInstructorToFix);
                row.CreateCell(6).SetCellValue(college.FullLessThanAssistantToFix);
                row.CreateCell(7).SetCellValue(college.FullLessThanAssociateToFix);
            }
            /*END COLLEGE SHEET, START DEPT SHEET*/
            ISheet sheet2 = workbook.CreateSheet("InversionsByDepartment"); //setup sheet 2

            IRow row1 = sheet2.CreateRow(0);
            row1.CreateCell(0).SetCellValue("College");
            row1.CreateCell(1).SetCellValue("Department");
            row1.CreateCell(2).SetCellValue("Total Amount To Fix");
            row1.CreateCell(3).SetCellValue("Assistant < Instructor");
            row1.CreateCell(4).SetCellValue("Associate < Instructor");
            row1.CreateCell(5).SetCellValue("Associate < Assistant");
            row1.CreateCell(6).SetCellValue("Full < Instructor");
            row1.CreateCell(7).SetCellValue("Full < Assistant");
            row1.CreateCell(8).SetCellValue("Full < Associate");

            var y = 1;
            foreach (var college in Colleges) //loop colleges
            {
                foreach (var dept in college.Departments) //loop departments
                {
                    var row = sheet2.CreateRow(y++);
                    row.CreateCell(0).SetCellValue(college.CollegeName);
                    row.CreateCell(1).SetCellValue(dept.DepartmentName);
                    row.CreateCell(2).SetCellValue(dept.TotalAmountToFix);
                    row.CreateCell(3).SetCellValue(dept.AssistantLessThanInstructorToFix);
                    row.CreateCell(4).SetCellValue(dept.AssociateLessThanInstructorToFix);
                    row.CreateCell(5).SetCellValue(dept.AssociateLessThanAssistantToFix);
                    row.CreateCell(6).SetCellValue(dept.FullLessThanInstructorToFix);
                    row.CreateCell(7).SetCellValue(dept.FullLessThanAssistantToFix);
                    row.CreateCell(8).SetCellValue(dept.FullLessThanAssociateToFix);
                }
            }
            /*END DEPT SHEET, START EMPLOYEE SHEET*/
            ISheet sheet3 = workbook.CreateSheet("InversionsByProfessor"); //setup sheet 3

            IRow row2 = sheet3.CreateRow(0);
            row2.CreateCell(0).SetCellValue("College");
            row2.CreateCell(1).SetCellValue("Department");
            row2.CreateCell(2).SetCellValue("Name");
            row2.CreateCell(3).SetCellValue("Salary");
            row2.CreateCell(4).SetCellValue("Number of Inversions");
            row2.CreateCell(5).SetCellValue("Amount to Fix Inversion");

            var final = 0;
            var z = 1;
            foreach (var college in Colleges) //loop colleges
            {
                foreach (var dept in college.Departments) //loop departments
                {
                    foreach (var emp in dept.InvertedEmployees) //loop inverted employees
                    {
                        var row = sheet3.CreateRow(z++);
                        row.CreateCell(0).SetCellValue(college.CollegeName);
                        row.CreateCell(1).SetCellValue(dept.DepartmentName);
                        row.CreateCell(2).SetCellValue(emp.Inverted.Name);
                        row.CreateCell(3).SetCellValue(emp.Inverted.SalaryAmount);
                        row.CreateCell(4).SetCellValue(emp.Inverters.Count);
                        row.CreateCell(5).SetCellValue(emp.TotalAmountToFix);
                        final += emp.TotalAmountToFix;
                    }
                }
            }
            /*END EMP SHEET, START OVERVIEW SHEET*/
            ISheet sheet4 = workbook.CreateSheet("Overview"); //setup sheet 4

            IRow row3 = sheet4.CreateRow(0);
            row3.CreateCell(0).SetCellValue("Total Number of Inversions");
            row3.CreateCell(1).SetCellValue("Total Amount to Fix Inversions");
            IRow row4 = sheet4.CreateRow(1);
            row4.CreateCell(0).SetCellValue(z);
            row4.CreateCell(1).SetCellValue(final);
            /*END OVERVIEW SHEET*/

            FileStream sw = File.Create(dlg.FileName);

            workbook.Write(sw);

            sw.Close();
        }

        /// <summary>
        /// Executes when the 'DETAILS' button is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowDataGrid(object sender, RoutedEventArgs e)
        {
            ShowDataGridExecute();
        }

        /// <summary>
        /// Sets ths display settings to show the 'DETAILS' tab and associated data grid
        /// </summary>
        private void ShowDataGridExecute()
        {
            Brush ogColor = DataGridButton.Background;
            SummaryButton.Background = ogColor;
            BrushConverter brush = new BrushConverter();
            DataGridButton.Background = (Brush)brush.ConvertFrom("#837AE5");
            DataGridButton.Foreground = new SolidColorBrush(Colors.White);
            SummaryButton.Foreground = new SolidColorBrush(Colors.Black);
            //HomeScreen.Visibility = Visibility.Hidden;
            InversionDataView.Visibility = Visibility.Visible;
            SummaryView.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Executes when the 'SUMMARY' button is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowSummary(object sender, RoutedEventArgs e)
        {
            ShowSummaryExecute();
        }

        /// <summary>
        /// Sets the display settings to show the 'SUMMARY' tab and associated data grid
        /// </summary>
        private void ShowSummaryExecute()
        {
            Brush ogColor = SummaryButton.Background;
            DataGridButton.Background = ogColor;
            BrushConverter brush = new BrushConverter();
            SummaryButton.Background = (Brush)brush.ConvertFrom("#837AE5");
            DataGridButton.Foreground = new SolidColorBrush(Colors.Black);
            SummaryButton.Foreground = new SolidColorBrush(Colors.White);
            //HomeScreen.Visibility = Visibility.Hidden;
            SummaryView.Visibility = Visibility.Visible;
            InversionDataView.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Executes when the 'Quit' button is pressed in the menu. 
        /// Displays a popup box to confirm they want to quit. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Quit(object sender, RoutedEventArgs e)
        {
            //var result = System.Windows.MessageBox.Show("Are you sure you want to quit? Your data will not be saved.", "Quit", MessageBoxButton.YesNo);
            //if (result == MessageBoxResult.No)
            //    return;
            
            //MetroWindow_Closing is called on "close()" method  above code causes the popup box to happen twice
            Close();
        }

        /// <summary>
        /// Executes when the X button (on the window itself) is pressed.
        /// Displays a popup box to confirm they want to quit. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var result = System.Windows.MessageBox.Show("Are you sure you want to quit? Your data will not be saved.", "Quit", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No) e.Cancel = true;
        }

        /// <summary>
        /// Executes when the 'Clear Data' button is pressed in the menu.
        /// Displays a popup box to confirm they want to clear the data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearData_Click(object sender, RoutedEventArgs e)
        {
            var result = System.Windows.MessageBox.Show("Would you like to clear the data?", "Clear Data", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No)
                return;
            ClearData();
        }

        /// <summary>
        /// Clears the data from the data grid and graphs.
        /// </summary>
        private void ClearData()
        {
            Colleges = new List<College>();
            DataGridCollege.ItemsSource = null; 
         
            LoadLineChartATFData();
            LoadLineChartNOIData();
            LoadPieChartData();
        }

        #endregion
    }
}