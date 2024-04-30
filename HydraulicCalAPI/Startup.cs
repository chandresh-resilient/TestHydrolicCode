using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HydraulicCalAPI.Service;
using System;
using Microsoft.OpenApi.Models;
using System.IO;
using System.Reflection;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json.Serialization;
using System.Linq;
using System.Globalization;
using System.Text.Json;

namespace HydraulicCalAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                      .AddJsonOptions(options =>
                      {
                          options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                          options.JsonSerializerOptions.Converters.Add(new DoubleConverter());
                      });
            services.AddSingleton<HydraulicCalculationService>();
            services.AddSwaggerGen(c =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                // Use StringEnumConverter for enum values
                c.SchemaFilter<EnumSchemaFilter>();
                c.SchemaFilter<DecimalSchemaFilter>();
                c.OperationFilter<DecimalOperationFilter>();

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.)
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty; // To serve the Swagger UI at the app's root
            });

        }
    }

    // Define the EnumSchemaFilter
    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                schema.Enum.Clear();
                foreach (var enumName in Enum.GetNames(context.Type))
                {
                    schema.Enum.Add(new OpenApiString(enumName));
                }
            }
        }
    }

}
public class DecimalSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type == typeof(decimal) || context.Type == typeof(decimal?))
        {
            schema.Type = "number";
            schema.Format = "double"; // 'double' is usually used to represent decimal in OpenAPI
            schema.Example = new OpenApiDouble(0.0); // Set a default example
        }
        else if (schema.Properties != null)
        {
            foreach (var property in schema.Properties.Values)
            {
                if (property.Type == "number" && (property.Format == "decimal" || property.Format == "double"))
                {
                    property.Example = new OpenApiDouble(1.2);
                }
            }
        }
    }
}
public class DoubleConverter : JsonConverter<Double>
{
    public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Check if the token type is a number and return its value directly
        if (reader.TokenType == JsonTokenType.Number)
        {
            return reader.GetDouble();
        }

        // Handle the token as a string if it is not a number
        if (reader.TokenType == JsonTokenType.String)
        {
            string stringValue = reader.GetString();
            if (stringValue == "Infinity")
                return double.PositiveInfinity;
            if (stringValue == "-Infinity")
                return double.NegativeInfinity;
            if (stringValue == "NaN")
                return double.NaN;

            // Attempt to parse the string as a double
            if (double.TryParse(stringValue, NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
            {
                return result;
            }
            // If parsing fails, throw an informative exception
            return 0.0;
        }
        return 0.0;

    }



    public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
    {
        if (double.IsPositiveInfinity(value) || double.IsNegativeInfinity(value) || double.IsNaN(value)  || value == double.MinValue || value == double.MaxValue)
        {
            writer.WriteNumberValue(0);
        }
        else
        {
            writer.WriteNumberValue(value);
        }
    }
}


public class DecimalOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        foreach (var parameter in operation.Parameters)
        {
            var schema = context.SchemaRepository.Schemas.Values
                .FirstOrDefault(s => s.Reference?.Id == parameter.Name && (s.Type == "number" || s.Type == "string"));

            if (schema != null && (schema.Format == "decimal" || schema.Type == "number"))
            {
                schema.Example = new OpenApiDouble(0.0);
            }
        }
    }
}