using CampusClash.Application.Interfaces;
using CampusClash.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Resend;

namespace CampusClash.Infrastructure;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureEmail(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddResend(options =>
            options.ApiToken = configuration["Resend:ApiKey"]!);

        services.AddScoped<IEmailService, EmailService>();

        return services;
    }
}
