using MyWeb.Core;
using MyWeb.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyWeb.Services.Catalog
{
    public class CategoryService : ICategoryService
    {
        #region Fields

        private readonly IRepository<Category> _categoryRepository;

        #endregion

        #region Ctor

        public CategoryService(IRepository<Category> categoryRepository)
        {
            this._categoryRepository = categoryRepository;
        }

        #endregion

        public void DeleteCategory(Category category)
        {
            if (category == null)
                throw new ArgumentNullException("category");

            category.Deleted = true;
            UpdateCategory(category);

            //reset a "Parent category" property of all child subcategories
            var subcategories = GetAllCategoriesByParentCategoryId(category.Id, true);

            foreach (var subcategory in subcategories)
            {
                subcategory.ParentCategoryId = 0;
                UpdateCategory(subcategory);
            }
        }

        public IPagedList<Category> GetAllCategories(string categoryName = "",
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var query = _categoryRepository.Table;
            if (!showHidden)
                query = query.Where(c => c.Published);

            if (!string.IsNullOrWhiteSpace(categoryName))
                query = query.Where(c => c.Name.Contains(categoryName));

            query = query.Where(c => !c.Deleted);
            query = query.OrderBy(c => c.ParentCategoryId).ThenBy(c => c.DisplayOrder);
            var unsortedCategories = query.ToList();

            return new PagedList<Category>(unsortedCategories, pageIndex, pageSize);
        }

        public IList<Category> GetAllCategoriesByParentCategoryId(int parentCategoryId,
            bool showHidden = false, bool includeAllLevels = false)
        {
            var query = _categoryRepository.Table;
            if (!showHidden)
                query = query.Where(c => c.Published);
            query = query.Where(c => c.ParentCategoryId == parentCategoryId);
            query = query.Where(c => !c.Deleted);
            query = query.OrderBy(c => c.DisplayOrder);

            if (!showHidden)
            {
                query = from c in query
                        group c by c.Id
                        into cGroup
                        orderby cGroup.Key
                        select cGroup.FirstOrDefault();
                query = query.OrderBy(c => c.DisplayOrder);
            }

            var categories = query.ToList();
            if (includeAllLevels)
            {
                var childCategories = new List<Category>();
                //add child levels
                foreach (var category in categories)
                {
                    childCategories.AddRange(GetAllCategoriesByParentCategoryId(category.Id, showHidden, includeAllLevels));
                }
                categories.AddRange(childCategories);
            }
            return categories;
        }

        public IList<Category> GetAllCategoriesDisplayedOnHomePage(bool showHidden = false)
        {
            var query = from c in _categoryRepository.Table
                        orderby c.DisplayOrder
                        where c.Published &&
                        !c.Deleted &&
                        c.ShowOnHomePage
                        select c;

            var categories = query.ToList();

            return categories;
        }

        public Category GetCategoryById(int categoryId)
        {
            if (categoryId == 0)
                return new Category();

            return _categoryRepository.GetById(categoryId);
        }

        public virtual void InsertCategory(Category category)
        {
            if (category == null)
                throw new ArgumentNullException("category");

            _categoryRepository.Insert(category);
        }

        /// <summary>
        /// Updates the category
        /// </summary>
        /// <param name="category">Category</param>
        public virtual void UpdateCategory(Category category)
        {
            if (category == null)
                throw new ArgumentNullException("category");

            //validate category hierarchy
            //var parentCategory = GetCategoryById(category.ParentCategoryId);
            //while (parentCategory != null)
            //{
            //    if (category.Id == parentCategory.Id)
            //    {
            //        category.ParentCategoryId = 0;
            //        break;
            //    }
            //    parentCategory = GetCategoryById(parentCategory.ParentCategoryId);
            //}

            _categoryRepository.Update(category);
        }
    }
}
