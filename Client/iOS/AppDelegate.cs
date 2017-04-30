using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using WindowsAzure.Messaging;

namespace XamarinFormsAzure.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        private SBNotificationHub Hub { get; set; }

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            LoadApplication(new App());

            var settings = UIUserNotificationSettings.GetSettingsForTypes(UIUserNotificationType.Alert, new NSSet());

            UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            UIApplication.SharedApplication.RegisterForRemoteNotifications();
            HandlePushNotification(options);
            return base.FinishedLaunching(app, options);
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            Hub = new SBNotificationHub(PushNotificationConstants.HUB_CONNECTION_STRING, PushNotificationConstants.HUB_NAME);

            string normalizedDeviceToken = deviceToken.Description
                                                      .Replace(" ", string.Empty)
                                                      .Replace("<", string.Empty)
                                                      .Replace(">", string.Empty).ToUpper();

            System.Diagnostics.Debug.WriteLine($"******************************************");
            System.Diagnostics.Debug.WriteLine($"Device Token: {normalizedDeviceToken}");
            System.Diagnostics.Debug.WriteLine($"******************************************");

            Hub.UnregisterAllAsync(deviceToken, error =>
            {
                if (error != null)
                {
                    System.Diagnostics.Debug.WriteLine(new AggregateException("Erro ao registrar NotificationHub", new NSErrorException(error)));
                    return;
                }

                NSSet tags = null;
                Hub.RegisterNativeAsync(deviceToken, tags, errorCallback =>
                {
                    if (errorCallback != null)
                    {
                        System.Diagnostics.Debug.WriteLine(new AggregateException("Error at registering native hub", new NSErrorException(errorCallback)));
                    }
                });
            });
        }

        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
            HandlePushNotification(userInfo);
        }

        public override void DidReceiveRemoteNotification(UIApplication application,
                                                          NSDictionary userInfo,
                                                          Action<UIBackgroundFetchResult> completionHandler)
        {
            HandlePushNotification(userInfo);
            completionHandler(UIBackgroundFetchResult.NoData);
        }

        private void HandlePushNotification(NSDictionary userInfo)
        {
            if (userInfo == null || !userInfo.Any())
            {
                return;
            }

            Dictionary<string, string> notificationContent = new Dictionary<string, string>();
            foreach (NSObject info in userInfo.Keys)
            {
                notificationContent.Add(info as NSString, userInfo.ValueForKey(info as NSString) as NSString);
            }

            App.CurrentApp.HandleNotification(notificationContent);
        }
   }
}
