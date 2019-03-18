using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;

namespace CS3750_FinalProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Employee> _employees = new List<Employee>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var filedialog = new OpenFileDialog();
            if (filedialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string file = filedialog.FileName;
                TextFieldParser parser = new TextFieldParser(file) { HasFieldsEnclosedInQuotes = true };
                parser.SetDelimiters(",");

                string[] fields;

                while (!parser.EndOfData)
                {
                    fields = parser.ReadFields();
                    if (fields[0] == "" || fields[0] == "ID")
                        continue;

                    _employees.Add(new Employee
                    {
                        Id = fields[0],
                        College = fields[1],
                        Department = fields[2],
                        Name = fields[3],
                        Rank = fields[9],
                        SalaryAmount = int.Parse(fields[18].Replace(",", ""))
                    });
                }
            }
        }
    }
}