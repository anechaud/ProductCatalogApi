using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProductCatalouge.Api.Middleware;
using ProductCatalouge.Core.Configuration;
using ProductCatalouge.Core.Interfaces;
using ProductCatalouge.Core.Mapper;
using ProductCatalouge.EntityFramework.Models.DB;
using ProductCatalouge.Infrastructure.Implementation.Repository;
using ProductCatalouge.Infrastructure.Implementation.Service;
using Serilog;
using System.Text;

namespace ProductCatalouge.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
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
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IProductService, ProductService>();
            services.AddSingleton(new MapperConfiguration(mc => mc.AddProfile(new MappingProfile()))
               .CreateMapper());
            var key = Encoding.ASCII.GetBytes(secret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseExceptionHandlerMiddleware();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product Catalouge Api");
            });
            app.UseSerilogRequestLogging();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
