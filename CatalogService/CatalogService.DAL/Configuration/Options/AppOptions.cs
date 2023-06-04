namespace CatalogService.DAL.Configuration.Options
{
    public class AppOptions {
        public string? DbConnectionString { get; set; }
        public string? AzureConnectionString { get; set; }
        public string? AzureQueueName { get; set; }
    }
}