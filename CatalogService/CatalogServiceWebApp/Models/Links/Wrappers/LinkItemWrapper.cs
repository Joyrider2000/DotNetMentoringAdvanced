using CatalogServiceWebApp.Models.Pagination;

namespace CatalogServiceWebApp.Models.Links.Wrappers
{
    public class LinkItemWrapper<T> : BaseLinkWrapper
    {
        public T? Value { get; set; }

        public LinkItemWrapper(T? value)
        {
            Value = value;
        }
    }
}
