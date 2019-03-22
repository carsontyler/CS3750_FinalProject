using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS3750_FinalProject
{
    public class InversionCalculator
    {
        private List<Employee> _ah = new List<Employee>();
        private List<Employee> _be = new List<Employee>();
        private List<Employee> _east = new List<Employee>();
        private List<Employee> _educ = new List<Employee>();
        private List<Employee> _hp = new List<Employee>();
        private List<Employee> _lib = new List<Employee>();
        private List<Employee> _sbs = new List<Employee>();
        private List<Employee> _sci = new List<Employee>();
        private List<Employee> _all = new List<Employee>();

        public void OrganizeByCollege()
        {
            foreach (var e in Employees.EmployeesList)
            {
                switch (e.College)
                {
                    case "A&H":
                        _ah.Add(e);
                        break;
                    case "B&E":
                        _be.Add(e);
                        break;
                    case "EAST":
                        _east.Add(e);
                        break;
                    case "EDUC":
                        _educ.Add(e);
                        break;
                    case "HP":
                        _hp.Add(e);
                        break;
                    case "LIB":
                        _lib.Add(e);
                        break;
                    case "S&BS":
                        _sbs.Add(e);
                        break;
                    case "SCI":
                        _sci.Add(e);
                        break;
                    default:
                        _all.Add(e);
                        break;
                }
            }
        }
    }
}