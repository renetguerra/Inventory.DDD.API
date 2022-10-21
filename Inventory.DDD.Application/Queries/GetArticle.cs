using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Carter;
using Inventory.DDD.Domain;
using Inventory.DDD.Domain.DTOs;
using Inventory.DDD.Domain.Helpers;
using Inventory.DDD.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Inventory.DDD.Application.Queries
{
    /// <summary>
    /// Queries para la obtención de datos siguiendo el patrón CQRS
    /// </summary>
    public class GetArticle : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("api/article", (IMediator mediator) =>
            {
                return mediator.Send(new GetArticle());
            })
            .WithName(nameof(GetArticle))
            .WithTags(nameof(Article));

        }

        #region GetArticles(List)
        public record GetArticlesQuery : IRequest<IEnumerable<Article>>;

        // Handler        
        public class Handler : IRequestHandler<GetArticlesQuery, IEnumerable<Article>>
        {
            private readonly DataContext _context;            
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IUnitOfWork unitOfWork, IMapper mapper)
            {
                _context = context;
                _unitOfWork = unitOfWork;
                _mapper = mapper;                
            }
            
            public async Task<IEnumerable<Article>> Handle(GetArticlesQuery request, CancellationToken cancellationToken)
            {                                
                var articles = await _unitOfWork.ArticleRepository.GetArticlesAsync();

                return articles;                
            }
        }

        public class GetArticlesMappingProfile : Profile
        {
            public GetArticlesMappingProfile() => CreateMap<Article, GetArticlesResponse>();                
        }
      
        public record GetArticlesResponse(Article Article);

        #endregion GetArticles(List)

        #region GetArticle(ById)

        // ByIdQuery
        public record ByIdQuery(int Id) : IRequest<IResult>;

        // Handler
        public class ByIdHandler : IRequestHandler<ByIdQuery, IResult>
        {            
            private readonly IUnitOfWork _unitOfWork;

            public ByIdHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;                
            }
            public async Task<IResult> Handle(ByIdQuery request, CancellationToken cancellationToken)
            {               
                var article = await _unitOfWork.ArticleRepository.GetArticleByIdAsync(request.Id);
                if (await _unitOfWork.Complete()) return Results.Ok(article);
                
                return Results.BadRequest("Error");                
            }
        }

        #endregion GetArticle(ById)
    }
}
