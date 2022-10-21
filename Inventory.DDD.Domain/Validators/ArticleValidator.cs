using FluentValidation;

namespace Inventory.DDD.Domain.Validators
{
    public class ArticleValidator : AbstractValidator<Article>
    {
        public ArticleValidator()
        {
            RuleFor(p => p.Name)
                .NotNull()
                .NotEmpty().WithMessage("Campo obligatorio")
                .Length(5, 50).WithMessage($"Mínimo de {0} y máximo de {1} caracteres");

            RuleFor(p => p.Description)
                .NotNull()
                .NotEmpty().WithMessage("Campo obligatorio")
                .MaximumLength(150).WithMessage($"Máximo de {0} caracteres");

            RuleFor(p => p.Brand)
                .NotNull()
                .NotEmpty().WithMessage("Campo obligatorio")
                .Length(3, 50).WithMessage($"Mínimo de {0} y máximo de {1} caracteres");

            RuleFor(p => p.Type)
               .NotNull()
               .NotEmpty().WithMessage("Campo obligatorio")
               .Length(5, 50).WithMessage($"Mínimo de {0} y máximo de {1} caracteres");

            RuleFor(p => p.Price)
               .NotNull()
               .NotEmpty().WithMessage("Campo obligatorio");

            RuleFor(p => p.Stock)
               .NotNull()
               .NotEmpty().WithMessage("Campo obligatorio");

            RuleFor(p => p.ExpirationDate)
              .NotNull()
              .NotEmpty().WithMessage("Campo obligatorio");
        }
    }
}