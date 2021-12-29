using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Goal.Infra.Http.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Goal.Demo.Api.Swagger
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        public void Configure(SwaggerGenOptions options)
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Goal API", Version = "v1" });
            options.IncludeXmlComments(GetXmlCommentsFile());
            options.DocumentFilter<LowerCaseDocumentFilter>();
            options.DescribeAllParametersInCamelCase();
        }

        private static string GetXmlCommentsFile()
        {
            string xmlFile = $"{Assembly.GetEntryAssembly().GetName().Name}.xml";
            return Path.Combine(AppContext.BaseDirectory, xmlFile);
        }
    }
}