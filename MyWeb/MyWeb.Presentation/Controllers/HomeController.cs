using MyWeb.Data;
using MyWeb.Presentation.Models;
using MyWeb.Services.NewsGroup;
using MyWeb.Services.NewsItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyWeb.Presentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly INewsService _newsService;
        private readonly INewsCategoryService _newsCategoryService;

        public HomeController(INewsService newsService, INewsCategoryService newsCategoryService)
        {
            _newsService = newsService;
            _newsCategoryService = newsCategoryService;
        }

        public ActionResult Index()
        {
            return View(_newsService.GetAllNews());
        }

        public ActionResult Detail()
        {
            string slug = string.Empty;

            if (RouteData.Values["slug"] != null)
            {
                slug = RouteData.Values["slug"].ToString();
            }

            var news = _newsService.GetBySlug(slug);
            var detailViewModel = new DetailViewModel();
            var previousPage = _newsService.GetPreviousOrNextPage(news.Id, true);
            var nextPage = _newsService.GetPreviousOrNextPage(news.Id, false);
            var numberOfComments = _newsService.GetNewsCommentsCount(news);

            var newsViewModel = new NewsViewModel()
            {
                Id = news.Id,
                LanguageId = news.LanguageId,
                Title = news.Title,
                Short = news.Short,
                Full = news.Full,
                Published = news.Published,
                AllowComments = news.AllowComments,
                LimitedToStores = news.LimitedToStores,
                MetaKeywords = news.MetaKeywords,
                MetaDescription = news.MetaDescription,
                MetaTitle = news.MetaTitle,
                ImageUrl = news.ImageUrl,
                Slug = news.Slug,
                NewsCategoryId = news.NewsCategoryId,
                PreviousPageSlug = previousPage?.Slug,
                PreviousPageName = previousPage?.Title,
                NextPageSlug = nextPage?.Slug,
                NextPageName = nextPage?.Title,
                CreatedOnUtc = news.CreatedOnUtc,
                NumberOfComments = numberOfComments
            };

            detailViewModel.NewsItems = newsViewModel;
            detailViewModel.NewsRelates = _newsService.GetNewsRelateById(news.Id);
            var newsComments = news.NewsComments.Where(comment => comment.IsApproved);

            foreach (var newsComment in newsComments.OrderBy(comment => comment.CreatedOnUtc))
            {
                var commentModel = new NewsCommentModel()
                {
                    Id = newsComment.Id,
                    CustomerId = newsComment.CustomerId,
                    CustomerName = newsComment.Customer.Email,
                    CommentTitle = newsComment.CommentTitle,
                    CommentText = newsComment.CommentText,
                    CreatedOn = newsComment.CreatedOnUtc,
                    AllowViewingProfiles = false
                };

                detailViewModel.Comments.Add(commentModel);
            }

            return View(detailViewModel);
        }

        public ActionResult ListCategory()
        {
            string slug = string.Empty;

            if (RouteData.Values["slug"] != null)
            {
                slug = RouteData.Values["slug"].ToString();
            }

            var category = _newsCategoryService.GetBySlug(slug);

            return View(_newsService.GetNewsByCategoryId(category.Id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewsCommentAdd(int newsId, DetailViewModel detailViewModel)
        {
            var newsItem = _newsService.GetNewsById(newsId);

            if (newsItem == null || !newsItem.Published || !newsItem.AllowComments)
                return RedirectToRoute("/");

            if (ModelState.IsValid)
            {
                var comment = new NewsComment
                {
                    NewsItemId = newsItem.Id,
                    CustomerId = 11,
                    CommentTitle = detailViewModel.AddNewComment.Email,
                    CommentText = detailViewModel.AddNewComment.CommentText,
                    IsApproved = true,
                    StoreId = 1,
                    CreatedOnUtc = DateTime.UtcNow,
                };
                newsItem.NewsComments.Add(comment);
                _newsService.UpdateNews(newsItem);

                //The text boxes should be cleared after a comment has been posted
                //That' why we reload the page
                TempData["nop.news.addcomment.result"] = comment.IsApproved ? "Thành công" : "Thất bại";

                return RedirectToRoute("Detail", new { slug = newsItem.Slug });
            }


            //If we got this far, something failed, redisplay form
            return View(detailViewModel);
        }

        [ChildActionOnly]
        public PartialViewResult ListCategoryPartial()
        {
            return PartialView("_ListCategoryPartial", _newsCategoryService.GetAll());
        }
    }
}