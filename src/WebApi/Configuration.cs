using DotNetEnv;

namespace WebApi;

public static class Configuration
{
    public static void AddConfiguration(this WebApplicationBuilder builder)
    {
        Env.Load();
        
        builder.Configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();
    }
}