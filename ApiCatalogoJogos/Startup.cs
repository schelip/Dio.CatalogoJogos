using System;
using System.IO;
using System.Linq;
using System.Reflection;
using ApiCatalogoJogos.Data.Infrastructure;
using ApiCatalogoJogos.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace ApiCatalogoJogos
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
            AddInjections("ApiCatalogoJogos.Business.Repositories",
                "ApiCatalogoJogos.Infrastructure.Data.Repositories", services);
            AddInjections("ApiCatalogoJogos.Business.Services",
                "ApiCatalogoJogos.Infrastructure.Services", services);

            services.AddDbContext<CatalogoJogosDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Dio.CatalogoJogos",
                    Version = "v1"
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.EnableAnnotations();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("swagger/v1/swagger.json", "Dio.CatalogoJogos v1");
                    c.RoutePrefix = string.Empty;//swagger
                });
            }

            //app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        // Util
        private void AddInjections(string contractNamespace, string implementationNamespace, IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var contractTypes = Helpers.GetTypes(contractNamespace, assembly)
            .Where(t => !t.Name.Contains("Base"));
            foreach (var ct in contractTypes)
            {
                var impName = ct.Name.Substring(1, ct.Name.Length-1);
                var it = Helpers.GetTypes(implementationNamespace, assembly)
                    .Where(t => !t.Name.EndsWith("Base"))
                    .FirstOrDefault(t => t.Name.Equals(impName));
                services.AddScoped(ct, it);
            };
        }
    }
}
