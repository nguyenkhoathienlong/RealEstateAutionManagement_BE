

namespace Data.Enum
{
    public class EnumType
    {
    }

    public enum Gender
    {
        Male,
        Female,
        Others
    }
    public enum Role
    {
        Staff,
        Guest,
        Member
    }
    public enum UserStatus
    {
        Active,
        Inactive,
        Banned
    }
    public enum AuctionStatus
    {
        Pending,
        Approved,
        OnGoing,
        Completed,
        Rejected,
        Failed
    }
    public enum SettingDataUnit
    {
        Percent = 0,
        Minutes = 1,
        Hours = 2,
        Days = 3,
        Meters = 4,
        Kilometers = 5,
        Times = 6,
        Default = 7,
        Vnd = 8
    }
}
