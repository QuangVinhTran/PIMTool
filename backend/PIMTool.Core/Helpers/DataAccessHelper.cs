using System.Reflection;
using DbUp;
using Microsoft.Extensions.Configuration;
using PIMTool.Core.Constants;
using PIMTool.Core.Exceptions;

namespace PIMTool.Core.Helpers;

public static class DataAccessHelper
{
    private static IConfiguration _configuration;
    private static string connectionString;
    
    public static void InitConfiguration(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public static string GetDefaultConnection()
    {
        return GetConnection(CoreConstants.DEFAULT_CONNECTION);
    }

    public static string GetConnection(string connectionName)
    {
        connectionString = _configuration.GetConnectionString(connectionName) ?? throw new MissingConnectionStringException();
        return connectionString;
    }

    public static void MigrateDatabase(string assemblyName)
    {
        var connection = connectionString.IsNullOrEmpty() ? GetDefaultConnection() : connectionString;
        EnsureDatabase.For.SqlDatabase(connection);
        
        var upgradeEngine = DeployChanges.To.SqlDatabase(connection)
            .WithScriptsEmbeddedInAssembly(Assembly.Load(assemblyName))
            .LogToConsole()
            .Build();
        var scripts = upgradeEngine.GetDiscoveredScripts();
        if (scripts.Any())
        {
            upgradeEngine.PerformUpgrade();
        }
        else
        {
            Console.WriteLine("No scripts found");
        }
    }
}