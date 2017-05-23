using MyWeb.Core;
using MyWeb.Data;
using MyWeb.Presentation.Areas.Admin.Models;
using MyWeb.Services.NewsGroup;
using System;
using System.Net;
using System.Web.Mvc;

namespace MyWeb.Presentation.Areas.Admin.Controllers
{
    [Authorize]
    public class NewsGroupController : Controller
    {
        private readonly INewsCategoryService _newsCategoryService;

        public NewsGroupController(INewsCategoryService newsCategoryService)
        {
            _newsCategoryService = newsCategoryService;
        }

        // GET: Admin/Category
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            return View(_newsCategoryService.GetAll());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NewsCategoryViewModel newsCategoryViewModel)
        {
            ModelState.Remove("Slug");

            if (!ModelState.IsValid)
            {
                return View(newsCategoryViewModel);
            }
            var newsCategory = new NewsCategory();
            newsCategory.Name = newsCategoryViewModel.Name;
            newsCategory.Slug = CommonHelper.NameToSlug(newsCategoryViewModel.Name);
            newsCategory.Published = true;
            newsCategory.CreatedOnUtc = DateTime.Now;
            newsCategory.MetaKeywords = newsCategoryViewModel.MetaKeywords;
            newsCategory.MetaDescription = newsCategoryViewModel.MetaDescription;
            newsCategory.MetaTitle = newsCategoryViewModel.MetaTitle;
            newsCategory.ParentCategoryId = -1;
            _newsCategoryService.Insert(newsCategory);

            return RedirectToAction("List");
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var newsCategory = _newsCategoryService.GetById((int)id);

            if (newsCategory == null)
            {
                return HttpNotFound();
            }

            var newsCategoryViewModel = new NewsCategoryViewModel()
            {
                Id = newsCategory.Id,
                Name = newsCategory.Name,
                Published = newsCategory.Published,
                MetaKeywords = newsCategory.MetaKeywords,
                MetaDescription = newsCategory.MetaDescription,
                MetaTitle = newsCategory.MetaTitle,
            };

            return View(newsCategoryViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NewsCategoryViewModel newsCategoryViewModel)
        {
            ModelState.Remove("Slug");

            if (!ModelState.IsValid)
            {
                return View(newsCategoryViewModel);
            }

            var newsCategoryGetById = _newsCategoryService.GetById(newsCategoryViewModel.Id);
            newsCategoryGetById.Name = newsCategoryViewModel.Name;
            newsCategoryGetById.Slug = CommonHelper.NameToSlug(newsCategoryViewModel.Name);
            newsCategoryGetById.Published = newsCategoryViewModel.Published;
            newsCategoryGetById.UpdatedOnUtc = DateTime.Now;
            newsCategoryGetById.MetaKeywords = newsCategoryViewModel.MetaKeywords;
            newsCategoryGetById.MetaDescription = newsCategoryViewModel.MetaDescription;
            newsCategoryGetById.MetaTitle = newsCategoryViewModel.MetaTitle;
            newsCategoryGetById.ParentCategoryId = -1;
            _newsCategoryService.Update(newsCategoryGetById);

            return RedirectToAction("List");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var newsCategory = _newsCategoryService.GetById((int)id);

            if (newsCategory == null)
            {
                return HttpNotFound();
            }

            _newsCategoryService.Delete(newsCategory);

            return RedirectToAction("List");
        }
    }
}