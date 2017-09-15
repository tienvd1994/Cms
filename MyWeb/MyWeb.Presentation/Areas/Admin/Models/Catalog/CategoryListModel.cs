using System.Web.Mvc;

namespace MyWeb.Presentation.Areas.Admin.Models.Catalog
{
    public class CategoryListModel
    {
        [AllowHtml]
        public string SearchCategoryName { get; set; }
    }
}