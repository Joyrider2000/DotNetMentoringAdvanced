using CartingService.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CartingServiceWebApp.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Authorize]
    [Route("api/v2/cart")]
    public class CartV2Controller : CartV1Controller
    {
        public CartV2Controller(ICartService service, LinkGenerator linkGenerator) : base(service, linkGenerator)
        {
        }

        [HttpGet("{cartId}")]
        [Produces("application/json")]
        public override IActionResult GetCart([FromRoute] string cartId)
        {
            return Ok(_service.GetCart(cartId).Items);
        }

    }
}
