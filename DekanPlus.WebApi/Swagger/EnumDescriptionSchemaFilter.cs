using DekanPlus.Domain.Enums;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DekanPlus.WebApi.Swagger;

public class EnumDescriptionSchemaFilter : ISchemaFilter
{
    private static readonly Dictionary<Type, (string Title, (string Name, string Description)[] Values)> Descriptions = new()
    {
        [typeof(StudyForm)] = ("Вид навчання", new[]
        {
            (nameof(StudyForm.Externship), "екстернат"),
            (nameof(StudyForm.FullTimeBudget), "денне бюджетне"),
            (nameof(StudyForm.PartTimeBudget), "заочне бюджетне"),
            (nameof(StudyForm.FullTimeContract), "денне за контрактом"),
            (nameof(StudyForm.PartTimeContract), "заочне за контрактом"),
        }),
        [typeof(ExpulsionReason)] = ("Причина відрахування", new[]
        {
            (nameof(ExpulsionReason.OwnReason), "за заявою"),
            (nameof(ExpulsionReason.AcademicFailure), "академічна неуспішність"),
            (nameof(ExpulsionReason.MisconductViolation), "грубе порушення правил поведінки у ВНЗ"),
            (nameof(ExpulsionReason.Graduation), "закінчення ВНЗ"),
        }),
    };

    public void Apply(IOpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema is not OpenApiSchema concreteSchema) return;

        var enumType = Nullable.GetUnderlyingType(context.Type) ?? context.Type;
        if (!Descriptions.TryGetValue(enumType, out var info)) return;

        var lines = info.Values.Select(v => $"- `{v.Name}` — {v.Description}");
        concreteSchema.Description = $"{info.Title}:\n\n{string.Join("\n", lines)}";

        var allowed = info.Values.Select(v => v.Name).ToHashSet();
        if (concreteSchema.Enum is { } enumValues)
        {
            for (var i = enumValues.Count - 1; i >= 0; i--)
            {
                if (!allowed.Contains(enumValues[i]?.GetValue<string>() ?? string.Empty))
                    enumValues.RemoveAt(i);
            }
        }
    }
}
