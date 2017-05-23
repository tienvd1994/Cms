using MyWeb.Core;
using MyWeb.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWeb.Services.NewsItem
{
    public partial interface INewsService
    {
        #region News

        void DeleteNews(News news);
        News GetNewsById(int newsId);
        IList<News> GetNewsByIds(int[] newsIds);
        IPagedList<News> GetAllNews(int languageId = 0, int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);
        void InsertNews(News news);
        void UpdateNews(News news);
        IList<News> GetNewsByCategoryId(int categoryId);
        News GetBySlug(string slug);
        IList<News> GetNewsRelateById(int newsId);
        News GetPreviousOrNextPage(int newsId, bool isPrevious);

        #endregion

        #region News comments

        IList<NewsComment> GetAllComments(int customerId = 0, int storeId = 0, int? newsItemId = null, bool? approved = null, DateTime? fromUtc = null, DateTime? toUtc = null, string commentText = null);
        NewsComment GetNewsCommentById(int newsCommentId);
        IList<NewsComment> GetNewsCommentsByIds(int[] commentIds);
        int GetNewsCommentsCount(News newsItem, int storeId = 0, bool? isApproved = null);
        void DeleteNewsComment(NewsComment newsComment);
        void DeleteNewsComments(IList<NewsComment> newsComments);

        #endregion
    }
}
