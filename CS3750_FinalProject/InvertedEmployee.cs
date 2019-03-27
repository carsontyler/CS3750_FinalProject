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

        public string College { get; set; }
        public string Department { get; set; }
        public string Name { get; set; }
        public string Rank { get; set; }
        public int SalaryAmount { get; set; }
        public List<Employee> Inverters { get; set; }

        #endregion

        public InvertedEmployee(Employee e, Employee i)
        {
            College = e.College;
            Department = e.Department;
            Name = e.Name;
            Rank = e.Rank;
            SalaryAmount = e.SalaryAmount;
            Inverters = new List<Employee>();
            Inverters.Add(i);
        }
        public InvertedEmployee(Employee e)
        {
            College = e.College;
            Department = e.Department;
            Name = e.Name;
            Rank = e.Rank;
            SalaryAmount = e.SalaryAmount;
            Inverters = new List<Employee>();
        }

        public void AddInverter(Employee e)
        {
            Inverters.Add(e);
        }
    }
}
