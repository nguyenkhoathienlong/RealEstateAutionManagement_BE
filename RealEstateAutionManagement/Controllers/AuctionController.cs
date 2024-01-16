using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Service.Core;

namespace RealEstateAuctionManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionController : ControllerBase
    {
        private readonly IAuctionService _auctionService;

        public AuctionController(IAuctionService auctionService)
        {
            _auctionService = auctionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAuction([FromQuery] AuctionQueryModel query)
        {
            var result = await _auctionService.GetAll(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _auctionService.GetById(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AuctionCreateModel model)
        {
            var result = await _auctionService.Create(model);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] AuctionUpdateModel model)
        {
            var result = await _auctionService.Update(id, model);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _auctionService.Delete(id);
            return Ok(result);
        }
    }
}
