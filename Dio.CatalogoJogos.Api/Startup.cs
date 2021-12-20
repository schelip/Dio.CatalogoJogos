using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using Dio.CatalogoJogos.Api.Data.Infrastructure;
using Dio.CatalogoJogos.Api.Helpers;
using Dio.CatalogoJogos.Api.Infrastructure.Authorization;
using Dio.CatalogoJogos.Api.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Dio.CatalogoJogos.Api
{
    public class Startup
    {
        readonly string allowSpecificRequests = "_allowSpecificRequests";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            InjectByNamespace("Dio.CatalogoJogos.Api.Infrastructure.Data.Repositories",
                "Dio.CatalogoJogos.Api.Business.Repositories", services);
            InjectByNamespace("Dio.CatalogoJogos.Api.Infrastructure.Services",
                "Dio.CatalogoJogos.Api.Business.Services", services);

            services.AddScoped<IJwtUtils, JwtUtils>();

            services.AddDbContext<CatalogoJogosDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddControllers().AddJsonOptions(x =>
            {
                x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters()
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
                    Title = "Dio.CatalogoJogos",
                    Version = "v1"
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "Header de autoriza��o JWT (Exemplo: 'Bearer 123456abcdef')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.EnableAnnotations();
            });

            services.AddCors(options =>
            {
                options.AddPolicy(name: allowSpecificRequests,
                                  builder =>
                                  {
                                      builder
                                        .WithOrigins(Configuration.GetSection("AllowedOrigins").Get<string[]>())
                                        .AllowAnyHeader().AllowAnyMethod();
                                  });
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

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(allowSpecificRequests);

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        // Util
        private void InjectByNamespace(string contractNamespace, string implementationNamespace, IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var contractTypes = assembly.GetTypes(contractNamespace)
            .Where(t => !t.Name.Contains("Base"));
            foreach (var ct in contractTypes)
            {
                var impName = ct.Name[1..];
                var it = assembly.GetTypes(implementationNamespace)
                    .Where(t => !t.Name.EndsWith("Base"))
                    .FirstOrDefault(t => t.Name.Equals(impName));
                services.AddScoped(ct, it);
            };
        }
    }
}
