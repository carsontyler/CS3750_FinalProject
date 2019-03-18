using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS3750_FinalProject
{
    public class Employee
    {
        public string Id { get; set; }
        public string College { get; set; }
        public string Department { get; set; }
        public string Name { get; set; }
        public string Rank { get; set; }
        public int SalaryAmount { get; set; }
    }
}
