using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace XamarinFormsAzure
{
    public interface IAuthenticator
    {
        IPlatformParameters PlatformParameters { get; }
        TokenCache CustomTokenCache { get; }
    }
}
