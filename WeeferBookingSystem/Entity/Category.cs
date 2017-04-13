using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace WeeferBookingSystem
{
    public partial class Category
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Display(Name = "Required Approval")]
        public Boolean? IsRequiredApproval { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdateDateTime { get; set; }
    }
}