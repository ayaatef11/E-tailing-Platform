

namespace Repository.Identity
{
    public class IdentityContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> _userManager)
        {
            if (_userManager.Users.Count() == 0)
            {
                var usersData = File.ReadAllText("../Repository/Identity/DataSeeding/users.json");
                var users = JsonSerializer.Deserialize<List<AppUser>>(usersData);

                if (users?.Count() > 0)
                {
                    foreach (var user in users)
                    {
                        await _userManager.CreateAsync(user, "Pa$$w0rd");
                    }
                }
            }
        }
    }
}