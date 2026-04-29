using Microsoft.AspNetCore.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace IdentityServer.Extension
{
    public static class SwaggerExtension
    {
        static public void GenerateSwaggerDoc(this SwaggerGenOptions c, IWebHostEnvironment env)
        {
            c.SwaggerDoc("App", new OpenApiInfo
            {
                Title       = "Proxy Service API",
                Version     = "v1",
                Description = $"Current environment: {env.EnvironmentName}"
            });

            c.SwaggerDoc("BackStage", new OpenApiInfo
            {
                Title       = "Proxy BackStage API",
                Version     = "v1",
                Description = $"Current environment: {env.EnvironmentName}"
            });

            c.SwaggerDoc("Test", new OpenApiInfo
            {
                Title       = "Proxy Service Test",
                Version     = "v1",
                Description = $"Current environment: {env.EnvironmentName}"
            });

        }

        static public void SwaggerEndpoints(this SwaggerUIOptions c)
        {
            c.SwaggerEndpoint("/swagger/App/swagger.json", "App API");
            c.SwaggerEndpoint("/swagger/BackStage/swagger.json", "BackStage API");
            c.SwaggerEndpoint("/swagger/Test/swagger.json", "Test API");
        }
    }
}
