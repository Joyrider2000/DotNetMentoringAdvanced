using AutoMapper;
using CartingService.BLL.Services;
using CartingService.CartingService.BLL.Mappers.Builders;
using CartingService.CartingService.BLL.Services;
using CartingService.DAL.Controller;
using CartingService.DAL.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
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
            string? dbConnectionString = Configuration.GetSection("Database").GetValue<string>("ConnectionString");
            string? azureConnectionString = Configuration.GetSection("Azure").GetValue<string>("ConnectionString");
            string? azureQueueName = Configuration.GetSection("Azure").GetValue<string>("ProductQueueName");

            ICartRepository cartRepository = new CartRepository(dbConnectionString);
            ICartService cartService = new CartService(cartRepository);
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

            services.AddTransient(service => cartService);
            services.AddTransient(mapper => productMapper);
            services.AddHostedService<ProductConsumerService>();
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddSingleton<IQueueClient>(x => new QueueClient(azureConnectionString, azureQueueName));

        }
    }
}
