using System.Collections.Generic;

namespace CS3750_FinalProject
{
    public class InvertedEmployee : InversionData
    {
        #region Properties

        public Employee Inverted { get; set; }
        public List<Employee> Inverters { get; set; }

        #endregion

        #region Constructors

        public InvertedEmployee(Employee e, Employee i)
        {
            Inverted = new Employee
            {
                College = e.College,
                Department = e.Department,
                Name = e.Name,
                Rank = e.Rank,
                SalaryAmount = e.SalaryAmount
            };
            Inverters = new List<Employee> { i };
        }

        public InvertedEmployee(Employee e)
        {
            Inverted = new Employee
            {
                College = e.College,
                Department = e.Department,
                Name = e.Name,
                Rank = e.Rank,
                SalaryAmount = e.SalaryAmount
            };
            Inverters = new List<Employee>();
        }

        #endregion

        #region Public Methods

        public void AddInverter(Employee e)
        {
            Inverters.Add(e);
        }

        #endregion
    }
}