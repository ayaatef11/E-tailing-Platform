


namespace UserService.services
{
    public class ClaimService(UserManager<ClaimService>_userManager)
    {

        public async Task UpdateUserClaimsAsync(string userId, List<Claim> newClaims)
        {
            var user = await _userManager.FindByIdAsync(userId) ?? throw new Exception("User not found");
            var currentClaims = await _userManager.GetClaimsAsync(user);

            // Remove existing claims
            foreach (var claim in currentClaims)
            {
                await _userManager.RemoveClaimAsync(user, claim);
            }

            // Add new claims
            foreach (var claim in newClaims)
            {
                await _userManager.AddClaimAsync(user, claim);
            }
        }

        public async Task<bool> RemoveClaimAsync(string userId, string claimType)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            var claims = await _userManager.GetClaimsAsync(user);
            var claimToRemove = claims.FirstOrDefault(c => c.Type == claimType);

            if (claimToRemove != null)
            {
                var result = await _userManager.RemoveClaimAsync(user, claimToRemove);
                return result.Succeeded;
            }

            return false;
        }

        public async Task<bool> AddOrUpdateClaimAsync(string userId, Claim newClaim)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            var claims = await _userManager.GetClaimsAsync(user);
            var existingClaim = claims.FirstOrDefault(c => c.Type == newClaim.Type);

            if (existingClaim != null)
            {
                var result = await _userManager.RemoveClaimAsync(user, existingClaim);
                if (!result.Succeeded)
                {
                    return false;
                }
            }

            var addResult = await _userManager.AddClaimAsync(user, newClaim);
            return addResult.Succeeded;
        }


    }
}
