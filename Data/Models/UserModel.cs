using Data.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Data.Models
{
    public class UserModel : BaseModel
    {
        
    }

    public class UserViewModel : BaseModel
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public Gender Gender { get; set; }
        public string? Address { get; set; }
        public string? IdentityNumber { get; set; }
        public DateTime IdentityCardProvideDate { get; set; }
        public string? IdentityCardFrontImage { get; set; }
        public string? IdentityCardBackImage { get; set; }
        public string? Avatar { get; set; }
        public Role Role { get; set; }
        public UserStatus Status { get; set; }
    }

    public class UserCreateModel
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public Gender Gender { get; set; }
        public string? Address { get; set; }
        public string? IdentityNumber { get; set; }
        public DateTime IdentityCardProvideDate { get; set; }
        public string? IdentityCardFrontImage { get; set; }
        public string? IdentityCardBackImage { get; set; }
        public string? Avatar { get; set; }
        public Role Role { get; set; }
        public UserStatus Status { get; set; }
    }

    public class UserUpdateModel
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public Gender Gender { get; set; }
        public string? Address { get; set; }
        public string? IdentityNumber { get; set; }
        public DateTime IdentityCardProvideDate { get; set; }
        public string? IdentityCardFrontImage { get; set; }
        public string? IdentityCardBackImage { get; set; }
        public string? Avatar { get; set; }
        public Role Role { get; set; }
        public UserStatus Status { get; set; }
        [JsonIgnore]
        public DateTime DateUpdate { get; set; } = DateTime.UtcNow;
    }

    public class UserQueryModel : QueryStringParameters
    {
        public UserQueryModel()
        {
            OrderBy = "Name";
            OrderBy = "DateCreate";
        }
        public string? Search { get; set; }
    }

    public class UserRequest
    {
        [Required(ErrorMessage = "User Name is required")]
        public string? Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }

    public class UserRegisterModel
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public Gender Gender { get; set; }
        public string? Address { get; set; }
        public string? IdentityNumber { get; set; }
        public DateTime IdentityCardProvideDate { get; set; }
        public string? IdentityCardFrontImage { get; set; }
        public string? IdentityCardBackImage { get; set; }
        public string? Avatar { get; set; }
        [JsonIgnore]
        public Role Role { get; set; }
    }
}
