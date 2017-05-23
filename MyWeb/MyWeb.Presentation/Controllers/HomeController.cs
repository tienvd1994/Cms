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
                CreatedOnUtc = news.CreatedOnUtc
            };

            detailViewModel.NewsItems = newsViewModel;
            detailViewModel.NewsRelates = _newsService.GetNewsRelateById(news.Id);

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

        [ChildActionOnly]
        public PartialViewResult ListCategoryPartial()
        {
            return PartialView("_ListCategoryPartial", _newsCategoryService.GetAll());
        }
    }
}