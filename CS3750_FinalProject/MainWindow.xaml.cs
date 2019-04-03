using System;
using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
using System.Linq;
using System.Data;
using System.Windows.Controls;
using System.Windows.Media;
using MahApps.Metro.Controls;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

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
            HomeScreen.Visibility = Visibility.Hidden;
            InversionDataView.Visibility = Visibility.Visible;
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