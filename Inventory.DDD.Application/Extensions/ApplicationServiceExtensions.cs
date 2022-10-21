using System.Reflection;
using Carter;
using FluentValidation;
using Inventory.DDD.Application.Commands;
using Inventory.DDD.Application.Helpers;
using Inventory.DDD.Application.Queries;
using Inventory.DDD.Application.Security;
using Inventory.DDD.Domain;
using Inventory.DDD.Domain.IRepositories;
using Inventory.DDD.Domain.Validators;
using Inventory.DDD.Infrastructure;
using Inventory.DDD.Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.DDD.Application.Extensions
{
    /// <summary>
    /// Clase personalizada para las configuraciones de servicios, inyecciones de dependencias,
    /// conexiones de bbdd, etc.
    /// </summary>
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IArticleRepository, ArticleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddAuthentication("BasicAuthentication").AddScheme<AuthenticationSchemeOptions, BasicAuthHandler>("BasicAuthentication", null);

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            services.AddMediatR(typeof(Application));
            services.AddValidatorsFromAssemblyContaining(typeof(Application));
            services.AddTransient<IValidator<Article>, ArticleValidator>();

            services.AddDbContext<DataContext>(options => {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            return services;
        }
    }
}
