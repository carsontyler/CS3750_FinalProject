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
using System.Windows.Shapes;

namespace CS3750_FinalProject
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window
    {
        #region Constructors 

        /// <summary>
        /// Default constructor for the Splash Screen
        /// </summary>
        public SplashScreen()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Executes on 'Exit' button click. Exits the application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Executes on 'Open File' button. 
        /// Opens the file and checks to see if it was a valid CSV. If not, it stays on the current screen. 
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
                var mainWindow = new MainWindow(file);
                if (mainWindow.InvalidCsvFile)
                    return;
                mainWindow.Show();
                this.Close();
            }
        }

        #endregion
    }
}
