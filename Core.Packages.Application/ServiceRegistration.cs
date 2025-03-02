using Core.Packages.Application.Common.AutoMapper;
using Core.Packages.Application.Common.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection.Metadata;
namespace Core.Packages.Application
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddCoreApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile).Assembly);
            services.AddValidatorsFromAssembly(typeof(AssemblyReference).Assembly);
            services.AddValidatorsFromAssemblyContaining<RegisterCommandValidator>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
            services.AddTransient<IMediator, Mediator>();

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachePipelineBehavior<,>)); // 👈 Cache Pipeline'ı ekledik!
            return services;
        }

    }
}
