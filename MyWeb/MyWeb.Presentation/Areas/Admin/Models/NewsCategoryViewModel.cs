using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyWeb.Presentation.Areas.Admin.Models
{
    public class NewsCategoryViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Name { get; set; }

        public bool Published { get; set; }

        [StringLength(500)]
        public string MetaKeywords { get; set; }

        [StringLength(500)]
        public string MetaDescription { get; set; }

        [StringLength(500)]
        public string MetaTitle { get; set; }
    }
}