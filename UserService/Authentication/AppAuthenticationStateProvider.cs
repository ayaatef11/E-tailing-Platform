using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace NewsProject.Client.Authenticaions { 
public class AppAuthenticationStateProvider : AuthenticationStateProvider
{

    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {

        var MyUser = new ClaimsIdentity();
        return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(MyUser)));

    }

} }

