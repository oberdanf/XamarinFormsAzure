using System.Windows.Input;
using Xamarin.Forms;
using System.Text;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;

namespace XamarinFormsAzure
{
    public class HomePageViewModel : BaseViewModel
    {
        public HomePageViewModel()
        {
            LoginCommand = new Command(OnLogin);
            LogoutCommand = new Command(OnLogout);
            OpenStorageBlobPageCommand = new Command(OnOpenStorageBlobPage);
            OpenStorageQueuePageCommand = new Command(OnOpenStorageQueuePage);
            OpenSignalRPageCommand = new Command(OnOpenSignalRPage);
            AuthInformation = "Autentique-se!";
            IsAuthenticated = false;
        }

        public bool IsAuthenticated { get; set; }
        public ICommand LoginCommand { get; private set; }
        public ICommand LogoutCommand { get; private set; }
        public ICommand OpenStorageBlobPageCommand { get; private set; }
        public ICommand OpenStorageQueuePageCommand { get; private set; }
        public ICommand OpenSignalRPageCommand { get; private set; }

        public string AuthInformation { get; set; }

        private async void OnLogin(object sender)
        {
            var cachedItem = App.CurrentApp.AuthenticatorService.IsAuthenticated(Constants.CACHED_USER_ID);
            if (cachedItem != null)
            {
                AuthInformation = GetAuthInformation(cachedItem);
                IsAuthenticated = true;
                return;
            }

            var authResult = await App.CurrentApp.AuthenticatorService.Authenticate();

            if (authResult == null)
            {
                AuthInformation = "Autentique-se!";
                IsAuthenticated = false;
                return;
            }

            AuthInformation = GetAuthInformation(authResult);
            IsAuthenticated = true;
        }

        private void OnLogout(object sender)
        {
            App.CurrentApp.AuthenticatorService.Logout();
            AuthInformation = "Você foi deslogado. Autentique-se!";
            IsAuthenticated = false;
        }

        private string GetAuthInformation(AuthenticationResult authResult)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Você acabou de se autenticar!");
            sb.AppendLine($"Id: {authResult.UserInfo.DisplayableId}");
            sb.AppendLine($"Nome: {authResult.UserInfo.GivenName}");
            sb.AppendLine($"Sobrenome: {authResult.UserInfo.FamilyName}");
            sb.AppendLine($"Identity Provider: {authResult.UserInfo.IdentityProvider}");
            sb.AppendLine($"Tenant Id: { authResult.TenantId }");
            sb.AppendLine($"Expira em: { authResult.ExpiresOn }");
            sb.AppendLine($"Id Token: { authResult.IdToken}");
            return sb.ToString();
        }

        private string GetAuthInformation(TokenCacheItem tokenCacheItem)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Você já está autenticado!");
            sb.AppendLine($"Id: {tokenCacheItem.DisplayableId}");
            sb.AppendLine($"Nome: {tokenCacheItem.GivenName}");
            sb.AppendLine($"Sobrenome: {tokenCacheItem.FamilyName}");
            sb.AppendLine($"Identity Provider: {tokenCacheItem.IdentityProvider}");
            sb.AppendLine($"Tenant Id: { tokenCacheItem.TenantId }");
            sb.AppendLine($"Expira em: { tokenCacheItem.ExpiresOn }");
            sb.AppendLine($"Id Token: { tokenCacheItem.IdToken}");
            return sb.ToString();
        }

        private async void OnOpenStorageQueuePage(object sender)
        {
            try
            {
                await App.CurrentApp.PushPageAsync(new QueueStoragePage());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        private async void OnOpenStorageBlobPage(object sender)
        {
            try
            {
                await App.CurrentApp.PushPageAsync(new BlobStoragePage());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        private async void OnOpenSignalRPage(object sender)
        {
            try
            {
                await App.CurrentApp.PushPageAsync(new SignalRPage());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }
    }

}
