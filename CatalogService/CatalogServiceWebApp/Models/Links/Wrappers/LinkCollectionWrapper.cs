using CatalogServiceWebApp.Models.Pagination;

namespace CatalogServiceWebApp.Models.Links.Wrappers
{
    public class LinkCollectionWrapper<T> : BaseLinkWrapper
    {
        private readonly LinkGenerator _linkGenerator;
        private readonly HttpContext _context;

        public IEnumerable<T> Value { get; set; }

        public LinkCollectionWrapper(IEnumerable<T> value,
            HttpContext context,
            LinkGenerator linkGenerator)
        {
            Value = value;
            _linkGenerator = linkGenerator;
            _context = context;
        }

        public void GeneratePaginationLinks(IEnumerable<KeyValuePair<string, object>> parameters, PaginationParameters paginationParameters)
        {
            string? hrefPrev = null;
            string? hrefNext = null;
            if (paginationParameters.PageNumber > 0)
            {
                hrefPrev = _linkGenerator.GetUriByAction(_context, values:
                    parameters.Append(new KeyValuePair<string, object>("pageNumber", paginationParameters.PageNumber - 1))
                              .Append(new KeyValuePair<string, object>("pageSize", paginationParameters.PageSize))
                );
            }
            if (Value.Count() == paginationParameters.PageSize)
            {
                hrefNext = _linkGenerator.GetUriByAction(_context, values:
                    parameters.Append(new KeyValuePair<string, object>("pageNumber", paginationParameters.PageNumber + 1))
                              .Append(new KeyValuePair<string, object>("pageSize", paginationParameters.PageSize))
                );
            }
            Links.Add(new Link(hrefPrev, "prevPage", "GET"));
            Links.Add(new Link(hrefNext, "nextPage", "GET"));
        }
    }
}
