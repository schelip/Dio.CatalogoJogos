using System;
using System.IO;
using System.Linq;
using System.Reflection;
using ApiCatalogoJogos.Business.Repositories;
using ApiCatalogoJogos.Business.Services;
using ApiCatalogoJogos.Data.Infrastructure;
using ApiCatalogoJogos.Extensions;
using ApiCatalogoJogos.Infrastructure.Data.Repositories;
using ApiCatalogoJogos.Infrastructure.Services;
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
            AddInjections("Services", services);
            AddInjections("Repositories", services);

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
        private void AddInjections(string name, IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var iTypes = Helpers.GetTypes("ApiCatalogoJogo.Business." + name, assembly)
            .Where(i => !i.Name.EndsWith("Base"));
            foreach (var i in iTypes)
            {
                var className = i.Name.Skip(1).ToString();
                var c = Helpers.GetTypes("ApiCatalogoJogos.Infrastructure." + name, assembly)
                    .Where(i => !i.Name.EndsWith("Base"))
                    .FirstOrDefault(c => c.Name.Equals(className));
                var method = typeof(IServiceCollection).GetMethod("AddScoped");
                var generic = method.MakeGenericMethod(i, c);
                generic.Invoke(services, null);
            };
        }
    }
}
