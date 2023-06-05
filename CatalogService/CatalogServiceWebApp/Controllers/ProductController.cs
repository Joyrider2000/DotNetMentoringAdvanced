using CatalogService.BLL.Application.Services;
using CatalogService.BLL.Domain.Entities;
using CatalogServiceWebApp.Models.Links.Wrappers;
using CatalogServiceWebApp.Models.Pagination;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CatalogServiceWebApp.Controllers
{
    [Route("api/product")]
    public class ProductController : BaseController<ProductEntity>
    {
        private new readonly IProductService _service;

        public ProductController(IProductService service,
            ILogger<ProductController> logger,
            LinkGenerator linkGenerator) : base(service, logger, linkGenerator)
        {
            _service = service;
        }

        protected override void UpdateModelState()
        {
            ModelState.Remove("Category.Name");
        }

        [HttpGet]
        [Produces("application/json")]
        public async Task<IActionResult> GetByCategory([FromQuery, BindRequired] int categoryId, [FromQuery] PaginationParameters paginationParameters)
        {
            var products = await _service.GetByCategoryId(categoryId, paginationParameters.PageNumber, paginationParameters.PageSize);
            var productsWrapper = new LinkCollectionWrapper<ProductEntity>(products, HttpContext, _linkGenerator);
            productsWrapper.GeneratePaginationLinks(new List<KeyValuePair<string, object>>
                {
                    new KeyValuePair<string, object>("categoryId", categoryId)
                },
                paginationParameters
            );

            return Ok(productsWrapper);
        }
    }
}
