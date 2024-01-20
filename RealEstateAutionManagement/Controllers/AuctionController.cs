using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstateAuctionManagement.Claims;
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
            var userRole = User.Claims.GetUserRoleFromJwtToken();
            var result = await _auctionService.GetAll(query, userRole);
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

        [Authorize]
        [HttpPost("create-request")]
        public async Task<IActionResult> CreateAuctionRequest(AuctionCreateRequestModel model)
        {
            var userId = User.Claims.GetUserIdFromJwtToken();
            var result = await _auctionService.CreateAuctionRequest(model, userId);
            return Ok(result);
        }

        [Authorize(Roles = "Staff, Admin")]
        [HttpPut("{id}/approve")]
        public async Task<IActionResult> ApproveAuction(Guid id, [FromBody] ApproveAuctionModel model)
        {
            var userId = User.Claims.GetUserIdFromJwtToken();
            var result = await _auctionService.ApproveAuction(id, model, userId);
            return Ok(result);
        }
    }
}
