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

        #endregion

        #region Constructor

        public MainWindow()
        {
            InitializeComponent();
            Colleges = new List<College>();
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
                System.Windows.Controls.DataGrid InversionDataView = new System.Windows.Controls.DataGrid();
                InversionDataView.AlternatingRowBackground =testDataGrid.AlternatingRowBackground;
                InversionDataView.AlternationCount = testDataGrid.AlternationCount;
                
                testDataGrid.ItemsSource = Colleges;
                

                //DataContainer.Children.Add(InversionDataView);

                //System.Windows.Controls.DataGrid DepartmentDataView = new System.Windows.Controls.DataGrid();
                //DepartmentDataView.AlternatingRowBackground = testDataGrid.AlternatingRowBackground;
                //DepartmentDataView.AlternationCount = testDataGrid.AlternationCount;
                //DepartmentDataView.ItemsSource = Colleges[0].Departments;

                //DataContainer.Children.Add(DepartmentDataView);
            }
        }

        public void FormatCollegesData(List<College> DataList)
        {
           
        }


        #endregion

        #endregion

        private void ExpandRow(object sender, RoutedEventArgs e)
        {

        }
    }


    
}