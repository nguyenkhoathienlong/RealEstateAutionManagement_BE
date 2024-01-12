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
        public int Gender { get; set; }
        public string? Address { get; set; }
        public string? IdentityNumber { get; set; }
        public DateTime IdentityCardProvideDate { get; set; }
        public string? IdentityCardFrontImage { get; set; }
        public string? IdentityCardBackImage { get; set; }
        public string? Avatar { get; set; }
        public int Role { get; set; }
    }

    public class UserCreateModel : BaseModel
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public int Gender { get; set; }
        public string? Address { get; set; }
        public string? IdentityNumber { get; set; }
        public DateTime IdentityCardProvideDate { get; set; }
        public string? IdentityCardFrontImage { get; set; }
        public string? IdentityCardBackImage { get; set; }
        public string? Avatar { get; set; }
        public int Role { get; set; }
    }

    public class UserUpdateModel : BaseModel
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public int Gender { get; set; }
        public string? Address { get; set; }
        public string? IdentityNumber { get; set; }
        public DateTime IdentityCardProvideDate { get; set; }
        public string? IdentityCardFrontImage { get; set; }
        public string? IdentityCardBackImage { get; set; }
        public string? Avatar { get; set; }
        public int Role { get; set; }
        [JsonIgnore]
        public DateTime DateUpdate { get; set; } = DateTime.UtcNow;
    }
}
