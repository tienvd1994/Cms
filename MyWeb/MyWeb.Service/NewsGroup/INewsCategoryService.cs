using MyWeb.Core;
using MyWeb.Data;
using System.Collections.Generic;

namespace MyWeb.Services.NewsGroup
{
    public partial interface INewsCategoryService
    {
        void Delete(NewsCategory news);
        NewsCategory GetById(int newsId);
        IList<NewsCategory> GetByIds(int[] newsIds);
        IPagedList<NewsCategory> GetAll(int pageIndex = 0, int pageSize = int.MaxValue);
        void Insert(NewsCategory news);
        void Update(NewsCategory news);
        NewsCategory GetBySlug(string slug);
    }
}
