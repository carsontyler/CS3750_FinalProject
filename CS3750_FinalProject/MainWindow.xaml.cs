using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;

namespace CS3750_FinalProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var temp = new OpenFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv"
            };
            if (temp.ShowDialog() == System.Windows.Forms.DialogResult.OK) { }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var filedialog = new OpenFileDialog();
            if(filedialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    string file = filedialog.FileName;

                }
                catch (Exception Ex)
                {
                    Console.Write(Ex);
                }
            }
        }
    }
}