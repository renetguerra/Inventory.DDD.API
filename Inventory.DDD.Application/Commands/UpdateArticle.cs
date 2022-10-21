using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Carter;
using Carter.ModelBinding;
using FluentValidation;
using Inventory.DDD.Domain;
using Inventory.DDD.Domain.Events;
using Inventory.DDD.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Inventory.DDD.Application.Commands;
public class UpdateArticle : ICarterModule
{
    /// <summary>
    /// Comando para la acción de actualizar un artículo, siguiendo el patrón CQRS
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/article", async (IMediator mediator, UpdateArticleCommand command) =>
        {
            return await mediator.Send(command);
        })
        .WithName(nameof(UpdateArticle))
        .WithTags(nameof(Article))            
        .Produces(StatusCodes.Status404NotFound)
        .ProducesValidationProblem();
    }

    public record UpdateArticleCommand(Article Article) : IRequest<IResult>;    

    // Handler
    public class UpdateArticleHandler : IRequestHandler<UpdateArticleCommand, IResult>
    {
        private readonly IValidator<UpdateArticleCommand> _validator;
        private readonly DataContext _context;
        private readonly IUnitOfWork _unitOfWork;
        
        public UpdateArticleHandler(DataContext context, IUnitOfWork unitOfWork, IValidator<UpdateArticleCommand> validator)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }
        public async Task<IResult> Handle(UpdateArticleCommand request, CancellationToken cancellationToken)
        {
            var result = _validator.Validate(request);
            if (!result.IsValid)
            {
                return Results.ValidationProblem(result.GetValidationProblems());
            }

            var article = await _unitOfWork.ArticleRepository.GetArticleByIdAsync(request.Article.Id);            

            if (article == null)
            {
                return Results.NotFound();
            }
            
            if (article != null)
            {
                if (article.Price != request.Article.Price)
                {
                    request.Article.DomainEvents.Add(new ArticleUpdatePriceEvent(article, $"El precio del artículo {article.Name} se ha modificado. Nuevo precio: {article.Price} €" ));
                }

                if (article.ExpirationDate < DateTime.Now)
                {
                    request.Article.DomainEvents.Add(new ArticleExpiredEvent(article, $"El artículo {article.Name} con fecha de expiración {article.ExpirationDate} ha caducado."));
                }

                _unitOfWork.ArticleRepository.Update(request.Article);
            }

            

            if (await _unitOfWork.Complete())
                return Results.Ok(request);

            return Results.BadRequest("Problem updating the article");                                
        }
    }

    /// <summary>
    /// Reglas de validación para la entidad Article utilizando FluentValidation
    /// </summary>
    public class UpdateArticleValidator : AbstractValidator<UpdateArticleCommand>
    {
        public UpdateArticleValidator()
        {
            RuleFor(p => p.Article.Name)
                .NotNull()
                .NotEmpty().WithMessage("Campo obligatorio")
                .Length(5, 50).WithMessage($"Mínimo de {0} y máximo de {1} caracteres");

            RuleFor(p => p.Article.Description)
                .NotNull()
                .NotEmpty().WithMessage("Campo obligatorio")
                .MaximumLength(150).WithMessage($"Máximo de {0} caracteres");

            RuleFor(p => p.Article.Brand)
                .NotNull()
                .NotEmpty().WithMessage("Campo obligatorio")
                .Length(3, 50).WithMessage($"Mínimo de {0} y máximo de {1} caracteres");

            RuleFor(p => p.Article.Type)
               .NotNull()
               .NotEmpty().WithMessage("Campo obligatorio")
               .Length(5, 50).WithMessage($"Mínimo de {0} y máximo de {1} caracteres");

            RuleFor(p => p.Article.Price)
               .NotNull()
               .NotEmpty().WithMessage("Campo obligatorio");
            
            RuleFor(p => p.Article.Stock)
               .NotNull()
               .NotEmpty().WithMessage("Campo obligatorio");

            RuleFor(p => p.Article.ExpirationDate)
              .NotNull()
              .NotEmpty().WithMessage("Campo obligatorio");
        }
    }
}
    
   
