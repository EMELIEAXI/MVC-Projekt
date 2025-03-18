using Microsoft.Data.SqlClient; //för SQLConnectionStringBuilder
using Microsoft.EntityFrameworkCore; // för AddDBContext, UseSqlServer metod
using Microsoft.Extensions.DependencyInjection;//för IserviceCollection
using Northwind.DataContext.SqlServer;
using PraktiskaAppar;

namespace Northwind.EntityModels

{
    public static class NorthwindContextExtensions
    {
        public static IServiceCollection AddNorthwindContext(
            this IServiceCollection services, //typen som extends 
            string? connectionString = null)
        {
            if (connectionString == null)
            {
                SqlConnectionStringBuilder builder = new();

                builder.DataSource = "(localdb)\\MSSQLLocalDB";
                builder.InitialCatalog = "NorthwindDatabase";
                builder.TrustServerCertificate = true;
                builder.MultipleActiveResultSets = true;

                //visar timeout i 3 sekunder, defalut är 15 sekunder
                builder.ConnectTimeout = 3;

                //om ni använder Window Authentication
                builder.IntegratedSecurity = true;

                //om ni vill använda SQL Server Authentication
                //builder.UserID = Environment.GetEnvironmentVariable("MY_SQL_USR");
                //builder.Password = Environment.GetEnvironmentVariable("MY_SQL_PWD");

                connectionString = builder.ConnectionString;
            }
            services.AddDbContext<NorthwindDatabaseContext>(options =>
            {
                options.UseSqlServer(connectionString);

                options.LogTo(NorthwindContextLogger.WriteLine,
                    new[] { Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.CommandExecuting });
            },
            
            contextLifetime:ServiceLifetime.Transient,
            optionsLifetime: ServiceLifetime.Transient
            );

            return services;
        }   
        /// <summary>
    }
}
