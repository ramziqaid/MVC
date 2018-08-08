using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Jobs.Models
{
    public class ApplyForJob
    {
        public int Id { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        [Display (Name ="الملاحظة")]
        public string Message { get; set; }

        [Display(Name = "تاريخ التقديم")]
        public DateTime ApplyDate { get; set; }

    
        public int JobId { get; set; }
        public virtual Job Job { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}