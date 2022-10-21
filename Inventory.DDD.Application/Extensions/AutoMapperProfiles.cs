using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Inventory.DDD.Domain;
using Inventory.DDD.Domain.DTOs;
using static Inventory.DDD.Application.Commands.UpdateArticle;
using static Inventory.DDD.Application.Queries.GetArticle;

namespace Inventory.DDD.Application.Helpers
{
    /// <summary>
    /// Mapeo de entidades usando Automapper
    /// </summary>
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Article, ArticleDTO>();
            CreateMap<ArticleDTO, Article>();

            CreateMap<Article, UpdateArticleCommand>();
            CreateMap<UpdateArticleCommand, Article>();
            
        }
    }
}
