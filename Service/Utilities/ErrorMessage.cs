using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Utilities
{
    public static class ErrorMessage
    {
        public static string IdNotExist = "ID không tồn tại.";
        public static string PhoneExist = "Số điện thoại đã tồn tại.";
        public static string UserNameExist = "Tên người dùng đã tồn tại.";
        public static string UserNameDoNotExist = "Người dùng không tồn tại.";
        public static string InvalidAccount = "Tên người dùng hoặc mật khẩu không chính xác.";
        public static string RoleNameExist = "Tên vai trò đã tồn tại.";
        public static string BannedAccount = "Tài khoản đã bị cấm.";
        public static string RealEstateNotExist = "Bất động sản không tồn tại.";
        public static string StartDateTooEarly = "Ngày bắt đầu quá sớm.";
        public static string EndDateNotSameAsStartDate = "Ngày kết thúc không giống ngày bắt đầu.";
        public static string EndTimeNotLaterThanStartTime = "Thời gian kết thúc không muộn hơn thời gian bắt đầu.";
        public static string RealEstateAlreadyInAuction = "Bất động sản đã có trong phiên đấu giá.";
        public static string MaxBidIncrementLessThanBidIncrement = "Giá trị tăng tối đa nhỏ hơn giá trị tăng.";
        public static string MaxBidIncrementNotMultipleOfBidIncrement = "Giá trị tăng tối đa không phải là bội số của giá trị tăng.";
        public static string ApprovalRequestExpired = "Yêu cầu phê duyệt đã hết hạn.";
        public static string RealEstateNotApproved = "Bất động sản chưa được phê duyệt.";
        public static string AuctionNotPending = "Phiên đấu giá không đang chờ xử lý.";
        public static string RealEstateNotPending = "Bất động sản không đang chờ xử lý.";
        public static string RealEstateSoldOrRejected = "Bất động sản đã được bán hoặc từ chối.";
        public static string CategoryNotExist = "Danh mục không tồn tại.";
        public static string UserNotPending = "Người dùng không đang chờ xử lý.";
        public static string PaymentRequestCreationError = "Lỗi khi tạo yêu cầu thanh toán.";
        public static string TransactionNotExist = "Giao dịch không tồn tại.";
        public static string TransactionClosed = "Giao dịch đã đóng.";
        public static string UserAlreadyRegisteredAuction = "Người dùng đã đăng ký phiên đấu giá.";
        public static string UserCannotRegisterOwnAuction = "Người dùng không thể đăng ký phiên đấu giá của chính mình.";
        public static string AuctionNotOngoing = "Phiên đấu giá không đang diễn ra.";
        public static string UserCannotBidOwnAuction = "Người dùng không thể đặt giá cho phiên đấu giá của chính mình.";
        public static string BidNotHigherThanCurrent = "Giá đặt phải cao hơn giá hiện tại.";
        public static string AuctionNotApproved = "Phiên đấu giá chưa được phê duyệt.";
        public static string AuctionNotOnGoing = "Phiên đấu giá không đang diễn ra.";
        public static string UserNotVerified = "Người dùng chưa được xác minh.";
        public static string AuctionNotCompleted = "Phiên đấu giá chưa hoàn thành.";
        public static string UserNotAuctionWinner = "Người dùng không phải là người chiến thắng phiên đấu giá.";
        public static string UserNotRegisteredForAuction = "Người dùng chưa đăng ký phiên đấu giá.";
        public static string BidNotMultipleOfIncrement = "Giá đặt phải là bội số của giá trị tăng.";
        public static string BidGreaterThanMaxIncrement = "Giá đặt vượt quá giá trị tăng tối đa.";
        public static string BidLessThanStartingPrice = "Giá đặt phải lớn hơn giá khởi điểm.";
    }
}
