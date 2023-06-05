using CartingService.BLL.Services;
using CartingServiceWebApp.Models.Links.Wrappers;
using CartingServiceWebApp.Models.Links;
using Microsoft.AspNetCore.Mvc;
using CartingService.BLL.Entities;
using System.ComponentModel.DataAnnotations;
using CartingService.DAL.Exceptions;
using Microsoft.Identity.Web.Resource;
using Microsoft.AspNetCore.Authorization;

namespace CartingServiceWebApp.Controllers
{
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:ApiScope")]
    [Produces("application/json")]
    [ApiController]
    [Authorize]
    [Route("api/v1/cart")]
    public class CartV1Controller : Controller
    {
        private protected readonly ICartService _service;

        private readonly LinkGenerator _linkGenerator;

        public CartV1Controller(ICartService service,
            LinkGenerator linkGenerator)
        {
            _service = service;
            _linkGenerator = linkGenerator;
        }
        
        [HttpGet]
        [Produces("application/json")]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAllCarts());
        }

        [HttpGet("{cartId}")]
        [Produces("application/json")]
        public virtual IActionResult GetCart([FromRoute] string cartId)
        {
            return Ok(_service.GetCart(cartId));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult Create([FromBody] CartEntity cart)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdItem = _service.AddCart(cart);
            var itemWrapper = new LinkItemWrapper<CartEntity>(createdItem);
            if (createdItem != null)
            {
                itemWrapper.Links.Add(new Link(_linkGenerator.GetUriByAction(HttpContext) + $"/{createdItem.Id}", "updateItem", "PUT"));
                itemWrapper.Links.Add(new Link(_linkGenerator.GetUriByAction(HttpContext) + $"/{createdItem.Id}", "deleteItem", "DELETE"));
            }

            return CreatedAtAction(nameof(Create), new { id = createdItem?.Id }, itemWrapper);
        }
        
        [HttpPost("{cartId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult AddCartItem([FromRoute] string cartId, [FromBody] CartItemEntity cartItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _service.AddCartItem(cartId, cartItem);
                return Ok();
            }
            catch (ItemExistsException ex)
            {
                return Conflict(ex.Message);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{cartId}/{itemId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Delete([FromRoute] string cartId, [FromRoute] int itemId)
        {
            try
            {
                _service.DeleteCartItem(cartId, itemId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}