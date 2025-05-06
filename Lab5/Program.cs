using Abstractions.Repositories;
using Application.Extensions;
using Console;
using Console.Extensions;
using DataAccess.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

namespace Lab5;

public static class Program
{
    public static void Main()
    {
        
        var collection = new ServiceCollection();

        collection
            .AddApplication()
            .AddInfrastructureDataAccess(configuration =>
            {
                configuration.Host = "localhost";
                configuration.Port = 5432;
                configuration.Username = "atm_user";
                configuration.Password = "atm_password";
                configuration.Database = "atm_db";
                configuration.SslMode = "Prefer";
            })
            .AddPresentationConsole();

        ServiceProvider provider = collection.BuildServiceProvider();

        IAdminRepository admin = provider.GetRequiredService<IAdminRepository>();
        admin.ChangeAdminPassword(123, 123);
        
        using IServiceScope scope = provider.CreateScope();

        scope.UseInfrastructureDataAccess();
        
        
        
        ScenarioRunner scenarioRunner = scope.ServiceProvider
            .GetRequiredService<ScenarioRunner>();

        while (true)
        {
            scenarioRunner.Run();
            AnsiConsole.Clear();
        }
        
        
    }
}