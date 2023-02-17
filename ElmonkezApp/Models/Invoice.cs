using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ElmonkezApp.Models
{
    [Table("Invoices", Schema = "dbo")]
    public class Invoice
    {
        public Invoice()
        {
            InvoiceDetails = new List<InvoiceDetail>();

        }


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "InvoiceId")]
        public int InvoiceId { get; set; }

        [Required]
        [Display(Name = "OrderNumber")]
        public string OrderNumber { get; set; }


        [Required]
        [Display(Name = "Name")]
        public string OrderName { get; set; }

        [Required]
        [Display(Name = "Phone")]
        public string PhoneNum { get; set; }

        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string UserEmail { get; set; }


        [Required]
        [Display(Name = "Address")]
        public string OrderAddress { get; set; }

        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

       
        
       



        public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; }
    }
}
