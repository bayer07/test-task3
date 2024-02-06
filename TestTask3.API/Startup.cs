using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Data;
using System.Data.SqlClient;
using TestTask3.Profiles;
using TestTask3.Services;

namespace TestTask3
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<FileService>();
            services.AddSingleton<IDbConnection>(ctx =>
            {
                return new SqlConnection("Server=.\\SQLExpress;Database=dbname; Trusted_Connection=Yes;");
            });
            services.AddAutoMapper(typeof(MappingProfile));
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(x => x.WithOrigins("http://localhost:3000").WithMethods("POST").AllowAnyHeader());

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapGet("/", async context =>
                {
                    context.Response.Redirect("api/files");
                });
            });
        }
    }
}
