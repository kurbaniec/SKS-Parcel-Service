/*
 * Parcel Logistics Service
 *
 * No description provided (generated by Swagger Codegen https://github.com/swagger-api/swagger-codegen)
 *
 * OpenAPI spec version: 1.20.0
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using NetTopologySuite.IO.Converters;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using UrbaniecZelenay.SKS.Package.BusinessLogic;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Interfaces;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Mappings;
using UrbaniecZelenay.SKS.Package.DataAccess.Interfaces;
using UrbaniecZelenay.SKS.Package.DataAccess.Sql;
using UrbaniecZelenay.SKS.Package.Services.Filters;
using UrbaniecZelenay.SKS.ServiceAgents;
using UrbaniecZelenay.SKS.ServiceAgents.Interfaces;
using UrbaniecZelenay.SKS.WebhookManager;
using UrbaniecZelenay.SKS.WebhookManager.Interfaces;


namespace UrbaniecZelenay.SKS.Package.Services
{
    /// <summary>
    /// Startup
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        private readonly IWebHostEnvironment _hostingEnv;

        private IConfiguration Configuration { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="env"></param>
        /// <param name="configuration"></param>
        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            _hostingEnv = env;
            Configuration = configuration;
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Profiles from other projects need to be added manually
            services.AddAutoMapper(typeof(Startup), typeof(MappingsProfileBlDal));

            // Add framework services.
            services
                .AddMvc(options =>
                {
                    options.InputFormatters
                        .RemoveType<Microsoft.AspNetCore.Mvc.Formatters.SystemTextJsonInputFormatter>();
                    options.OutputFormatters
                        .RemoveType<Microsoft.AspNetCore.Mvc.Formatters.SystemTextJsonOutputFormatter>();
                }).AddFluentValidation(mvcConfiguration =>
                    mvcConfiguration.RegisterValidatorsFromAssemblyContaining<Startup>())
                .AddNewtonsoftJson(opts =>
                {
                    opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    opts.SerializerSettings.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy()));
                    opts.SerializerSettings.Converters.Add(new GeometryConverter());
                })
                .AddXmlSerializerFormatters();

            services
                .AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("1.20.1", new OpenApiInfo
                    {
                        Version = "1.20.1",
                        Title = "Parcel Logistics Service",
                        Description = "Parcel Logistics Service (ASP.NET Core 3.1)",
                        Contact = new OpenApiContact()
                        {
                            Name = "SKS",
                            Url = new Uri("http://www.technikum-wien.at/"),
                            Email = ""
                        },
                        // Results in "System.UriFormatException: Invalid URI: The URI is empty" error message.
                        // Remove the following line.
                        // See: https://stackoverflow.com/a/68535121
                        // TermsOfService = new Uri("")
                    });
                    c.CustomSchemaIds(type => type.FullName);
                    c.IncludeXmlComments(
                        $"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}{_hostingEnv.ApplicationName}.xml");

                    // Include DataAnnotation attributes on Controller Action parameters as Swagger validation rules (e.g required, pattern, ..)
                    // Use [ValidateModelState] on Actions to actually validate it in C# as well!
                    c.OperationFilter<GeneratePathParamsValidationFilter>();
                });

            // Configure database & Dependency Injection
            // See: https://codewithmukesh.com/blog/repository-pattern-in-aspnet-core/
            // And: https://stackoverflow.com/a/60399887/12347616
            services.AddDbContext<ParcelLogisticsContext>(options =>
                // Add Spatial Data support
                // See: https://docs.microsoft.com/en-us/ef/core/modeling/spatial
                options.UseSqlServer(Configuration.GetConnectionString("ParcelLogisticsContext"), dbOpt => 
                    dbOpt.UseNetTopologySuite().CommandTimeout(300)));
            // Register the service and implementation for the database context
            // See: https://www.jerriepelser.com/blog/resolve-dbcontext-as-interface-in-aspnet5-ioc-container/
            services.AddScoped<IParcelLogisticsContext>(provider => provider.GetService<ParcelLogisticsContext>()!);
            services.AddScoped<IWarehouseRepository, WarehouseRepository>();
            services.AddScoped<IParcelRepository, ParcelRepository>();
            services.AddScoped<IGeoEncodingAgent, OpenStreetMapEncodingAgent>();
            services.AddScoped<ITransferWarehouseAgent, RestTransferWarehouseAgent>();
            services.AddTransient<IWarehouseManagementLogic, WarehouseManagementLogic>();
            services.AddTransient<IStaffLogic, StaffLogic>();
            services.AddTransient<ISenderLogic, SenderLogic>();
            services.AddTransient<IRecipientLogic, RecipientLogic>();
            services.AddTransient<ILogisticsPartnerLogic, LogisticsPartnerLogic>();
            services.AddTransient<IParcelWebhookLogic, ParcelWebhookLogic>();
            services.AddScoped<IWebhookManager, WebhookManager.WebhookManager>();
            services.AddScoped<IWebhookRepository, WebhookRepository>();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="context"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory,
            ParcelLogisticsContext context)
        {
            // Create Database if not already exists
            context.Database.EnsureCreated();

            app.UseRouting();

            //TODO: Uncomment this if you need wwwroot folder
            // app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                //TODO: Either use the SwaggerGen generated Swagger contract (generated from C# classes)
                c.SwaggerEndpoint("/swagger/1.20.1/swagger.json", "Parcel Logistics Service");

                //TODO: Or alternatively use the original Swagger contract that's included in the static files
                // c.SwaggerEndpoint("/swagger-original.json", "Parcel Logistics Service Original");
            });

            //TODO: Use Https Redirection
            // app.UseHttpsRedirection();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //TODO: Enable production exception handling (https://docs.microsoft.com/en-us/aspnet/core/fundamentals/error-handling)
                app.UseExceptionHandler("/Error");

                app.UseHsts();
            }
            
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "../UrbaniecZelenay.SKS.WebApp";
                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}