using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Xamarin.Forms;

namespace XamarinFormsAzure
{
    public class AuthenticatorService
    {
        private AuthenticationResult authenticationResult;

        public IAuthenticator PlatformAuthenticator { get; set; } = DependencyService.Get<IAuthenticator>();

        public async Task<AuthenticationResult> Authenticate()
        {
            try
            {
                this.authenticationResult = await Authenticate(
                    Constants.COMMON_AUTHORITY,
                    Constants.GRAPH_RESOURCE_URI,
                    Constants.AZURE_CLIENT_ID,
                    Constants.RETURN_URI.OriginalString,
                    Constants.CACHED_USER_ID,
                    Constants.DOMAIN_HINT);
                return this.authenticationResult;
            }
            catch (AdalServiceException)
            {
                Logout();
                return null;
            }
        }

        public void Logout()
        {
            var authContext = new AuthenticationContext(Constants.COMMON_AUTHORITY);
            // assume que somente um usuário usará o app
            authContext.TokenCache.Clear();
            this.authenticationResult = null;
        }

        public TokenCacheItem IsAuthenticated(string username)
        {
            var authContext = new AuthenticationContext(Constants.COMMON_AUTHORITY);

            return authContext.TokenCache.ReadItems().FirstOrDefault(i => i.DisplayableId.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        private async Task<AuthenticationResult> Authenticate(string authority, string resource, string clientId, string returnUri, string userIdentifier = null, string domainHint = null)
        {
            var authContext = new AuthenticationContext(authority);

            if (authContext.TokenCache.ReadItems().Any())
            {
                var cachedAuthority = authContext.TokenCache.ReadItems().First().Authority;

                authContext = Device.RuntimePlatform == Device.Android
                                    ? new AuthenticationContext(cachedAuthority, PlatformAuthenticator.CustomTokenCache)
                                    : new AuthenticationContext(cachedAuthority);
            }

            var uri = new Uri(returnUri);
            var userId = string.IsNullOrEmpty(userIdentifier)
                               ? UserIdentifier.AnyUser
                               : new UserIdentifier(userIdentifier, UserIdentifierType.OptionalDisplayableId);

            AuthenticationResult authResult = await authContext.AcquireTokenAsync(
                resource,
                clientId,
                uri,
                PlatformAuthenticator.PlatformParameters,
                userId,
                domainHint);

            return authResult;
        }
    }
}
