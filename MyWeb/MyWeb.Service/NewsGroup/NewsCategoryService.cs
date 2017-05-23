using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyWeb.Core;
using MyWeb.Data;

namespace MyWeb.Services.NewsGroup
{
    public partial class NewsCategoryService : INewsCategoryService
    {
        private readonly IRepository<NewsCategory> _newsCategoryRepository;

        public NewsCategoryService(IRepository<NewsCategory> newsCategoryRepository)
        {
            _newsCategoryRepository = newsCategoryRepository;
        }

        public void Delete(NewsCategory news)
        {
            _newsCategoryRepository.Delete(news);
        }

        public IPagedList<NewsCategory> GetAll(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _newsCategoryRepository.Table.OrderByDescending(n => n.CreatedOnUtc);
            var newsCategory = new PagedList<NewsCategory>(query, pageIndex, pageSize);

            return newsCategory;

        }

        public NewsCategory GetById(int newsId)
        {
            return _newsCategoryRepository.GetById(newsId);
        }

        public IList<NewsCategory> GetByIds(int[] newsIds)
        {
            var query = _newsCategoryRepository.Table;
            return query.Where(p => newsIds.Contains(p.Id)).ToList();
        }

        public NewsCategory GetBySlug(string slug)
        {
            var query = _newsCategoryRepository.Table;
            return query.Where(m => m.Slug.Equals(slug) && !m.Deleted)?.SingleOrDefault();
        }

        public void Insert(NewsCategory news)
        {
            _newsCategoryRepository.Insert(news);
        }

        public void Update(NewsCategory news)
        {
            _newsCategoryRepository.Update(news);
        }
    }
}
