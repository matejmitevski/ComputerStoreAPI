using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ComputerStore.BLL.DTOs;
using ComputerStore.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ComputerStore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockController : ControllerBase
    {
        private readonly IStockImportService _stockImportService;

        public StockController(IStockImportService stockImportService)
        {
            _stockImportService = stockImportService;
        }

        [HttpPost("import")]
        public async Task<IActionResult> Import([FromBody] List<StockImportRequest> requests)
        {
            var result = await _stockImportService.ImportAsync(requests);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok(result.Data);
        }
    }
}