using System;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(XamarinFormsAzure.iOS.Authenticator))]
namespace XamarinFormsAzure.iOS
{
    public class Authenticator : IAuthenticator
    {
        public IPlatformParameters PlatformParameters
        {
            get
            {
                var controller = UIApplication.SharedApplication.KeyWindow.RootViewController;
                return new PlatformParameters(controller);
            }
        }

        public TokenCache CustomTokenCache
        {
            get
            {
                // should not need this on iOS
                throw new NotImplementedException();
            }
        }
    }
}
