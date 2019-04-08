using System.Collections.Generic;

namespace CS3750_FinalProject
{
    public class College : InversionData
    {
        #region Properties

        public List<Department> Departments { get; set; }
        public string CollegeName { get; set; }

        #endregion

        #region Constructor

        public College(string collegeName)
        {
            CollegeName = collegeName;
            Departments = new List<Department>();
        }

        #endregion
    }
}