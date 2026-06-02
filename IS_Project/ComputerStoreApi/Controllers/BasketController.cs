using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ComputerStore.BLL.DTOs;
using ComputerStore.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ComputerStore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpPost("calculate-discount")]
        public async Task<IActionResult> CalculateDiscount([FromBody] BasketRequest request)
        {
            var result = await _basketService.CalculateDiscountAsync(request);

            if (!result.IsSuccess)
            {
                return result.StatusCode switch
                {
                    404 => NotFound(result.Error),
                    _ => BadRequest(result.Error)
                };
            }

            return Ok(result.Data);
        }
    }
}