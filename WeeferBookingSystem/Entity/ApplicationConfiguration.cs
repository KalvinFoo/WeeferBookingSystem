using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WeeferBookingSystem
{
    public partial class ApplicationConfiguration
    {
        public int Id { get; set; }

        [Required]
        public string ApprovalUsername { get; set; }

        [Display(Name = "SMTP Username")]
        public string SmtpUsername { get; set; }
        [Display(Name = "SMTP Password")]
        [UIHint("Password")]
        public string SmtpPassword { get; set; }
        [Display(Name = "SMTP From Name")]
        public string SmtpFromName { get; set; }
        [Display(Name = "SMTP Host")]
        public string SmtpHost { get; set; }
        [Display(Name = "SMTP Port")]
        public int SmtpPort { get; set; }
        [Display(Name = "SMTP EnableSSL")]
        public bool SmtpEnableSsl { get; set; }
    }
}