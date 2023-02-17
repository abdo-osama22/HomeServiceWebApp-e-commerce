using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ElmonkezApp.Models
{
    [Table("Categories", Schema = "dbo")]
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "CategoryId")]
        public int CategoryId { get; set; }

        [Required]
        [Column(TypeName = "varchar(150)")]
        [Display(Name = "CategoryName")]
        public string CategoryName { get; set; }
        [Required]
        [Column(TypeName = "varchar(500)")]
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Image")]
        public string Image { get; set; }

        public virtual ICollection<Service> Services { get; set; }
    }
}
