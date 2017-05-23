namespace MyWeb.Presentation.Models
{
    using Data;
    using System;
    using System.Collections.Generic;

    public class DetailViewModel
    {
        public NewsViewModel NewsItems { get; set; }
        public IEnumerable<News> NewsRelates { get; set; }
    }

    public class NewsViewModel
    {
        public int Id { get; set; }
        public int LanguageId { get; set; }
        public string Title { get; set; }
        public string Short { get; set; }
        public string Full { get; set; }
        public bool Published { get; set; }
        public bool AllowComments { get; set; }
        public bool LimitedToStores { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        public string ImageUrl { get; set; }
        public string Slug { get; set; }
        public int NewsCategoryId { get; set; }
        public string PreviousPageSlug { get; set; }
        public string PreviousPageName { get; set; }
        public string NextPageSlug { get; set; }
        public string NextPageName { get; set; }
        public DateTime CreatedOnUtc { get; set; }
    }
}