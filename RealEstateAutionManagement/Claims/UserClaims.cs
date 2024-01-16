using Data.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RealEstateAuctionManagement.Claims
{
    public static class UserClaims
    {
        public static string GetUserNameFromJwtToken(this IEnumerable<Claim> claims)
        {
            var userName = claims.FirstOrDefault(claims => claims.Type == ClaimTypes.Name)?.Value;
            return userName ?? "";
        }

        public static string GetUserIdFromJwtToken(this IEnumerable<Claim> claims)
        {
            var userId = claims.FirstOrDefault(claims => claims.Type == "id")?.Value;
            return userId ?? "";
        }
    }
}
