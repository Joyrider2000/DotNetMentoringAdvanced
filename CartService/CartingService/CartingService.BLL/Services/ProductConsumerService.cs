﻿using AutoMapper;
using CartingService.BLL.Entities;
using CartingService.BLL.Mappers.Builders;
using CartingService.BLL.Services;
using CartingService.CartingService.BLL.Entities.External.CatalogService;
using CartingService.DAL.Entities;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace CartingService.CartingService.BLL.Services
{
    public class ProductConsumerService : BackgroundService
    {
        private readonly IQueueClient _queueClient;
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;

        public ProductConsumerService(IQueueClient queueClient, ICartService cartService, IMapper mapper)
        {
            _queueClient = queueClient;
            _cartService = cartService;
            _mapper = mapper;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _queueClient.RegisterMessageHandler((message, token) =>
            {
                ProductEntity? product = JsonSerializer.Deserialize<ProductEntity>(message.Body);
                _cartService.UpdateProducts(_mapper.Map<CartItemEntity>(product));
                return Task.CompletedTask;
            }, new MessageHandlerOptions(args => Task.CompletedTask)
            {
                AutoComplete = true,
                MaxConcurrentCalls = 1
            });
            return Task.CompletedTask;
        }
    }
}
        