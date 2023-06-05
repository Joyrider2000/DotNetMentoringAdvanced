using CatalogService.BLL.Application.Services;
using CatalogService.BLL.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CatalogServiceWebApp.Controllers
{
    [Route("api/category")]
    public class CategoryController : BaseController<CategoryEntity>
    {
        public CategoryController(IBaseService<CategoryEntity> service,
            ILogger<BaseController<CategoryEntity>> logger,
            LinkGenerator linkGenerator) : base(service, logger, linkGenerator)
        {
        }

        protected override void UpdateModelState()
        {
            ModelState.Remove("Parent.Name");
        }

        [HttpGet]
        [Produces("application/json")]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("GET all called");
            return Ok(await _service.List());
        }
    }
}
