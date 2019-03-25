using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS3750_FinalProject
{
    public class College : InversionData
    {
        public List<Department> Departments { get; set; }
        public string CollegeName { get; set; }

        public College(string collegeName)
        {
            CollegeName = collegeName;
            Departments = new List<Department>();
        }
    }
}
