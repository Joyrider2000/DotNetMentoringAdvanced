using AutoMapper;
using CartingService.BLL.Services;
using CartingService.CartingService.BLL.Mappers.Builders;
using CartingService.CartingService.BLL.Services;
using CartingService.DAL.Configuration.Options;
using CartingService.DAL.Controller;
using CartingService.DAL.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;

namespace CartingServiceWebApp
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var appOptions = new AppOptions() {
                DbConnectionString = Configuration["Database:ConnectionString"],
                AzureConnectionString = Configuration["Azure:ConnectionString"],
                AzureQueueName = Configuration["Azure:ProductQueueName"]
            };

            services.AddLogging(builder => {
                // Only Application Insights is registered as a logger provider
                builder.AddApplicationInsights(
                    configureTelemetryConfiguration: (config) => config.ConnectionString = Configuration["ApplicationInsightsConnectionString"],
                    configureApplicationInsightsLoggerOptions: (options) => { }
                );
                builder.AddFilter<ApplicationInsightsLoggerProvider>("CartingService", Microsoft.Extensions.Logging.LogLevel.Trace);
            });

            IMapper productMapper = ProductMapperBuilder.Build();

            services.AddMicrosoftIdentityWebApiAuthentication(Configuration);
            services.AddControllers();
            services.AddEndpointsApiExplorer();

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

            services.AddApplicationInsightsTelemetry();
            services.AddSingleton(appOptions);
            services.AddSingleton<ICartService, CartService>();
            services.AddSingleton<ICartRepository, CartRepository>();
            services.AddSingleton(productMapper);
            services.AddHostedService<ProductConsumerService>();
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddSingleton<IQueueClient>(x => new QueueClient(appOptions.AzureConnectionString, appOptions.AzureQueueName));

        }
    }
}
