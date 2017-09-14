using MyWeb.Core;
using MyWeb.Data;
using MyWeb.Presentation.Areas.Admin.Models;
using MyWeb.Services.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyWeb.Presentation.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: Admin/Category
        public ActionResult Index()
        {
            var categories = _categoryService.GetAllCategories("", 0, int.MaxValue, true);

            return View(categories);
        }

        public ActionResult Create()
        {
            ViewBag.ListCategory = new SelectList(_categoryService.GetAllCategories(), "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(CategoryViewModel categoryViewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ListCategory = new SelectList(_categoryService.GetAllCategories(), "Id", "Name");

                return View(categoryViewModel);
            }

            var category = new Category()
            {
                Name = categoryViewModel.Name,
                Slug = CommonHelper.NameToSlug(categoryViewModel.Name),
                Description = categoryViewModel.Description,
                CategoryTemplateId = categoryViewModel.CategoryTemplateId,
                MetaKeywords = categoryViewModel.MetaKeywords,
                MetaDescription = categoryViewModel.MetaDescription,
                MetaTitle = categoryViewModel.MetaTitle,
                ParentCategoryId = categoryViewModel.ParentCategoryId,
                PageSize = 0,
                AllowCustomersToSelectPageSize = false,
                PageSizeOptions = string.Empty,
                PriceRanges = string.Empty,
                ShowOnHomePage = false,
                IncludeInTopMenu = false,
                SubjectToAcl = false,
                LimitedToStores = false,
                Published = true,
                Deleted = false,
                DisplayOrder = 0,
                CreatedOnUtc = DateTime.Now,
                UpdatedOnUtc = DateTime.Now
            };

            _categoryService.InsertCategory(category);

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var category = _categoryService.GetCategoryById(id);
            if (category == null)
                //No news item found with the specified id
                return RedirectToAction("Index");

            var categoryViewModel = new CategoryViewModel();
            ViewBag.SelectListCategory = _categoryService.GetAllCategories();
            categoryViewModel.Name = category.Name;
            categoryViewModel.Description = category.Description;
            categoryViewModel.CategoryTemplateId = category.CategoryTemplateId;
            categoryViewModel.MetaKeywords = category.MetaKeywords;
            categoryViewModel.MetaDescription = category.MetaDescription;
            categoryViewModel.MetaTitle = category.MetaTitle;
            categoryViewModel.ParentCategoryId = category.ParentCategoryId;

            return View(categoryViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(CategoryViewModel categoryViewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ListCategory = new SelectList(_categoryService.GetAllCategories(), "Id", "Name");

                return View(categoryViewModel);
            }

            var category = new Category()
            {
                Id = categoryViewModel.Id,
                Name = categoryViewModel.Name,
                Slug = CommonHelper.NameToSlug(categoryViewModel.Name),
                Description = categoryViewModel.Description,
                CategoryTemplateId = categoryViewModel.CategoryTemplateId,
                MetaKeywords = categoryViewModel.MetaKeywords,
                MetaDescription = categoryViewModel.MetaDescription,
                MetaTitle = categoryViewModel.MetaTitle,
                ParentCategoryId = categoryViewModel.ParentCategoryId,
                PageSize = 0,
                AllowCustomersToSelectPageSize = false,
                PageSizeOptions = string.Empty,
                PriceRanges = string.Empty,
                ShowOnHomePage = false,
                IncludeInTopMenu = false,
                SubjectToAcl = false,
                LimitedToStores = false,
                Published = true,
                Deleted = false,
                DisplayOrder = 0,
                CreatedOnUtc = DateTime.Now,
                UpdatedOnUtc = DateTime.Now
            };

            _categoryService.UpdateCategory(category);

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var category = _categoryService.GetCategoryById(id);

            if (category == null)
                return RedirectToAction("Index");

            _categoryService.DeleteCategory(category);
            return RedirectToAction("Index");
        }
    }
}