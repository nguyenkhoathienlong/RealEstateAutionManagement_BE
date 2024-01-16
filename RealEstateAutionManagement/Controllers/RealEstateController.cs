using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Service.Core;

namespace RealEstateAuctionManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RealEstateController : ControllerBase
    {
        private readonly IRealEstateService _realEstateService;

        public RealEstateController(IRealEstateService realEstateService)
        {
            _realEstateService = realEstateService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRealEstate([FromQuery] RealEstateQueryModel query)
        {
            var result = await _realEstateService.GetAll(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _realEstateService.GetById(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RealEstateCreateModel model)
        {
            var result = await _realEstateService.Create(model);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] RealEstateUpdateModel model)
        {
            var result = await _realEstateService.Update(id, model);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _realEstateService.Delete(id);
            return Ok(result);
        }
    }
}
