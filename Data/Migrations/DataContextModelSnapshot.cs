﻿// <auto-generated />
using System;
using Data.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Data.Entities.Auction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ApproveByUserId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("ApproveTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<float>("BidIncrement")
                        .HasColumnType("real");

                    b.Property<Guid>("CreateByUserId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DateCreate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DateUpdate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<float>("Deposit")
                        .HasColumnType("real");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<float?>("MaxBidIncrement")
                        .HasColumnType("real");

                    b.Property<Guid>("RealEstateId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("RegistrationEndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<float>("RegistrationFee")
                        .HasColumnType("real");

                    b.Property<DateTime>("RegistrationStartDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<float>("StartingPrice")
                        .HasColumnType("real");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ApproveByUserId");

                    b.HasIndex("CreateByUserId");

                    b.HasIndex("RealEstateId");

                    b.ToTable("Auctions");
                });

            modelBuilder.Entity("Data.Entities.BankAccount", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("BankAccountName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("BankAccountNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("BankAccountType")
                        .HasColumnType("integer");

                    b.Property<string>("BankName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("DateCreate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DateUpdate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("BankAccounts");
                });

            modelBuilder.Entity("Data.Entities.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DateCreate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DateUpdate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Data.Entities.Feedback", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DateCreate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DateUpdate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Feedbacks");
                });

            modelBuilder.Entity("Data.Entities.Notification", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DateCreate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DateUpdate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("Data.Entities.RealEstate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("ApproveTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DateCreate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DateUpdate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("LinkAttachment")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("UserId");

                    b.ToTable("RealEstates");
                });

            modelBuilder.Entity("Data.Entities.RealEstateImage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DateCreate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DateUpdate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<Guid>("RealEstateId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("RealEstateId");

                    b.ToTable("RealEstateImages");
                });

            modelBuilder.Entity("Data.Entities.Setting", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("DataUnit")
                        .HasColumnType("integer");

                    b.Property<DateTime>("DateCreate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DateUpdate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("Data.Entities.Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<float>("Amount")
                        .HasColumnType("real");

                    b.Property<DateTime>("DateCreate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DateUpdate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<int>("PaymentMethod")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<Guid?>("UserBidId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserBidId");

                    b.HasIndex("UserId");

                    b.ToTable("Transaction");
                });

            modelBuilder.Entity("Data.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Avatar")
                        .HasColumnType("text");

                    b.Property<DateTime>("DateCreate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DateUpdate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Gender")
                        .HasColumnType("integer");

                    b.Property<string>("IdentityCardBackImage")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("IdentityCardFrontImage")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("IdentityCardProvideDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("IdentityNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("character varying(13)");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Data.Entities.UserBid", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<float>("Amount")
                        .HasColumnType("real");

                    b.Property<Guid>("AuctionId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DateCreate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DateUpdate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDeposit")
                        .HasColumnType("boolean");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("AuctionId");

                    b.HasIndex("UserId");

                    b.ToTable("UserBids");
                });

            modelBuilder.Entity("Data.Entities.Auction", b =>
                {
                    b.HasOne("Data.Entities.User", "ApprovedByUser")
                        .WithMany("AuctionApproved")
                        .HasForeignKey("ApproveByUserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Data.Entities.User", "CreatedByUser")
                        .WithMany("AuctionCreated")
                        .HasForeignKey("CreateByUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Data.Entities.RealEstate", "RealEstates")
                        .WithMany("Auctions")
                        .HasForeignKey("RealEstateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ApprovedByUser");

                    b.Navigation("CreatedByUser");

                    b.Navigation("RealEstates");
                });

            modelBuilder.Entity("Data.Entities.BankAccount", b =>
                {
                    b.HasOne("Data.Entities.User", "Users")
                        .WithMany("BankAccounts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Users");
                });

            modelBuilder.Entity("Data.Entities.Feedback", b =>
                {
                    b.HasOne("Data.Entities.User", "Users")
                        .WithMany("Feedbacks")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Users");
                });

            modelBuilder.Entity("Data.Entities.Notification", b =>
                {
                    b.HasOne("Data.Entities.User", "Users")
                        .WithMany("Notifications")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Users");
                });

            modelBuilder.Entity("Data.Entities.RealEstate", b =>
                {
                    b.HasOne("Data.Entities.Category", "Categories")
                        .WithMany("RealEstates")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Entities.User", "Users")
                        .WithMany("RealEstates")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Categories");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("Data.Entities.RealEstateImage", b =>
                {
                    b.HasOne("Data.Entities.RealEstate", "RealEstates")
                        .WithMany("RealEstateImages")
                        .HasForeignKey("RealEstateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RealEstates");
                });

            modelBuilder.Entity("Data.Entities.Transaction", b =>
                {
                    b.HasOne("Data.Entities.UserBid", "UserBid")
                        .WithMany("Transactions")
                        .HasForeignKey("UserBidId");

                    b.HasOne("Data.Entities.User", "Users")
                        .WithMany("Transactions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserBid");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("Data.Entities.UserBid", b =>
                {
                    b.HasOne("Data.Entities.Auction", "Auctions")
                        .WithMany()
                        .HasForeignKey("AuctionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Entities.User", "Users")
                        .WithMany("UserBids")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Auctions");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("Data.Entities.Category", b =>
                {
                    b.Navigation("RealEstates");
                });

            modelBuilder.Entity("Data.Entities.RealEstate", b =>
                {
                    b.Navigation("Auctions");

                    b.Navigation("RealEstateImages");
                });

            modelBuilder.Entity("Data.Entities.User", b =>
                {
                    b.Navigation("AuctionApproved");

                    b.Navigation("AuctionCreated");

                    b.Navigation("BankAccounts");

                    b.Navigation("Feedbacks");

                    b.Navigation("Notifications");

                    b.Navigation("RealEstates");

                    b.Navigation("Transactions");

                    b.Navigation("UserBids");
                });

            modelBuilder.Entity("Data.Entities.UserBid", b =>
                {
                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
