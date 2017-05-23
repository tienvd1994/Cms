using MyWeb.Core.Infrastructure.Mapper;
using MyWeb.Data;
using MyWeb.Presentation.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWeb.Presentation.Areas.Admin.Extensions
{
    public static class MappingExtensions
    {
        public static TDestination MapTo<TSource, TDestination>(this TSource source)
        {
            return AutoMapperConfiguration.Mapper.Map<TSource, TDestination>(source);
        }

        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
        {
            return AutoMapperConfiguration.Mapper.Map(source, destination);
        }

        public static NewsViewModel ToModel(this News entity)
        {
            return entity.MapTo<News, NewsViewModel>();
        }

        public static News ToEntity(this NewsViewModel model)
        {
            return model.MapTo<NewsViewModel, News>();
        }

        public static News ToEntity(this NewsViewModel model, News destination)
        {
            return model.MapTo(destination);
        }
    }
}