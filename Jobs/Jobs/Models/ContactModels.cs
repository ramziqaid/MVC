using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Jobs.Models
{
    public class ContactModels
    {
        [Required]
        [Display(Name = "اسم المرسل")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "الرسالة")]
        public string Message { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "الايميل")]
        public string Email { get; set; }

    }
}