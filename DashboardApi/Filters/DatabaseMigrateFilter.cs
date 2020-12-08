using System;
using System.Reflection;
using DbUp;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DashboardApi.Filters
{
    public class DatabaseMigrateFilter: IStartupFilter
    {
        public IConfiguration _configuration { get; set; }
        public ILogger<DatabaseMigrateFilter> _logger { get; set; }
        public DatabaseMigrateFilter(IConfiguration configuration, ILogger<DatabaseMigrateFilter> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            var connectionString = _configuration.GetConnectionString("default");
            
            EnsureDatabase.For.PostgresqlDatabase(connectionString);

            var dbUpgradeEngineBuilder = DeployChanges.To
                .PostgresqlDatabase(connectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .WithTransaction()
                .LogTo(new MigrationLogger<DatabaseMigrateFilter>(_logger));

            var dbUpgradeEngine = dbUpgradeEngineBuilder.Build();

            if (dbUpgradeEngine.IsUpgradeRequired())
            {
                _logger.LogInformation("Upgrade required! Upgrading...");
                var operation = dbUpgradeEngine.PerformUpgrade();
                if (operation.Successful)
                {
                    _logger.LogInformation("Upgrade completed successfully");
                }
                else
                {
                    _logger.LogError("Upgrade failed!");
                    throw new Exception("Upgrade failed", operation.Error);
                }
            }


            return next;
        }
    }
}