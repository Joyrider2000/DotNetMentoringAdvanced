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

namespace CatalogServiceWebApp {
    public class Startup {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services) {
            string? dbConnectionString = Configuration.GetSection("Database").GetValue<string>("ConnectionString");
            string? azureConnectionString = Configuration.GetSection("Azure").GetValue<string>("ConnectionString");
            string? azureQueueName = Configuration.GetSection("Azure").GetValue<string>("ProductQueueName");

            IMessageService azureMessageSerivce = new AzureMessageService(azureConnectionString, azureQueueName);
            IProductService productService = new ProductService(new ProductRepository(dbConnectionString), azureMessageSerivce);
            IBaseService<CategoryEntity> categoryService = new BaseService<CategoryEntity>(new CategoryRepository(dbConnectionString));
            var str = Configuration["AuthorizationUrl"];
            Console.WriteLine(str);

            services.AddMicrosoftIdentityWebApiAuthentication(Configuration);
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddTransient(service => productService);
            services.AddTransient(service => categoryService);
            services.Configure<ApiBehaviorOptions>(options => {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddSingleton<IQueueClient>(x => new QueueClient(azureConnectionString, azureQueueName));

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
