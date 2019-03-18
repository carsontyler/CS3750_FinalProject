using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseProject
{
    public class Salary
    {
        [Key]
        public int SalaryId { get; set; }
        [Required]
        public int SalaryAmount { get; set; }
        [Required]
        public short Year { get; set; }
    }
}
