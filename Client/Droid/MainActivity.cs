using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Gcm;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Collections.Generic;
using Plugin.Permissions;

namespace XamarinFormsAzure.Droid
{
    [Activity(Label = "XamarinFormsAzure.Droid",
              Icon = "@drawable/icon",
              Theme = "@style/MyTheme",
              MainLauncher = true,
              LaunchMode = LaunchMode.SingleTop,
              ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            RegisterWithGcm();
            global::Xamarin.Forms.Forms.Init(this, bundle);

            LoadApplication(new App());
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            AuthenticationAgentContinuationHelper.SetAuthenticationAgentContinuationEventArgs(requestCode, resultCode, data);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);

            var notificationContent = new Dictionary<string, string>();
            foreach (string item in intent.Extras.KeySet())
            {
                notificationContent.Add(item, intent.Extras.Get(item).ToString());
            }

            App.CurrentApp.HandleNotification(notificationContent);
        }

        private void RegisterWithGcm()
        {
            try
            {
                GcmClient.CheckDevice(this);
                GcmClient.CheckManifest(this);

                System.Diagnostics.Debug.WriteLine("Registering...");
                GcmService.Initialize(this);
                GcmService.Register(this);
            }
            catch (Java.Net.MalformedURLException)
            {
                System.Diagnostics.Debug.WriteLine("There was an error creating the client. Verify the URL.", "Error");
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message, "Error");
            }
        }
    }
}
