using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS3750_FinalProject
{
    public class InvertedEmployee : InversionData
    {
        #region Properties

        public Employee Inverted { get; set; }
        public List<Employee> Inverters { get; set; }

        #endregion

        public InvertedEmployee(Employee e, Employee i)
        {
            Inverted = new Employee();
            Inverted.College = e.College;
            Inverted.Department = e.Department;
            Inverted.Name = e.Name;
            Inverted.Rank = e.Rank;
            Inverted.SalaryAmount = e.SalaryAmount;
            Inverters = new List<Employee>();
            Inverters.Add(i);
        }
        public InvertedEmployee(Employee e)
        {
            Inverted = new Employee();
            Inverted.College = e.College;
            Inverted.Department = e.Department;
            Inverted.Name = e.Name;
            Inverted.Rank = e.Rank;
            Inverted.SalaryAmount = e.SalaryAmount;
            Inverters = new List<Employee>();
        }

        public void AddInverter(Employee e)
        {
            Inverters.Add(e);
        }
    }
}
