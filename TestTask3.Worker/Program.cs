using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Data;
using System.Data.SqlClient;

namespace TestTask3.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IDbConnection>(ctx =>
                    {
                        var connectionString = hostContext.Configuration.GetConnectionString("MyDatabase");
                        var connection = new SqlConnection(connectionString);
                        connection.Open();
                        return connection;
                    });
                    services.AddHostedService<Worker>();
                });
    }
}
