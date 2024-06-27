using System;
using Common.Helpers;
using DTO;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace API.Infrastructure;

internal class DtoExampleFilter : ISchemaFilter
{
    #region StaticProperties
    private static int DefaultInt { get; } = 1;

    private static int AnotherInt { get; } = 2;

    private static double DefaultDouble { get; } = 2.72;

    private static double AnotherDouble { get; } = 3.14;

    private static int _counter;

    private static int Counter => ++_counter;
    #endregion

    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        schema.Example = schema.Type switch
        {
            "string" when schema.Format == "date-time" => new OpenApiString(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ")),
            "string" => new OpenApiString($"_string{Counter}"),
            "integer" => new OpenApiInteger(DefaultInt),
            "decimal" => new OpenApiDouble(DefaultDouble),
            _ => schema.Example,
        }; 
        
        if (context.Type == typeof(AuthDto.Login))
            schema.Example = new OpenApiObject
            {
                [$"{nameof(AuthDto.Login.UserName)}"] = new OpenApiString("Admin"),
                [$"{nameof(AuthDto.Login.Password)}"] = new OpenApiString("x"),
            };

        else
            schema.Example = schema.Example;
    }
}
