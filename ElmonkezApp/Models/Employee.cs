using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ElmonkezApp.Models
{
    [Table("Employees", Schema = "dbo")]
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "EmployeeId")]
        public int EmployeeId { get; set; }

        [Required]
        [Column(TypeName = "varchar(150)")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        
        [Required]
        [Column(TypeName = "varchar(200)")]
        [Display(Name = "Job")]
        public string Job { get; set; }
       
        [Display(Name = "Image")]
        public string Image { get; set; }

       
    }
}
