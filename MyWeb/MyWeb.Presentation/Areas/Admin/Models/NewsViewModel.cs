using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MyWeb.Presentation.Areas.Admin.Models
{
    public class NewsViewModel
    {
        public int Id { get; set; }

        [Required]
        public int NewsCategoryId { get; set; }
        public int LanguageId { get; set; }

        [Required]
        [AllowHtml]
        public string Title { get; set; }

        [Required]
        [AllowHtml]
        public string Short { get; set; }

        [Required]
        [AllowHtml]
        public string Full { get; set; }

        public bool AllowComments { get; set; }

        [UIHint("DateTimeNullable")]
        public DateTime? StartDate { get; set; }

        [UIHint("DateTimeNullable")]
        public DateTime? EndDate { get; set; }
        [AllowHtml]
        public string MetaKeywords { get; set; }
        [AllowHtml]
        public string MetaDescription { get; set; }
        [AllowHtml]
        public string MetaTitle { get; set; }

        public bool Published { get; set; }

        public bool ApprovedComments { get; set; }

        public string ImageUrl { get; set; }
    }
}