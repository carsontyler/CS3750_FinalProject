using System.Collections.Generic;
using System.Windows;

namespace CS3750_FinalProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields

        #endregion

        #region Constructor

        public MainWindow()
        {
            InitializeComponent();
            Employees.EmployeesList = new List<Employee>();
        }

        #endregion

        #region Methods

        #endregion
    }
}