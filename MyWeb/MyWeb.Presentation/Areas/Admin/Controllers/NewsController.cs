using MyWeb.Core;
using MyWeb.Data;
using MyWeb.Presentation.Areas.Admin.Models;
using MyWeb.Services.NewsGroup;
using MyWeb.Services.NewsItem;
using System;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace MyWeb.Presentation.Areas.Admin.Controllers
{
    [Authorize]
    public class NewsController : Controller
    {
        #region Fields

        private readonly INewsService _newsService;
        private readonly INewsCategoryService _newsCategoryService;

        #endregion

        #region Ctor

        public NewsController(INewsService newsService, INewsCategoryService newsCategoryService)
        {
            this._newsService = newsService;
            _newsCategoryService = newsCategoryService;
        }

        #endregion

        #region News item.
        // GET: Admin/News
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual ActionResult List()
        {
            var news = _newsService.GetAllNews(0, 0, int.MaxValue, true);

            return View(news);
        }

        public ActionResult Create()
        {
            ViewBag.ListCategory = new SelectList(_newsCategoryService.GetAll(), "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NewsViewModel newsModel, HttpPostedFileBase upload)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ListCategory = new SelectList(_newsCategoryService.GetAll(), "Id", "Name");

                return View(newsModel);
            }

            if (upload != null)
            {
                string extension = Path.GetExtension(upload.FileName);

                if (!CommonHelper.ValidateExtension(extension))
                {
                    ModelState.AddModelError("", string.Format("Kiểu tệp đuôi \"{0}\" không cho phép.", extension));

                    return View(newsModel);
                }

                string uploadDirectory = "/Uploads/Images";
                string path = Path.Combine(Server.MapPath(uploadDirectory));

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string fileName = upload.FileName;
                string imagePath = string.Empty;
                string fileNameChangedNotExtension = string.Empty;
                imagePath = Path.Combine(Server.MapPath(uploadDirectory), fileName);
                string fileNameNotExtension = Path.GetFileNameWithoutExtension(fileName);
                int i = 1;

                while (System.IO.File.Exists(imagePath))
                {
                    fileNameChangedNotExtension = string.Format("{0}-{1}", fileNameNotExtension, i);
                    i++;
                    imagePath = Path.Combine(Server.MapPath(uploadDirectory), string.Format("{0}{1}", fileNameChangedNotExtension, extension));
                }

                upload.SaveAs(imagePath);
                Image imgOriginal = Image.FromFile(imagePath);
                Image imgActual = CommonHelper.Resize(imgOriginal);
                newsModel.ImageUrl = uploadDirectory + "/" + upload.FileName;
            }

            var newsItem = new News();
            newsItem.LanguageId = 1;
            newsItem.Title = newsModel.Title;
            newsItem.Short = newsModel.Short;
            newsItem.Full = newsModel.Full;
            newsItem.Published = newsModel.Published;
            newsItem.AllowComments = newsModel.AllowComments;
            newsItem.LimitedToStores = false;
            newsItem.MetaKeywords = newsModel.MetaKeywords;
            newsItem.MetaDescription = newsModel.MetaDescription;
            newsItem.MetaTitle = newsModel.MetaTitle;
            newsItem.StartDateUtc = newsModel.StartDate;
            newsItem.EndDateUtc = newsModel.EndDate;
            newsItem.CreatedOnUtc = DateTime.UtcNow;
            newsItem.ImageUrl = newsModel.ImageUrl;
            newsItem.Slug = CommonHelper.NameToSlug(newsModel.Title);
            newsItem.NewsCategoryId = newsModel.NewsCategoryId;
            _newsService.InsertNews(newsItem);

            return RedirectToAction("List");
        }

        public ActionResult Edit(int id)
        {
            var newsItem = _newsService.GetNewsById(id);
            if (newsItem == null)
                //No news item found with the specified id
                return RedirectToAction("List");

            var newsModel = new NewsViewModel();

            ViewBag.SelectListCategory = _newsCategoryService.GetAll();
            newsModel.LanguageId = newsItem.LanguageId;
            newsModel.Title = newsItem.Title;
            newsModel.Short = newsItem.Short;
            newsModel.Full = newsItem.Full;
            newsModel.Published = newsItem.Published;
            newsModel.AllowComments = newsItem.AllowComments;
            newsModel.MetaKeywords = newsItem.MetaKeywords;
            newsModel.MetaDescription = newsItem.MetaDescription;
            newsModel.MetaTitle = newsItem.MetaTitle;
            newsModel.StartDate = newsItem.StartDateUtc;
            newsModel.EndDate = newsItem.EndDateUtc;
            newsModel.ImageUrl = newsItem.ImageUrl;
            newsModel.NewsCategoryId = newsItem.NewsCategoryId;

            return View(newsModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NewsViewModel newsModel, HttpPostedFileBase upload)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.SelectListCategory = _newsCategoryService.GetAll();

                return View(newsModel);
            }

            if (upload != null)
            {
                string extension = Path.GetExtension(upload.FileName);

                if (!CommonHelper.ValidateExtension(extension))
                {
                    ModelState.AddModelError("", string.Format("Kiểu tệp đuôi \"{0}\" không cho phép.", extension));

                    return View(newsModel);
                }

                string uploadDirectory = "/Uploads/Images";
                string path = Path.Combine(Server.MapPath(uploadDirectory));

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string fileName = upload.FileName;
                string imagePath = string.Empty;
                string fileNameChangedNotExtension = string.Empty;
                imagePath = Path.Combine(Server.MapPath(uploadDirectory), fileName);
                string fileNameNotExtension = Path.GetFileNameWithoutExtension(fileName);
                int i = 1;

                while (System.IO.File.Exists(imagePath))
                {
                    fileNameChangedNotExtension = string.Format("{0}-{1}", fileNameNotExtension, i);
                    i++;
                    imagePath = Path.Combine(Server.MapPath(uploadDirectory), string.Format("{0}{1}", fileNameChangedNotExtension, extension));
                }

                upload.SaveAs(imagePath);
                Image imgOriginal = Image.FromFile(imagePath);
                Image imgActual = CommonHelper.Resize(imgOriginal);
                newsModel.ImageUrl = uploadDirectory + "/" + upload.FileName;
            }

            var newsItem = new News();
            newsItem.Id = newsModel.Id;
            newsItem.LanguageId = 1;
            newsItem.Title = newsModel.Title;
            newsItem.Short = newsModel.Short;
            newsItem.Full = newsModel.Full;
            newsItem.Published = newsModel.Published;
            newsItem.AllowComments = newsModel.AllowComments;
            newsItem.LimitedToStores = false;
            newsItem.MetaKeywords = newsModel.MetaKeywords;
            newsItem.MetaDescription = newsModel.MetaDescription;
            newsItem.MetaTitle = newsModel.MetaTitle;
            newsItem.StartDateUtc = newsModel.StartDate;
            newsItem.EndDateUtc = newsModel.EndDate;
            newsItem.CreatedOnUtc = DateTime.UtcNow;
            newsItem.ImageUrl = newsModel.ImageUrl;
            newsItem.NewsCategoryId = newsModel.NewsCategoryId;
            newsItem.Slug = CommonHelper.NameToSlug(newsModel.Title);
            _newsService.UpdateNews(newsItem);

            return RedirectToAction("List");
        }

        public ActionResult Delete(int id)
        {
            var newsItem = _newsService.GetNewsById(id);

            if (newsItem == null)
                //No news item found with the specified id
                return RedirectToAction("List");

            _newsService.DeleteNews(newsItem);
            return RedirectToAction("List");
        }

        #endregion

        #region News comments

        public ActionResult Comments(int? filterByNewsItemId)
        {
            //"approved" property
            //0 - all
            //1 - approved only
            //2 - disapproved only
            var comments = _newsService.GetAllComments(0, 0, filterByNewsItemId);

            return View(comments);
        }

        [HttpPost]
        public virtual ActionResult CommentDelete(int id)
        {
            var comment = _newsService.GetNewsCommentById(id);

            if (comment == null)
                throw new ArgumentException("No comment found with the specified id");

            _newsService.DeleteNewsComment(comment);

            return RedirectToAction("Comments");
        }

        #endregion
    }
}