using Application.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    { 
        services.AddScoped<CreateAuthorUseCase>();
        services.AddScoped<CreateCategoryUseCase>();
        services.AddScoped<CreateBookUseCase>();
        services.AddScoped<ListBookTitlesUseCase>();
        services.AddScoped<GetBookDetailUseCase>();
        services.AddScoped<CreateCountryUseCase>();
        services.AddScoped<CreateStateUseCase>();
        
        return services;
    }
}