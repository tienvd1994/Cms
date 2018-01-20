
namespace MyWeb.Services.NewsItem
{
    using Data;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public partial class NewsService : INewsService
    {
        #region Fields

        private readonly IRepository<News> _newsItemRepository;
        private readonly IRepository<NewsComment> _newsCommentRepository;
        private readonly IRepository<StoreMapping> _storeMappingRepository;
        //private readonly CatalogSettings _catalogSettings;
        //private readonly IEventPublisher _eventPublisher;

        #endregion

        #region Ctor

        public NewsService(IRepository<News> newsItemRepository,
            IRepository<NewsComment> newsCommentRepository,
            IRepository<StoreMapping> storeMappingRepository
            //,CatalogSettings catalogSettings,
            //IEventPublisher eventPublisher
            )
        {
            this._newsItemRepository = newsItemRepository;
            this._newsCommentRepository = newsCommentRepository;
            this._storeMappingRepository = storeMappingRepository;
            //this._catalogSettings = catalogSettings;
            //this._eventPublisher = eventPublisher;
        }

        #endregion

        #region News

        public void InsertNews(News news)
        {
            if (news == null)
                throw new ArgumentNullException("news");

            _newsItemRepository.Insert(news);
        }

        public void UpdateNews(News news)
        {
            if (news == null)
                throw new ArgumentNullException("news");

            _newsItemRepository.Update(news);
        }

        public void DeleteNews(News news)
        {
            if (news == null)
                throw new ArgumentNullException("newsItem");

            _newsItemRepository.Delete(news);
        }

        public IList<News> GetAllNews(int languageId = 0, int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var query = _newsItemRepository.Table;

            if (languageId > 0)
            {
                query = query.Where(m => languageId == m.LanguageId && m.Published);
            }

            query = query.OrderByDescending(m => m.StartDateUtc ?? m.CreatedOnUtc);
            //var news = new PagedList<News>(query, pageIndex, pageSize);

            return query.ToList();
        }

        public News GetNewsById(int newsId)
        {
            if (newsId == 0)
                return null;

            return _newsItemRepository.GetById(newsId);
        }

        public IList<News> GetNewsByIds(int[] newsIds)
        {
            var query = _newsItemRepository.Table;
            return query.Where(p => newsIds.Contains(p.Id)).ToList();
        }

        public IList<News> GetNewsByCategoryId(int categoryId)
        {
            var query = _newsItemRepository.Table;
            return query.Where(m => m.NewsCategoryId == categoryId).OrderByDescending(m => m.StartDateUtc ?? m.CreatedOnUtc).ToList();
        }

        public News GetBySlug(string slug)
        {
            var query = _newsItemRepository.Table;

            return query.Where(m => m.Slug.Equals(slug))?.SingleOrDefault();
        }

        public IList<News> GetNewsRelateById(int newsId)
        {
            var query = _newsItemRepository.Table;
            var news = query.Where(m => m.Id == newsId)?.SingleOrDefault();

            return query.Where(m => m.NewsCategoryId == news.NewsCategoryId && m.Id != newsId).ToList();
        }

        public IList<News> Search(string keyword)
        {
            var query = _newsItemRepository.TableNoTracking;

            return query.Where(m => m.Title.Contains(keyword) || m.Full.Contains(keyword)).ToList();
        }

        #endregion

        #region News comments

        public void DeleteNewsComment(NewsComment newsComment)
        {
            if (newsComment == null)
                throw new ArgumentNullException("newsComment");

            _newsCommentRepository.Delete(newsComment);
        }

        public void DeleteNewsComments(IList<NewsComment> newsComments)
        {
            if (newsComments == null)
                throw new ArgumentNullException("newsComments");

            foreach (var newsComment in newsComments)
            {
                DeleteNewsComment(newsComment);
            }
        }

        public IList<NewsComment> GetAllComments(int customerId = 0, int storeId = 0, int? newsItemId = default(int?), bool? approved = default(bool?), DateTime? fromUtc = default(DateTime?), DateTime? toUtc = default(DateTime?), string commentText = null)
        {
            var query = _newsCommentRepository.Table;

            if (approved.HasValue)
                query = query.Where(comment => comment.IsApproved == approved);

            if (newsItemId > 0)
                query = query.Where(comment => comment.NewsItemId == newsItemId);

            if (customerId > 0)
                query = query.Where(comment => comment.CustomerId == customerId);

            if (storeId > 0)
                query = query.Where(comment => comment.StoreId == storeId);

            if (fromUtc.HasValue)
                query = query.Where(comment => fromUtc.Value <= comment.CreatedOnUtc);

            if (toUtc.HasValue)
                query = query.Where(comment => toUtc.Value >= comment.CreatedOnUtc);

            if (!string.IsNullOrEmpty(commentText))
                query = query.Where(c => c.CommentText.Contains(commentText) || c.CommentTitle.Contains(commentText));

            query = query.OrderBy(nc => nc.CreatedOnUtc);

            return query.ToList();
        }

        public NewsComment GetNewsCommentById(int newsCommentId)
        {
            if (newsCommentId == 0)
                return null;

            return _newsCommentRepository.GetById(newsCommentId);
        }

        public IList<NewsComment> GetNewsCommentsByIds(int[] commentIds)
        {
            if (commentIds == null || commentIds.Length == 0)
            {
                return new List<NewsComment>();
            }

            var query = from nc in _newsCommentRepository.Table
                        where commentIds.Contains(nc.Id)
                        select nc;

            var comments = query.ToList();

            //sort by passed identifiers
            var sortedComments = new List<NewsComment>();
            foreach (int id in commentIds)
            {
                var comment = comments.Find(x => x.Id == id);
                if (comment != null)
                    sortedComments.Add(comment);
            }

            return sortedComments;
        }

        /// <summary>
        /// Đếm comment của 1 news.
        /// </summary>
        /// <param name="newsItem">NewsItem</param>
        /// <param name="storeId">StoreId</param>
        /// <param name="isApproved">IsApproved</param>
        /// <returns></returns>
        public int GetNewsCommentsCount(News newsItem, int storeId = 0, bool? isApproved = default(bool?))
        {
            var query = _newsCommentRepository.Table.Where(comment => comment.NewsItemId == newsItem.Id);

            if (storeId > 0)
                query = query.Where(comment => comment.StoreId == storeId);

            if (isApproved.HasValue)
                query = query.Where(comment => comment.IsApproved == isApproved.Value);

            return query.Count();
        }

        public News GetPreviousOrNextPage(int newsId, bool isPrevious)
        {
            if (isPrevious)
            {
                var query = _newsItemRepository.TableNoTracking.Where(m => m.Id < newsId && m.Id != newsId).OrderByDescending(m => m.Id)?.FirstOrDefault();

                return query;
            }
            else
            {
                var query = _newsItemRepository.TableNoTracking.Where(m => m.Id > newsId && m.Id != newsId).OrderBy(m => m.Id)?.FirstOrDefault();

                return query;
            }
        }

        public IList<NewsComment> GetNewsCommentChildById(int newsCommentId)
        {
            var query = _newsCommentRepository.TableNoTracking;

            return query.Where(m => m.IsApproved && m.ParentId == newsCommentId).OrderByDescending(m => m.CreatedOnUtc).ToList();
        }

        #endregion
    }
}
