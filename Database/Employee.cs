using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseProject
{
    class Employee
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(10)]
        public string College { get; set; }
        [Required]
        [MaxLength(20)]
        public string Department { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Rank { get; set; }

        public Salary Salary { get; set; }
    }
}
