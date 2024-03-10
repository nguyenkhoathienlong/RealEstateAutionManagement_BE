using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Utilities
{
    public static class ErrorMessage
    {
        public static string IdNotExist = "ID_DOES_NOT_EXIST";
        public static string PhoneExist = "PHONE_EXISTED";
        public static string UserNameExist = "USER_EXISTED";
        public static string UserNameDoNotExist = "USER_DOES_NOT_EXISTED";
        public static string InvalidAccount = "USER_OR_PASSWORD_INCORRECT";
        public static string RoleNameExist = "ROLE_NAME_EXISTED";
        public static string BannedAccount = "ACCOUNT_BANNED";
        public static string RealEstateNotExist = "REAL_ESTATE_DOES_NOT_EXIST";
        public static string StartDateTooEarly = "START_DATE_TOO_EARLY";
        public static string EndDateNotSameAsStartDate = "END_DATE_NOT_SAME_AS_START_DATE";
        public static string EndTimeNotLaterThanStartTime = "END_TIME_NOT_LATER_THAN_START_TIME";
        public static string RealEstateAlreadyInAuction = "REAL_ESTATE_ALREADY_IN_AUCTION";
        public static string MaxBidIncrementLessThanBidIncrement = "MAX_BID_INCREMENT_LESS_THAN_BID_INCREMENT";
        public static string MaxBidIncrementNotMultipleOfBidIncrement = "MAX_BID_INCREMENT_NOT_MULTIPLE_OF_BID_INCREMENT";
        public static string ApprovalRequestExpired = "APPROVAL_REQUEST_EXPIRED";
        public static string RealEstateNotApproved = "REAL_ESTATE_NOT_APPROVED";
        public static string AuctionNotPending = "AUCTION_NOT_PENDING";
        public static string RealEstateNotPending = "REAL_ESTATE_NOT_PENDING";
        public static string RealEstateSoldOrRejected = "REAL_ESTATE_SOLD_OR_REJECTED";
        public static string CategoryNotExist = "CATEGORY_DOES_NOT_EXIST";
        public static string UserNotPending = "USER_NOT_PENDING";
        public static string PaymentRequestCreationError = "PAYMENT_REQUEST_CREATION_ERROR";
        public static string TransactionNotExist = "TRANSACTION_NOT_EXIST";
        public static string TransactionClosed = "TRANSACTION_CLOSED";
        public static string UserAlreadyRegisteredAuction = "USER_ALREADY_REGISTERED_AUCTION";
        public static string UserCannotRegisterOwnAuction = "USER_CANNOT_REGISTER_OWN_AUCTION";
        public static string AuctionNotOngoing = "AUCTION_NOT_ONGOING";
        public static string UserCannotBidOwnAuction = "USER_CANNOT_BID_OWN_AUCTION";
        public static string BidNotHigherThanCurrent = "BID_NOT_HIGHER_THAN_CURRENT";
        public static string AuctionNotApproved = "AUCTION_NOT_APPROVED";
        public static string AuctionNotOnGoing = "AUCTION_NOT_ONGOING";
        public static string UserNotVerified = "USER_NOT_VERIFIED";
        public static string AuctionNotCompleted = "AUCTION_NOT_COMPLETED";
        public static string UserNotAuctionWinner = "USER_NOT_AUCTION_WINNER";
        public static string UserNotRegisteredForAuction = "USER_NOT_REGISTERED_FOR_AUCTION";
        public static string BidNotMultipleOfIncrement = "BID_NOT_MULTIPLE_OF_INCREMENT";
        public static string BidGreaterThanMaxIncrement = "BID_GREATER_THAN_MAX_INCREMENT";
        public static string BidLessThanStartingPrice = "BID_LESS_THAN_STARTING_PRICE";
    }
}
