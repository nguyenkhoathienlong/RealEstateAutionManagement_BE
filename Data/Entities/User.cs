using Data.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class User : BaseEntities
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name can't be longer than 50 characters")]
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        [Phone(ErrorMessage = "Phone is not true to the format")]
        [StringLength(13, ErrorMessage = "Phone number up to 13 characters long")]
        public string PhoneNumber { get; set; } = null!;
        [Required]
        [StringLength(50, ErrorMessage = "User Name can't be longer than 50 characters")]
        public string UserName { get; set; } = null!;
        [Required]
        [JsonIgnore]
        public string Password { get; set; } = null!;
        [Required]
        public Gender Gender { get; set; } = Gender.Others;
        [Required]
        public string? Address { get; set; }
        [Required]
        public string IdentityNumber { get; set; } = null!;
        [Required]
        public DateTime IdentityCardProvideDate { get; set; }
        [Required]
        public string IdentityCardFrontImage { get; set; } = null!;
        [Required]
        public string IdentityCardBackImage { get; set; } = null!;
        public string? Avatar { get; set; }
        [Required]
        public int Role { get; set; }
        [Required]
        public int Status { get; set; }

        //relationship
        public IList<Transaction> Transactions { get; set; } = null!;
        public IList<RealEstate> RealEstates { get; set; } = null!;
        public IList<BankAccount> BankAccounts { get; set; } = null!;
        public IList<UserBid> UserBids { get; set; } = null!;
        public IList<Notification> Notifications { get; set; } = null!;
        public IList<Auction> AuctionCreated { get; set; } = null!;
        public IList<Auction> AuctionApproved { get; set; } = null!;
        public IList<Feedback> Feedbacks { get; set; } = null!;
    }
}
