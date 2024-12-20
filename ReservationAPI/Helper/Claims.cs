using System.Security.Claims;

namespace ReservationAPI.Helper
{
    public static class Claims
    {
        public static string Id(this ClaimsPrincipal claim)
        {
            if (claim == null || claim.Identity == null || !claim.Identity.IsAuthenticated) return "";

            ClaimsPrincipal currentUser = claim;
            var name = currentUser.FindFirstValue(ClaimTypes.NameIdentifier);
            return name ?? "";
        }
        public static string Name(this ClaimsPrincipal claim)
        {
            if (claim == null || claim.Identity == null || !claim.Identity.IsAuthenticated) return "";

            ClaimsPrincipal currentUser = claim;
            var name = currentUser.FindFirstValue(ClaimTypes.Name);
            return name ?? "";
        }

        public static string Email(this ClaimsPrincipal claim)
        {
            if (claim == null || claim.Identity == null || !claim.Identity.IsAuthenticated) return "";

            ClaimsPrincipal currentUser = claim;
            var name = currentUser.FindFirstValue(ClaimTypes.Email);
            return name ?? "";
        }
    }
}
