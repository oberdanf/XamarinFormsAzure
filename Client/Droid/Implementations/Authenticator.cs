using Android.App;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Xamarin.Forms;

[assembly: Dependency(typeof(XamarinFormsAzure.Droid.Authenticator))]
namespace XamarinFormsAzure.Droid
{
    public class Authenticator : IAuthenticator
    {
        private static readonly FileCache FILE_CACHE = new FileCache();

        public IPlatformParameters PlatformParameters => new PlatformParameters((Activity)Forms.Context);

        public TokenCache CustomTokenCache { get; } = FILE_CACHE;
    }
}
