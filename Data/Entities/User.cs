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
        public string? Name { get; set; }
        public string? Email { get; set; }
        [Phone(ErrorMessage = "Phone is not true to the format")]
        [StringLength(13, ErrorMessage = "Phone number up to 13 characters long")]
        public string? PhoneNumber { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "User Name can't be longer than 50 characters")]
        public string? UserName { get; set; }
        [Required]
        [JsonIgnore]
        public string? Password { get; set; }
        public int Gender { get; set; }
        public string? Address { get; set; }
        public string? IdentityNumber { get; set; }
        public DateTime IdentityCardProvideDate { get; set; }
        public string? IdentityCardFrontImage { get; set; }
        public string? IdentityCardBackImage { get; set; }
        public string? Avatar { get; set; }
        public int Role { get; set; }

        //relationship
        public IList<Transaction>? Transactions { get; set; }
        public IList<RealEstate>? RealEstates { get; set; }
        public IList<BankAccount>? BankAccounts { get; set; }
        public IList<UserBid>? UserBids { get; set; }
        public IList<Notification>? Notifications { get; set; }
        public IList<Auction>? AuctionCreated { get; set; }
        public IList<Auction>? AuctionApproved { get; set; }
        public IList<Feedback>? Feedbacks { get; set; }
    }
}
