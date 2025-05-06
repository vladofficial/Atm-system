using Abstractions.Repositories;
using Application.Authorization;
using Application.BankAccounts;
using Application.Logging;
using Contracts.Authorization;
using Contracts.BankAccounts;
using Contracts.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthorizationService, AuthorizationService>();
        services.AddScoped<BankAccountService>();

        services.AddScoped<IBankAccountService>(provider =>
        {
            IBankAccountService bankAccountService = provider.GetRequiredService<BankAccountService>();
            ILogRepository logRepository = provider.GetRequiredService<ILogRepository>();
            return new BankAccountLoggingService(bankAccountService, logRepository);
        });

        
        services.AddScoped<ILogService, LogService>();

        return services;
    }
}