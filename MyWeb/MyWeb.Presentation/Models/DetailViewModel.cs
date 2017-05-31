namespace MyWeb.Presentation.Models
{
    using Data;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class DetailViewModel
    {
        public DetailViewModel()
        {
            Comments = new List<NewsCommentModel>();
            AddNewComment = new AddNewsCommentModel();
        }

        public NewsViewModel NewsItems { get; set; }
        public IEnumerable<News> NewsRelates { get; set; }
        public IList<NewsCommentModel> Comments { get; set; }
        public AddNewsCommentModel AddNewComment { get; set; }
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
        public string NewsCategorySlug { get; set; }
        public string NewsCategoryName { get; set; }
        public string PreviousPageSlug { get; set; }
        public string PreviousPageName { get; set; }
        public string NextPageSlug { get; set; }
        public string NextPageName { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public int NumberOfComments { get; set; }
    }

    public class NewsCommentModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAvatarUrl { get; set; }
        public string CommentTitle { get; set; }
        public string CommentText { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool AllowViewingProfiles { get; set; }
        public int ParentId { get; set; }
    }

    public class AddNewsCommentModel
    {
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string CommentText { get; set; }
        public int ParentId { get; set; }
    }
}