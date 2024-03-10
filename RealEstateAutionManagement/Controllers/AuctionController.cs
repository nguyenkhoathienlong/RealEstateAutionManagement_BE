using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstateAuctionManagement.Claims;
using Service.Core;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        [Authorize]
        [HttpGet("own")]
        public async Task<IActionResult> GetOwnAuctions([FromQuery] AuctionQueryModel query)
        {
            var userId = User.Claims.GetUserIdFromJwtToken();
            var result = await _auctionService.GetOwnAuctions(query, userId);
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

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] AuctionUpdateModel model)
        {
            var userId = User.Claims.GetUserIdFromJwtToken();
            var result = await _auctionService.Update(id, model, userId);
            return Ok(result);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = User.Claims.GetUserIdFromJwtToken();
            var result = await _auctionService.Delete(id, userId);
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

        [Authorize]
        [HttpPost("{id}/register")]
        public async Task<IActionResult> RegisterForAuction(Guid id)
        {
            var userId = User.Claims.GetUserIdFromJwtToken();
            var result = await _auctionService.RegisterForAuction(id, userId);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("callback")]
        public async Task<IActionResult> PaymentCallback()
        {
            var result = await _auctionService.PaymentCallback(Request.Query);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("{id}/place-bid")]
        public async Task<IActionResult> PlaceBid(Guid id, [FromBody] PlaceBidModel model)
        {
            var userId = User.Claims.GetUserIdFromJwtToken();
            var auctionId = await _auctionService.PlaceBid(id, model, userId);
            return Ok(auctionId);
        }

        [Authorize(Roles = "Staff, Admin")]
        [HttpPatch("{id}/open")]
        public async Task<IActionResult> OpenAuction(Guid id)
        {
            var result = await _auctionService.OpenAuction(id);
            return Ok(result);
        }

        [Authorize(Roles = "Staff, Admin")]
        [HttpPatch("{id}/close")]
        public async Task<IActionResult> CloseAuction(Guid id)
        {
            var result = await _auctionService.CloseAuction(id);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("{id}/pay")]
        public async Task<IActionResult> PayForAuction(Guid id)
        {
            var userId = User.Claims.GetUserIdFromJwtToken();
            var result = await _auctionService.PayForAuction(id, userId);
            return Ok(result);
        }
    }
}
