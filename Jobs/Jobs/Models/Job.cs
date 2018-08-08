using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jobs.Models
{
    public class Job
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "اسم الوظيفة")]
        public string JobTitle { get; set; }

        [Required(ErrorMessage ="يجب إدخال وصف الوظيفة")]
        [Display(Name = "وصف الوظيفة")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string JobDescription { get; set; }

        //[Required]
        [Display(Name = "صورة الوظيفة")]
        public string JobImgPath { get; set; }

        [Required]
        [Display(Name = "نوع الوظيفة")]
        public int CategoryID { get; set; }

       // [ForeignKey("CategoryId")]
        public virtual CategoryViewModels Category { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}