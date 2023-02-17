using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ElmonkezApp.Models
{
    [Table("InvoicesDetails", Schema = "dbo")]
    public class InvoiceDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "InvodetailId")]
        public int InvodetailId { get; set; }

       

        [ForeignKey("ServiceId")]
        public int ServiceId { get; set; }

        [ForeignKey("InvoiceId")]
        public int InvoiceId { get; set; }

       

        public virtual Invoice Invoice { get; set; }

        public virtual Service Service { get; set; }

       
    }
}
