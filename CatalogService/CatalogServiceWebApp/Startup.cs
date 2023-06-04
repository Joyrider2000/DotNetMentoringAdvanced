using CatalogService.BLL.Application.Services.Message;
using CatalogService.BLL.Application.Services;
using CatalogService.BLL.Domain.Entities;
using CatalogService.DAL.Messages.Services;
using CatalogService.DAL.SQLiteDb.CategoryRepository;
using CatalogService.DAL.SQLiteDb.ProductRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Logging.ApplicationInsights;
using CatalogService.DAL.Configuration.Options;
using CatalogService.BLL.Application.Repositories;

namespace CatalogServiceWebApp {
    public class Startup {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services) {

            var appOptions = new AppOptions() {
                DbConnectionString = Configuration["Database:ConnectionString"],
                AzureConnectionString = Configuration["Azure:ConnectionString"],
                AzureQueueName = Configuration["Azure:ProductQueueName"]
            };

            services.AddLogging(builder =>
            {
                // Only Application Insights is registered as a logger provider
                builder.AddApplicationInsights(
                    configureTelemetryConfiguration: (config) => config.ConnectionString = Configuration["ApplicationInsightsConnectionString"],
                    configureApplicationInsightsLoggerOptions: (options) => { }
                );
                builder.AddFilter<ApplicationInsightsLoggerProvider>("CatalogService", Microsoft.Extensions.Logging.LogLevel.Trace);
            });

            services.AddApplicationInsightsTelemetry();

            services.AddMicrosoftIdentityWebApiAuthentication(Configuration);
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSingleton(appOptions);
            services.AddSingleton<IMessageService, AzureMessageService>();
            services.AddSingleton<IProductRepository, ProductRepository>();
            services.AddSingleton<IBaseRepository<CategoryEntity>, CategoryRepository>();
            services.AddSingleton<IProductService, ProductService>();
            services.AddSingleton<IBaseService<CategoryEntity>, BaseService<CategoryEntity>>();
            services.Configure<ApiBehaviorOptions>(options => {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddSingleton<IQueueClient>(x => new QueueClient(appOptions.AzureConnectionString, appOptions.AzureConnectionString));

            // Sign-in users with the Microsoft identity platform
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddMicrosoftIdentityWebApp(Configuration.GetSection("AzureAd"));

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "swaggerAADdemo", Version = "v1" });
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme {
                    Description = "OAuth2.0 Auth Code with PKCE",
                    Name = "oauth2",
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows {
                        AuthorizationCode = new OpenApiOAuthFlow {
                            AuthorizationUrl = new Uri(Configuration["SwaaggerAzureAD:AuthorizationUrl"]),
                            TokenUrl = new Uri(Configuration["SwaaggerAzureAD:TokenUrl"]),
                            Scopes = new Dictionary<string, string>
                            {
                                { Configuration["SwaaggerAzureAD:ApiScope"], "API access" }
                            }
                        }
                    }
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
                        },
                        new[] { Configuration["SwaaggerAzureAD:Scope"] }
                    }
                });
            });
        }
    }
}
