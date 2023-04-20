using CatalogService.BLL.Application.Services.Message;
using CatalogService.BLL.Application.Services;
using CatalogService.BLL.Domain.Entities;
using CatalogService.DAL.Messages.Services;
using CatalogService.DAL.SQLiteDb.CategoryRepository;
using CatalogService.DAL.SQLiteDb.ProductRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;

namespace CatalogServiceWebApp
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

            IMessageService azureMessageSerivce = new AzureMessageService(azureConnectionString, azureQueueName);
            IProductService productService = new ProductService(new ProductRepository(dbConnectionString), azureMessageSerivce);
            IBaseService<CategoryEntity> categoryService = new BaseService<CategoryEntity>(new CategoryRepository(dbConnectionString));

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddTransient(service => productService);
            services.AddTransient(service => categoryService);
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddSingleton<IQueueClient>(x => new QueueClient(azureConnectionString, azureQueueName));

        }
    }
}
