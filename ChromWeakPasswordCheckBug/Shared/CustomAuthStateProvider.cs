using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChromWeakPasswordCheckBug.Shared
{
    public interface ILogonHandler
    {
        Task LogonAsync();
        Task LogoffAsync();
    }

    public class CustomAuthStateProvider : AuthenticationStateProvider, ILogonHandler
    {
        private AuthenticationState _authenticationState = new AuthenticationState(new ClaimsPrincipal());

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // adding this task delay makes it work
            //await Task.Delay(1);
            return _authenticationState;
        }

        public async Task LogonAsync()
        {
            await ClearAuthStateAsync();

            var identity = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Name, "mrfibuli"),
            }, "Fake authentication type");

            var user = new ClaimsPrincipal(identity);
            _authenticationState = new AuthenticationState(user);

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task LogoffAsync()
        {
            await ClearAuthStateAsync();
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        private async Task ClearAuthStateAsync()
        {
            _authenticationState = new AuthenticationState(new ClaimsPrincipal());
        }
    }
}
