using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Carter;
using Carter.ModelBinding;
using FluentValidation;
using Inventory.DDD.Domain;
using Inventory.DDD.Domain.Events;
using Inventory.DDD.Domain.Helpers;
using Inventory.DDD.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Inventory.DDD.Application.Commands;
public class DeleteArticle : ICarterModule
{
    /// <summary>
    /// Comando para la acción de eliminar un artículo, siguiendo el patrón CQRS
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/article/remove-article-name/{name}", async (IMediator mediator, string name) =>
        {
            return await mediator.Send(new DeleteArticleCommand(name));
        })
        .WithName(nameof(CreateArticle))
        .WithTags(nameof(Article))
        .ProducesValidationProblem()
        .Produces(StatusCodes.Status201Created);
    }
    public record DeleteArticleCommand(string Name) : IRequest<IResult>;

    // Handler
    public class DeleteArticleHandler : IRequestHandler<DeleteArticleCommand, IResult>
    {            
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;        

    public DeleteArticleHandler(IUnitOfWork unitOfWork, IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _mediator = mediator;
    }
    public async Task<IResult> Handle(DeleteArticleCommand request, CancellationToken cancellationToken)
    {                
        var article = await _unitOfWork.ArticleRepository.GetArticleByNameAsync(request.Name);

        if (article is null)
        {
            return Results.NotFound();
        }

        //await _mediator.Publish(new ArticleDeleteEvent(article));
        article.DomainEvents.Add(new ArticleDeleteEvent(article, $"Se ha borrado el artículo {article.Name}"));

        _unitOfWork.ArticleRepository.RemoveArticle(article);
            
        if (await _unitOfWork.Complete())              
            return Results.Ok(article);
                                                                               
            return Results.BadRequest("Problem removing the article");                
        }
    }       
}
    
   
