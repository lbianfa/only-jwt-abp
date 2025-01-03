using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using OnlyJWT.EntityFrameworkCore;
using OnlyJWT.MultiTenancy;
using Microsoft.OpenApi.Models;
using Volo.Abp;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;
using Volo.Abp.VirtualFileSystem;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace OnlyJWT;

[DependsOn(
    typeof(OnlyJWTHttpApiModule),
    typeof(AbpAutofacModule),
    typeof(AbpAspNetCoreMultiTenancyModule),
    typeof(OnlyJWTApplicationModule),
    typeof(OnlyJWTEntityFrameworkCoreModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpSwashbuckleModule)
)]
public class OnlyJWTHttpApiHostModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        ConfigureAuthentication(context, configuration);
        ConfigureConventionalControllers();
        ConfigureVirtualFileSystem(context);
        ConfigureCors(context, configuration);
        ConfigureSwaggerServices(context, configuration);

        Configure<MvcOptions>(options =>
        {
            options.Conventions.Add(new ControllerExclusions());
        });
    }

    private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(options =>
              {
                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuer = true,
                      ValidateAudience = true,
                      ValidateLifetime = true,
                      ValidateIssuerSigningKey = true,
                      ValidIssuer = configuration["JwtBearer:Issuer"],
                      ValidAudience = configuration["JwtBearer:Audience"],
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtBearer:SecurityKey"]))
                  };
              });

        context.Services.AddAuthorization();
    }

    private void ConfigureVirtualFileSystem(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        if (hostingEnvironment.IsDevelopment())
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.ReplaceEmbeddedByPhysical<OnlyJWTDomainSharedModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}OnlyJWT.Domain.Shared"));
                options.FileSets.ReplaceEmbeddedByPhysical<OnlyJWTDomainModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}OnlyJWT.Domain"));
                options.FileSets.ReplaceEmbeddedByPhysical<OnlyJWTApplicationContractsModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}OnlyJWT.Application.Contracts"));
                options.FileSets.ReplaceEmbeddedByPhysical<OnlyJWTApplicationModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}OnlyJWT.Application"));
            });
        }
    }

    private void ConfigureConventionalControllers()
    {
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers.Create(typeof(OnlyJWTApplicationModule).Assembly);
        });
    }

    private static void ConfigureSwaggerServices(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddAbpSwaggerGen(
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "OnlyJWT API", Version = "v1" });

                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Description = "Ingrese 'Bearer' seguido del token JWT en el campo de autorización. Ejemplo: 'Bearer {token}'",
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                };

                options.AddSecurityDefinition("Bearer", jwtSecurityScheme);

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        jwtSecurityScheme, new[] { "Bearer" }
                    }
                });

                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
            });
    }

    private void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                    .WithOrigins(configuration["App:CorsOrigins"]?
                        .Split(",", StringSplitOptions.RemoveEmptyEntries)
                        .Select(o => o.RemovePostFix("/"))
                        .ToArray() ?? Array.Empty<string>())
                    .WithAbpExposedHeaders()
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseAbpRequestLocalization();

        app.UseCorrelationId();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors();
        app.UseJwtTokenMiddleware();
        app.UseAuthentication();

        if (MultiTenancyConsts.IsEnabled)
        {
            app.UseMultiTenancy();
        }
        app.UseUnitOfWork();
        app.UseDynamicClaims();
        app.UseAuthorization();

        app.UseSwagger();
        app.UseAbpSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "OnlyJWT API");

            c.IndexStream = () =>
            {
                var pathToCustomIndex = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/swagger/ui/index.html");
                return new FileStream(pathToCustomIndex, FileMode.Open, FileAccess.Read);
            };
        });

        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();
    }
}
