using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Service.Core;

namespace RealEstateAuctionManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RealEstateImageController : ControllerBase
    {
        private readonly IRealEstateImageService _realEstateImageService;

        public RealEstateImageController(IRealEstateImageService realEstateImageService)
        {
            _realEstateImageService = realEstateImageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRealEstateImage([FromQuery] RealEstateImageQueryModel query)
        {
            var result = await _realEstateImageService.GetAll(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _realEstateImageService.GetById(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RealEstateImageCreateModel model)
        {
            var result = await _realEstateImageService.Create(model);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] RealEstateImageUpdateModel model)
        {
            var result = await _realEstateImageService.Update(id, model);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _realEstateImageService.Delete(id);
            return Ok(result);
        }
    }
}
