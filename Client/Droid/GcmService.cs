using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Support.V7.App;
using Gcm;
using WindowsAzure.Messaging;

[assembly: Permission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "com.google.android.c2dm.permission.RECEIVE")]
[assembly: UsesPermission(Name = "android.permission.INTERNET")]
[assembly: UsesPermission(Name = "android.permission.WAKE_LOCK")]
namespace XamarinFormsAzure.Droid
{

    [Service]
    public class GcmService : GcmServiceBase
    {
        private static NotificationHub hub;

        public static void Initialize(Context context)
        {
            hub = new NotificationHub(PushNotificationConstants.HUB_NAME, PushNotificationConstants.HUB_CONNECTION_STRING, context);
        }

        public static void Register(Context Context)
        {
            GcmClient.Register(Context, new[] { PushNotificationConstants.SENDER_ID });
        }

        public GcmService() : base(new[] { PushNotificationConstants.SENDER_ID })
        {
        }

        protected override void OnRegistered(Context context, string registrationId)
        {
            System.Diagnostics.Debug.WriteLine($"App registrado com GCM.\nRegistrationId: {registrationId}");

            if (hub != null)
            {
                System.Diagnostics.Debug.WriteLine("Registrando no NotificationHub...");
                string[] tags = null;
                hub.Register(registrationId, tags);
                System.Diagnostics.Debug.WriteLine("Registrado no NotificationHub!");
            }
        }

        protected override void OnUnRegistered(Context context, string registrationId)
        {
            if (hub != null)
            {
                System.Diagnostics.Debug.WriteLine("Removendo registro no NotificationHub...");
                hub.Unregister();
                System.Diagnostics.Debug.WriteLine("Registro removido no NotificationHub!");
            }
        }

        protected override void OnMessage(Context context, Intent intent)
        {
            System.Diagnostics.Debug.WriteLine("Notificação recebida");

            if (intent != null || intent.Extras != null)
            {
                System.Diagnostics.Debug.WriteLine("Logando Intent Extras...");

                foreach (var key in intent.Extras.KeySet())
                {
                    System.Diagnostics.Debug.WriteLine($"Key: {key} Value: {intent.Extras.Get(key)}");
                }

                string message = intent.Extras.GetString(PushNotificationConstants.MESSAGE);

                if (!string.IsNullOrWhiteSpace(message))
                {
                    CreateNotification("TDC 2017 - Notificação", message, intent.Extras);
                    return;
                }
            }
        }

        protected override void OnHandleIntent(Intent intent)
        {
            base.OnHandleIntent(intent);
        }

        protected override bool OnRecoverableError(Context context, string errorId)
        {
            System.Diagnostics.Debug.WriteLine($"Aconteceu um erro recuperável. Id {errorId}");
            return true;
        }

        protected override void OnError(Context context, string errorId)
        {
            System.Diagnostics.Debug.WriteLine($"Aconteceu um erro. Id {errorId}");
        }

        private void CreateNotification(string title, string message, Bundle extras)
        {
            System.Diagnostics.Debug.WriteLine("Gerando notificação...");

            var notificationManager = GetSystemService(NotificationService) as NotificationManager;
            var newIntent = new Intent(this, typeof(MainActivity));
            newIntent.AddFlags(ActivityFlags.SingleTop);

            string googleSentTime = extras.Get(PushNotificationConstants.GOOGLE_SENT_TIME).ToString();
            string from = extras.Get(PushNotificationConstants.FROM).ToString();
            string googleMessageId = extras.Get(PushNotificationConstants.GOOGLE_MESSAGE_ID).ToString();

            newIntent.PutExtra(PushNotificationConstants.GOOGLE_SENT_TIME, googleSentTime);
            newIntent.PutExtra(PushNotificationConstants.FROM, from);
            newIntent.PutExtra(PushNotificationConstants.GOOGLE_MESSAGE_ID, googleMessageId);

            NotificationCompat.Builder builder = new NotificationCompat.Builder(this);

            var notification = builder.SetContentIntent(PendingIntent.GetActivity(this, 0, newIntent, PendingIntentFlags.UpdateCurrent))
                                      .SetSmallIcon(Android.Resource.Drawable.SymDefAppIcon)
                                      .SetTicker(title)
                                      .SetContentTitle(title)
                                      .SetContentText(message)
                                      .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification))
                                      .SetAutoCancel(true)
                                      .Build();

            notificationManager.Notify(1, notification);
        }
    }
}
