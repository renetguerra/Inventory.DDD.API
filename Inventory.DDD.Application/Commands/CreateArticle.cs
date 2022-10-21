using System.Threading;
using System.Threading.Tasks;
using Carter;
using Carter.ModelBinding;
using FluentValidation;
using Inventory.DDD.Domain;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Inventory.DDD.Application.Commands;
public class CreateArticle : ICarterModule
{
    /// <summary>
    /// Comando para la acción de añadir un artículo, siguiendo el patrón CQRS
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/articles", async (HttpRequest req, IMediator mediator, CreateArticleCommand command) =>
        {
            return await mediator.Send(command);
        })
        .WithName(nameof(CreateArticle))
        .WithTags(nameof(Article))
        .ProducesValidationProblem()
        .Produces(StatusCodes.Status201Created);
    }
    public record CreateArticleCommand(Article Article) : IRequest<IResult>;

    // Handler
    public class CreateArticleHandler : IRequestHandler<CreateArticleCommand, IResult>
    {        
        private readonly IValidator<CreateArticleCommand> _validator;
        private readonly IUnitOfWork _unitOfWork;

        public CreateArticleHandler(IUnitOfWork unitOfWork, IValidator<CreateArticleCommand> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }
        public async Task<IResult> Handle(CreateArticleCommand request, CancellationToken cancellationToken)
        {            
            var result = _validator.Validate(request);
            if (!result.IsValid)
            {
                return Results.ValidationProblem(result.GetValidationProblems());
            }

            _unitOfWork.ArticleRepository.AddArticle(request.Article);

            if (await _unitOfWork.Complete())
                return Results.Ok(request);

            return Results.BadRequest("Problem creating the article");
        }        
    }

    /// <summary>
    /// Reglas de validación para la entidad Article utilizando FluentValidation
    /// </summary>
    public class CreateArticleValidator : AbstractValidator<CreateArticleCommand>
    {
        public CreateArticleValidator()
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
    
   
