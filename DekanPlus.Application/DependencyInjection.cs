using DekanPlus.Application.Common.Rules;
using DekanPlus.Application.Common.Rules.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DekanPlus.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        var studentRules = configuration.GetSection(StudentRulesOptions.SectionName).Get<StudentRulesOptions>()
                           ?? new StudentRulesOptions();
        services.AddSingleton(studentRules);

        services.AddScoped<IRuleEngine, RuleEngine>();

        var ruleTypes = typeof(RuleEngine).Assembly.GetTypes()
            .Where(t => typeof(IBusinessRule).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

        foreach (var ruleType in ruleTypes)
        {
            services.AddScoped(typeof(IBusinessRule), ruleType);
        }

        return services;
    }
}
