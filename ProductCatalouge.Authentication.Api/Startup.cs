using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ProductCatalouge.Core.Configuration;
using ProductCatalouge.Core.Interfaces;
using ProductCatalouge.EntityFramework.Models.DB;
using ProductCatalouge.Infrastructure.Implementation.Repository;
using ProductCatalouge.Infrastructure.Implementation.Service;
using System.Text;

namespace ProductCatalouge.Authentication.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            var config = new GlobalConfig();
            Configuration.Bind("ProductCatalogConfig", config);
            services.Configure<GlobalConfig>(Configuration.GetSection("ProductCatalogConfig"));

            services.AddSingleton<GlobalConfig>();
            services.AddSingleton(config);
            var connString = config.ConnectionStrings.EFCoreDBFirstDemoDatabase;
            var secret = config.Authentication.Secret;
            services.AddDbContext<ProductDetailsContext>(options =>
             options.UseSqlServer(connString),
             ServiceLifetime.Transient);
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Product Catalouge Api",
                    Version = "v1",
                    Description = "An Api to perform Crud Operation for Product Catalouge",
                    Contact = new OpenApiContact
                    {
                        Name = "Dev Team",
                        Email = "dev.team@abc.com"
                    }
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product Catalouge Api");
            });

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
