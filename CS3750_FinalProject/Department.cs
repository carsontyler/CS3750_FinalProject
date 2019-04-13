using System.Collections.Generic;
using System.Linq;

namespace CS3750_FinalProject
{
    public class Department : InversionData
    {
        #region Properties 

        public List<InvertedEmployee> InvertedEmployees { get; set; }
        public List<Employee> Employees { get; set; }
        public string DepartmentName { get; set; }

        #endregion

        #region Properties

        public Department(string department)
        {
            DepartmentName = department;
            InvertedEmployees = new List<InvertedEmployee>();
            Employees = new List<Employee>();
        }

        #endregion

        #region Methods

        public void OrderEmployees()
        {
            Employees = Employees.OrderByDescending(x => x.SalaryAmount).ToList();
        }
        #endregion
    }
}