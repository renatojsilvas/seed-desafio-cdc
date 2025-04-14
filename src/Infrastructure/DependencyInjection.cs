using System.Data;
using Domain.Author;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddScoped<IDbConnection>(sp =>
            new MySqlConnection(connectionString));
        
        services.AddScoped<IAuthorRepository, AuthorRepository>();
        
        
        return services;
    }
}