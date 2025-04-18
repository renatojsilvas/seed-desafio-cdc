using Application.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    { 
        services.AddScoped<CreateAuthorUseCase>();
        services.AddScoped<CreateCategoryUseCase>();
        
        return services;
    }
}