using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS3750_FinalProject
{
    public class Department : InversionData
    {
        public List<InvertedEmployee> InvertedEmployees { get; set; }
        public List<Employee> Employees { get; set; }
        public string DepartmentName { get; set; }

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
