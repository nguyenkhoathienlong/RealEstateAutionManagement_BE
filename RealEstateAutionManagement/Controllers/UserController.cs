using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Core;
using RealEstateAuctionManagement.Claims;

namespace RealEstateAuctionManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserRequest model)
        {
            var data = await _userService.Login(model);
            return Ok(data);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterModel model)
        {
            var data = await _userService.Register(model);
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUser([FromQuery] UserQueryModel query)
        {
            var result = await _userService.GetAll(query);
            return Ok(result);
        }

        [HttpGet("decode")]
        public async Task<IActionResult> DecodeToken()
        {
            var userName = User.Claims.GetUserNameFromJwtToken();
            return Ok(userName);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _userService.GetById(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserCreateModel model)
        {
            var result = await _userService.Create(model);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UserUpdateModel model)
        {
            var result = await _userService.Update(id, model);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _userService.Delete(id);
            return Ok(result);
        }

        [HttpPut("upload-image-profile")]
        public async Task<IActionResult> UpdateProfile(IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                return BadRequest("File is null or empty");
            }
            if (Path.GetExtension(image.FileName) != ".png" && Path.GetExtension(image.FileName) != ".jpg")
            {
                return BadRequest("Only image files are allowed");
            }
            var userId = User.Claims.GetUserIdFromJwtToken();
            var result = await _userService.UpdateProfileImage(userId, image);
            return Ok(("Image upload successfully"));
        }

        [HttpPut("upload-identification-information")]
        public async Task<IActionResult> UpdateIdentificationInformation([FromForm] IdentificationInformation model)
        {
            if (model == null)
            {
                return BadRequest("File is not null or empty");
            }
            if (Path.GetExtension(model?.IdentityCardFrontImage.FileName) != ".png" && Path.GetExtension(model?.IdentityCardFrontImage.FileName) != ".jpg")
            {
                return BadRequest("Identity Card Front only image files are allowed");
            }
            if (Path.GetExtension(model?.IdentityCardBackImage.FileName) != ".png" && Path.GetExtension(model?.IdentityCardBackImage.FileName) != ".jpg")
            {
                return BadRequest("Identity Card Back only image files are allowed");
            }
            var userId = User.Claims.GetUserIdFromJwtToken();
            var result = await _userService.UploadDocument(userId, model);
            return Ok("Image upload successfully");
        }

        //[Authorize(Roles = "Staff, Admin")]
        [HttpPut("{id}/approve")]
        public async Task<IActionResult> ApproveIdentificationDocument(Guid id, [FromBody]ApprovedIdentificationDocument model)
        {
            var userId = User.Claims.GetUserIdFromJwtToken();
            var result = await _userService.ApproveIdentificationDocument(id, model, userId);
            return Ok(result);
        }
    }
}
