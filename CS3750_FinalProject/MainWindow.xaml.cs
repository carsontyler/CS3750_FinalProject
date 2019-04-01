using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
using System.Linq;
using System.Data;
using System.ComponentModel;
using System;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Media;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;

namespace CS3750_FinalProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields

        public List<College> Colleges;
        public DataTable Inversiontable;
        public SeriesCollection SeriesCollectionLineChart { get; set; }
        public SeriesCollection SeriesCollectionBarChart { get; set; }
        public string[] LabelsLineChart { get; set; }
        public string[] LabelsBarChart { get; set; }
        public Func<double, string> YFormatterLineChart { get; set; }
        public Func<double, string> YFormatterBarChart { get; set; }

        #endregion

        #region Constructor

        public MainWindow()
        {
            InitializeComponent();
            Colleges = new List<College>();
            LineChart();
            BarChart();
        }

        private void BarChart()
        {
            SeriesCollectionBarChart = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "2015",
                    Values = new ChartValues<double> { 10, 50, 39, 50 }
                }
            };

            //adding series will update and animate the chart automatically
            SeriesCollectionBarChart.Add(new ColumnSeries
            {
                Title = "2016",
                Values = new ChartValues<double> { 11, 56, 42 }
            });

            //also adding values updates and animates the chart automatically
            SeriesCollectionBarChart[1].Values.Add(48d);

            LabelsBarChart = new[] { "Maria", "Susan", "Charles", "Frida" };
            YFormatterBarChart = value => value.ToString("N");

            DataContext = this;
        }
    

        private void LineChart()
        {
            SeriesCollectionLineChart = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Series 1",
                    Values = new ChartValues<double> { 4, 6, 5, 2 ,4 }
                },
                new LineSeries
                {
                    Title = "Series 2",
                    Values = new ChartValues<double> { 6, 7, 3, 4 ,6 },
                    PointGeometry = null
                },
                new LineSeries
                {
                    Title = "Series 3",
                    Values = new ChartValues<double> { 4,2,7,2,7 },
                    PointGeometry = DefaultGeometries.Square,
                    PointGeometrySize = 15
                }
            };

            LabelsLineChart = new[] { "Jan", "Feb", "Mar", "Apr", "May" };
            YFormatterLineChart = value => value.ToString("C");

            //modifying the series collection will animate and update the chart
            SeriesCollectionLineChart.Add(new LineSeries
            {
                Title = "Series 4",
                Values = new ChartValues<double> { 5, 3, 2, 4 },
                LineSmoothness = 0, //0: straight lines, 1: really smooth lines
                PointGeometry = Geometry.Parse("m 25 70.36218 20 -28 -20 22 -8 -6 z"),
                PointGeometrySize = 50,
                PointForeground = Brushes.Gray
            });

            //modifying any series values will also animate and update the chart
            SeriesCollectionLineChart[3].Values.Add(5d);

            DataContext = this;
        }
    


        #endregion

        #region Methods

        #region Private

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
            }
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

        #endregion

        #endregion

        private void HandleMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            MainScroller.ScrollToVerticalOffset(MainScroller.VerticalOffset - e.Delta);
        }


    }
}


    
