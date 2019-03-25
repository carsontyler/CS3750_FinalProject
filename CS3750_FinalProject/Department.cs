using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS3750_FinalProject
{
    public class Department : InversionData
    {
        public List<Employee> InvertedEmployees { get; set; }
        public List<Employee> Employees { get; set; }
        public string DepartmentName { get; set; }

        // TODO
        // Calculate 

        public Department()
        {
            InvertedEmployees = new List<Employee>();
        }

        public Department(string department)
        {
            DepartmentName = department;
            InvertedEmployees = new List<Employee>();
            Employees = new List<Employee>();
        }
    }
}
