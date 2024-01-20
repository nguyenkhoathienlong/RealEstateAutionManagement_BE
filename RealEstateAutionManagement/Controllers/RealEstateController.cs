using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateAuctionManagement.Claims;
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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] RealEstateCreateModel model)
        {
            var userId = User.Claims.GetUserIdFromJwtToken();

            // Validate the images
            foreach (var image in model.Images)
            {
                if (image == null || image.Length == 0)
                {
                    return BadRequest("File is null or empty");
                }
                if (Path.GetExtension(image.FileName) != ".png" && Path.GetExtension(image.FileName) != ".jpg")
                {
                    return BadRequest("Only .png and .jpg image files are allowed");
                }
            }

            var result = await _realEstateService.Create(model, userId);
            return Ok(result);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] RealEstateUpdateModel model)
        {
            var userId = User.Claims.GetUserIdFromJwtToken();
            var result = await _realEstateService.Update(id, model, userId);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _realEstateService.Delete(id);
            return Ok(result);
        }

        [Authorize(Roles = "Staff, Admin")]
        [HttpPut("{id}/approve")]
        public async Task<IActionResult> ApproveRealEstate(Guid id, [FromBody] ApproveRealEstateModel model)
        {
            var userId = User.Claims.GetUserIdFromJwtToken();
            var result = await _realEstateService.ApproveRealEstate(id, model, userId);
            return Ok(result);
        }
    }
}
