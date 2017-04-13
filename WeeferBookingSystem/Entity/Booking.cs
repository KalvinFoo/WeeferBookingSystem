using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using WeeferBookingSystem.Entity;

namespace WeeferBookingSystem
{
    public partial class Booking
    {
        public int Id { get; set; }
        public string Description { get; set; }

        [Required]
        [Display(Name = "Category")]
        [UIHint("ForeignObject")]
        public int CategoryId { get; set; }


        [Required]
        [Display(Name = "Date From")]
        public DateTime DateFrom { get; set; }


        [Required]
        [Display(Name = "Date To")]
        public DateTime DateTo { get; set; }

        [Display(Name = "Status")]
        [UIHint("Enum")]
        public Status Status { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdateDateTime { get; set; }

        public virtual Category Category { get; set; }
    }
}