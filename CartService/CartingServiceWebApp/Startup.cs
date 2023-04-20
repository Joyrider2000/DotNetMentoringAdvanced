using AutoMapper;
using CartingService.BLL.Services;
using CartingService.CartingService.BLL.Mappers.Builders;
using CartingService.CartingService.BLL.Services;
using CartingService.DAL.Controller;
using CartingService.DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;

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

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
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
