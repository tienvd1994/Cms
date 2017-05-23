namespace MyWeb.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class News
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public News()
        {
            NewsComments = new HashSet<NewsComment>();
        }

        public int Id { get; set; }

        public int LanguageId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Short { get; set; }

        [Required]
        public string Full { get; set; }

        public bool Published { get; set; }

        public DateTime? StartDateUtc { get; set; }

        public DateTime? EndDateUtc { get; set; }

        public bool AllowComments { get; set; }

        public bool LimitedToStores { get; set; }

        [StringLength(400)]
        public string MetaKeywords { get; set; }

        public string MetaDescription { get; set; }

        [StringLength(400)]
        public string MetaTitle { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public string ImageUrl { get; set; }

        [StringLength(500)]
        public string Slug { get; set; }

        public int NewsCategoryId { get; set; }

        public virtual Language Language { get; set; }

        public virtual NewsCategory NewsCategory { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NewsComment> NewsComments { get; set; }
    }
}
