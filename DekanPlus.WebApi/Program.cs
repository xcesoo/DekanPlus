using System.Reflection;
using System.Text.Json.Serialization;
using DekanPlus.Application;
using DekanPlus.Infrastructure;
using DekanPlus.Infrastructure.Extensions;
using DekanPlus.WebApi.Middlewares;
using DekanPlus.WebApi.Swagger;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandler<GlobalExceptionsHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "DekanPlus API",
        Version = "v1",
        Description = "API деканату «Облік студентів»: кафедри, напрями підготовки, фахи, групи та особові картки студентів."
    });

    foreach (var assembly in new[] { Assembly.GetExecutingAssembly(), typeof(DekanPlus.Application.DependencyInjection).Assembly })
    {
        var xmlFile = Path.Combine(AppContext.BaseDirectory, $"{assembly.GetName().Name}.xml");
        if (File.Exists(xmlFile))
            options.IncludeXmlComments(xmlFile, includeControllerXmlComments: true);
    }

    options.SupportNonNullableReferenceTypes();
    options.SchemaFilter<ExamplesSchemaFilter>();
    options.SchemaFilter<EnumDescriptionSchemaFilter>();
});

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);

var app = builder.Build();

await app.Services.InitializeDatabaseAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.UseAuthorization();

app.MapControllers();

app.Run();
