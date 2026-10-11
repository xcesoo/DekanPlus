using System.Globalization;
using System.Text.Json.Nodes;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DekanPlus.WebApi.Swagger;

public class ExamplesSchemaFilter : ISchemaFilter
{
    public void Apply(IOpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema is not OpenApiSchema concreteSchema) return;

        var memberName = context.MemberInfo?.Name;
        var today = DateTime.UtcNow;

        if (context.Type == typeof(DateOnly) || context.Type == typeof(DateOnly?))
        {
            var example = memberName == "BirthDate"
                ? new DateOnly(today.Year - 20, 5, 15)
                : DateOnly.FromDateTime(today);
            var text = example.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            concreteSchema.Example = JsonValue.Create(text);
            concreteSchema.Description = string.IsNullOrWhiteSpace(concreteSchema.Description)
                ? $"Дата у форматі yyyy-MM-dd. Приклад: {text}"
                : concreteSchema.Description;
        }
        else if (context.Type == typeof(int) && memberName == "EnrollmentYear")
        {
            concreteSchema.Example = JsonValue.Create(today.Year);
        }
    }
}
