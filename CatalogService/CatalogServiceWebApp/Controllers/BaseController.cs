using CatalogService.BLL.Application.Services;
using CatalogService.BLL.Domain.Entities;
using CatalogServiceWebApp.Models.Links;
using CatalogServiceWebApp.Models.Links.Wrappers;
using CatalogServiceWebApp.Models.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using System.Data;

namespace CatalogServiceWebApp.Controllers
{
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:ApiScope")]
    [Produces("application/json")]
//    [Authorize(Roles = $"{Roles.Buyer}, {Roles.Manager}")]
    [ApiController]
    public abstract class BaseController<T> : Controller where T : IdEntity
    {
        protected readonly IBaseService<T> _service;
        protected readonly ILogger<BaseController<T>> _logger;
        protected readonly LinkGenerator _linkGenerator;

        protected BaseController(IBaseService<T> service,
            ILogger<BaseController<T>> logger,
            LinkGenerator linkGenerator)
        {
            _service = service;
            _logger = logger;
            _linkGenerator = linkGenerator;
        }

        protected virtual void UpdateModelState() { }

        [HttpGet("{id}")]
        [Produces("application/json")]
        public async Task<IActionResult> GetItem([FromRoute] int id)
        {
            _logger.LogInformation("GET item called");
            T? item = await _service.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
//        [Authorize(Roles = Roles.Manager)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] T item)
        {
            UpdateModelState();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdItem = await _service.Add(item);
            var itemWrapper = new LinkItemWrapper<T>(createdItem);
            if (createdItem != null) 
            {
                itemWrapper.Links.Add(new Link(_linkGenerator.GetUriByAction(HttpContext) + $"/{createdItem.Id}", "updateItem", "PUT"));
                itemWrapper.Links.Add(new Link(_linkGenerator.GetUriByAction(HttpContext) + $"/{createdItem.Id}", "deleteItem", "DELETE"));
            }

            return CreatedAtAction(nameof(Create), new { id = createdItem?.Id }, itemWrapper);
        }

        [HttpPut("{id}")]
 //       [Authorize(Roles = Roles.Manager)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] T item)
        {
            UpdateModelState();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (item.Id != null && item.Id != id)
            {
                return BadRequest("Item id is inconsistent");
            }

            item.Id = id;
            await _service.Update(item);
            return NoContent();
        }

        [HttpDelete("{id}")]
 //       [Authorize(Roles = Roles.Manager)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _service.Delete(id);
            return NoContent();
        }
    }
}