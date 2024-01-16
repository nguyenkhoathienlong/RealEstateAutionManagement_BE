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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateAuctionRequest(AuctionCreateModel model)
        {
            var userId = User.Claims.GetUserIdFromJwtToken();
            var result = await _auctionService.CreateAuctionRequest(model, userId);
            return Ok(result);
        }
    }
}
