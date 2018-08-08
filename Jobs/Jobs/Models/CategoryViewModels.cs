using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Jobs.Models
{
    public class CategoryViewModels
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "اسم الصنف")]
        public string CategoryName { get; set; }

        [Required]
        [Display(Name = "الوصف")]
        public string CategoryDescription { get; set; }

        public virtual ICollection<Job> Jobs { get; set; }
    }
}