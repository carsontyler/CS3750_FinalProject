using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CS3750_FinalProject
{
    /// <summary>
    /// Interaction logic for MenuBar.xaml
    /// </summary>
    public partial class MenuBar : System.Windows.Controls.UserControl
    {
        #region Fields

        #endregion

        #region Constructor

        public MenuBar()
        {
            InitializeComponent();
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

                string[] fields;
                List<string> headers = new List<string>();

                while (!parser.EndOfData)
                {
                    fields = parser.ReadFields();
                    if (fields[0] == "")
                        continue;

                    if (fields[0] == "CLG" || fields[0] == "ID")
                    {
                        foreach (var field in fields)
                            headers.Add(field);
                        continue;
                    }

                    Employees.EmployeesList.Add(new Employee
                    {
                        College = fields[headers.IndexOf("CLG")],
                        Department = fields[headers.IndexOf("DEPT.")],
                        Name = fields[headers.IndexOf("NAME")],
                        Rank = fields[headers.IndexOf("RNK")],
                        SalaryAmount = int.Parse(fields[headers.IndexOf("9MSALARY")])
                    });
                }
            }
        }

        #endregion

        #region Public

        #endregion

        #endregion
    }
}