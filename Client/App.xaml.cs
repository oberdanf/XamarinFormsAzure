using Xamarin.Forms;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace XamarinFormsAzure
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new HomePage());
        }

        public static App CurrentApp => (App)Current;

        public AuthenticatorService AuthenticatorService => new AuthenticatorService();
        public BlobStorageService BlobStorageService => new BlobStorageService();
        public QueueStorageService QueueStorageService => new QueueStorageService();
        public MediaService MediaService => new MediaService();

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        public async Task DisplayAlert(string title, string message, string cancel = null)
        {
            await Current.MainPage.DisplayAlert(title, message, cancel ?? "OK");
        }

        public async Task PushPageAsync(Page page)
        {
            await ((NavigationPage)Current.MainPage).PushAsync(page);
        }

        public async void HandleNotification(Dictionary<string, string> notificationContent)
        {
            try
            {
                await PushPageAsync(new NotificationDetailsPage(notificationContent));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }
    }
}
