using Sample.Domain;
using Sample.Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Sample.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Sample.Web.Configuration;
using Microsoft.AspNetCore.Mvc.Formatters;
using Sample.Common;
using Sample.Common.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System.Net;

namespace Sample.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            DomainExtension.ConfigureDomain(builder.Services, builder.Configuration);

            ServiceExtension.ConfigureServices(builder.Services);

            ConfigureJWT(builder);

            AddControllersAndCustomFormater(builder);
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            AddSwaggerGen(builder);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStaticFiles();

            app.UseMiddleware<ResourcePathRewriteMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            UseStaticFile(app);

            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseMiddleware<SessionMiddleware>();

            app.MapControllers();

            app.Run();
        }

        private static void UseStaticFile(WebApplication app)
        {
            var path = app.Configuration.ResourceRoot().Split('/').Last();
            var local = Path.Combine(app.Environment.ContentRootPath, path);
            if (!Directory.Exists(local))
            {
                Directory.CreateDirectory(local);
            }
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(local),
                RequestPath = "/" + path
            });
        }

        private static void AddControllersAndCustomFormater(WebApplicationBuilder builder)
        {
            // Add services to the container.
            builder.Services.AddControllers(options =>
            {
                SystemTextJsonOutputFormatter s = (SystemTextJsonOutputFormatter)options.OutputFormatters.FirstOrDefault(a => a.GetType() == typeof(SystemTextJsonOutputFormatter));
                var index = options.OutputFormatters.IndexOf(s);
                options.OutputFormatters[index] = new CustomSystemTextJsonOutputFormatter(s.SerializerOptions);
            });
        }

        private static void AddSwaggerGen(WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "access_token",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });
        }

        private static void ConfigureJWT(WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = builder.Configuration.JWTAudience(),
                        ValidIssuer = builder.Configuration.JWTIssuer(),
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.JWTSecret()))
                    };


                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = (context) =>
                        {
                            StringValues values;

                            if (!context.Request.Query.TryGetValue("token", out values))
                            {
                                return Task.CompletedTask;
                            }

                            if (values.Count > 1)
                            {
                                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                                context.Fail(
                                    "Only one 'access_token' query string parameter can be defined. " +
                                    $"However, {values.Count:N0} were included in the request."
                                );

                                return Task.CompletedTask;
                            }

                            var token = values.Single();

                            if (String.IsNullOrWhiteSpace(token))
                            {
                                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                                context.Fail(
                                    "The 'access_token' query string parameter was defined, " +
                                    "but a value to represent the token was not included."
                                );

                                return Task.CompletedTask;
                            }

                            context.Token = token;

                            return Task.CompletedTask;
                        }
                    };

                });
        }
    }
}