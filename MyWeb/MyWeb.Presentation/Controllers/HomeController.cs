using MyWeb.Core;
using MyWeb.Core.Caching;
using MyWeb.Data;
using MyWeb.Presentation.Models;
using MyWeb.Services.NewsGroup;
using MyWeb.Services.NewsItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MyWeb.Presentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly INewsService _newsService;
        private readonly INewsCategoryService _newsCategoryService;
        private readonly ICacheManager _cacheManager;

        public HomeController(INewsService newsService, INewsCategoryService newsCategoryService, ICacheManager cacheManager)
        {
            _newsService = newsService;
            _newsCategoryService = newsCategoryService;
            _cacheManager = cacheManager;
        }

        public ActionResult Index(int? page = null)
        {
            var listNews = _newsService.GetAllNews();
            var pager = new Pager(listNews.Count(), page);

            var viewModel = new NewsViewModel
            {
                Items = listNews.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            string url = System.Web.HttpContext.Current.Request.Url.AbsolutePath;
            ViewBag.CurrentUrl = "";

            return View(viewModel);
        }

        public ActionResult Detail()
        {
            string slug = string.Empty;

            if (RouteData.Values["slug"] != null)
            {
                slug = RouteData.Values["slug"].ToString();
            }

            var news = _newsService.GetBySlug(slug);

            if (news == null)
            {
                return HttpNotFound();
            }

            var detailViewModel = new DetailViewModel();
            var previousPage = _newsService.GetPreviousOrNextPage(news.Id, true);
            var nextPage = _newsService.GetPreviousOrNextPage(news.Id, false);
            var numberOfComments = _newsService.GetNewsCommentsCount(news);

            var newsViewModel = new Models.NewsViewModel()
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
                NewsCategoryName = news.NewsCategory == null ? string.Empty : news.NewsCategory.Name,
                NewsCategorySlug = news.NewsCategory == null ? string.Empty : news.NewsCategory.Slug,
                PreviousPageSlug = previousPage?.Slug,
                PreviousPageName = previousPage?.Title,
                NextPageSlug = nextPage?.Slug,
                NextPageName = nextPage?.Title,
                CreatedOnUtc = news.CreatedOnUtc,
                NumberOfComments = numberOfComments
            };

            detailViewModel.NewsItems = newsViewModel;
            detailViewModel.NewsRelates = _newsService.GetNewsRelateById(news.Id);
            var newsComments = news.NewsComments.Where(comment => comment.IsApproved && comment.ParentId == 0);

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
                    AllowViewingProfiles = false,
                    ParentId = newsComment.ParentId
                };

                var newsCommentChilds = _newsService.GetNewsCommentChildById(newsComment.Id);

                if (newsCommentChilds != null && newsCommentChilds.Count > 0)
                {
                    var NewsCommentChildModels = new List<NewsCommentChildModel>();

                    foreach (var item in newsCommentChilds)
                    {
                        var NewsCommentChildModel = new NewsCommentChildModel()
                        {
                            Id = item.Id,
                            CustomerId = item.CustomerId,
                            CustomerName = item.Customer.Email,
                            CommentTitle = item.CommentTitle,
                            CommentText = item.CommentText,
                            CreatedOn = item.CreatedOnUtc,
                            AllowViewingProfiles = false,
                            ParentId = newsComment.ParentId
                        };

                        NewsCommentChildModels.Add(NewsCommentChildModel);
                    }

                    commentModel.NewsCommentChildModels = NewsCommentChildModels;
                }

                detailViewModel.Comments.Add(commentModel);
            }

            //var detailViewModelCached = _cacheManager.Get(string.Format("myweb.news.detail-{0}", news.Id),
            //    () => detailViewModel);

            return View(detailViewModel);
        }

        public ActionResult ListCategory(int? page = null)
        {
            string slug = string.Empty;

            if (RouteData.Values["slug"] != null)
            {
                slug = RouteData.Values["slug"].ToString();
            }

            var category = _newsCategoryService.GetBySlug(slug);

            if (category == null)
            {
                return HttpNotFound();
            }

            var listNews = _newsService.GetNewsByCategoryId(category.Id);
            var pager = new Pager(listNews.Count(), page);

            var viewModel = new NewsViewModel
            {
                Items = listNews.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            //var viewModelCached = _cacheManager.Get(string.Format("myweb.news.by.category-{0}", category.Id), () => viewModel);
            string url = System.Web.HttpContext.Current.Request.Url.AbsolutePath;
            ViewBag.CurrentUrl = url;

            return View(viewModel);
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
                    ParentId = detailViewModel.AddNewComment.ParentId,
                    CreatedOnUtc = DateTime.UtcNow,
                };

                newsItem.NewsComments.Add(comment);
                _newsService.UpdateNews(newsItem);

                //The text boxes should be cleared after a comment has been posted
                //That' why we reload the page
                TempData["nop.news.addcomment.result"] = comment.IsApproved ? "Bình luận thành công" : "Thất bại";

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

        /// <summary>
        /// Search news.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Search(string key, int? page = null)
        {
            var listNews = _newsService.Search(key);
            var pager = new Pager(listNews.Count(), page);

            var viewModel = new NewsViewModel
            {
                Items = listNews.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            string url = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;
            var urlSplit = url.Split(Constant.AND);
            ViewBag.CurrentUrl = urlSplit[0];

            return View(viewModel);
        }
    }

    public class NewsViewModel
    {
        public IEnumerable<News> Items { get; set; }
        public Pager Pager { get; set; }
    }

    public class Pager
    {
        public Pager(int totalItems, int? page, int pageSize = 20)
        {
            // calculate total, start and end pages
            var totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);
            var currentPage = page != null ? (int)page : 1;
            var startPage = currentPage - 5;
            var endPage = currentPage + 4;

            if (startPage <= 0)
            {
                endPage -= (startPage - 1);
                startPage = 1;
            }
            if (endPage > totalPages)
            {
                endPage = totalPages;
                if (endPage > 10)
                {
                    startPage = endPage - 9;
                }
            }

            TotalItems = totalItems;
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalPages = totalPages;
            StartPage = startPage;
            EndPage = endPage;
        }

        public int TotalItems { get; private set; }
        public int CurrentPage { get; private set; }
        public int PageSize { get; private set; }
        public int TotalPages { get; private set; }
        public int StartPage { get; private set; }
        public int EndPage { get; private set; }
    }
}