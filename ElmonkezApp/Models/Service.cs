using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ElmonkezApp.Models
{
    [Table("Services", Schema = "dbo")]
    public class Service
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ServiceId")]
        public int ServiceId { get; set; }

        [Required]
        [Column(TypeName = "varchar(150)")]
        [Display(Name = "ServiceName")]
        public string ServiceName { get; set; }
        [Required]
        [Column(TypeName = "varchar(300)")]
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Required]
        [Column(TypeName = "varchar(300)")]
        [Display(Name = "Details")]
        public string Details { get; set; }
        [Required]
        [Column(TypeName = "decimal(12,2)")]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }
        [Display(Name = "Image")]
        public string Image { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<InvoiceDetail> InvoicesDetails { get; set; }
    }
}
