namespace CatalogServiceWebApp.Models.Links.Wrappers
{
    public abstract class BaseLinkWrapper
    {
        public List<Link> Links { get; set; } = new List<Link>();
    }
}
