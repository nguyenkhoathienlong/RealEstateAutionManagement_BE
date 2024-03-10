using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Data.EFCore
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Auction> Auctions { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<RealEstate> RealEstates { get; set; }
        public DbSet<RealEstateImage> RealEstateImages { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<UserBid> UserBids { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the relationship
            modelBuilder.Entity<Auction>()
            .HasOne(a => a.CreatedByUser)
            .WithMany(u => u.AuctionCreated)
            .HasForeignKey(a => a.CreateByUserId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Auction>()
                .HasOne(a => a.ApprovedByUser)
                .WithMany(u => u.AuctionApproved)
                .HasForeignKey(a => a.ApproveByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RealEstate>()
                .HasOne(a => a.ApprovedByUser)
                .WithMany(u => u.RealEstateApproved)
                .HasForeignKey(a => a.ApproveByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.SeedSettings();
        }
    }
}
