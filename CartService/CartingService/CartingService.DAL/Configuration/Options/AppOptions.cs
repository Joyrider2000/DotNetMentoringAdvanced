using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartingService.DAL.Configuration.Options {
    public class AppOptions {
        public string? DbConnectionString { get; set; }
        public string? AzureConnectionString { get; set; }
        public string? AzureQueueName { get; set; }
    }
}
