using Core.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Extensions
{
    public static class UserClaimPrincipalExtensions
    {

        public static async Task<string> GetCurrentUserId(this ClaimsPrincipal principal, UserManager<User> userManager)
        {
            string email = principal.FindFirstValue(ClaimTypes.Email);

            var user = await userManager.FindByEmailAsync(email);

            return user.Id;
        } 
    }
}
