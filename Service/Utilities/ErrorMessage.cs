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
    }
}
