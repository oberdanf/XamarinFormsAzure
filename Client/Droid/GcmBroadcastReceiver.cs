using Android.App;
using Android.Content;
using Gcm;

[assembly: Permission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "com.google.android.c2dm.permission.RECEIVE")]
[assembly: UsesPermission(Name = "android.permission.INTERNET")]
[assembly: UsesPermission(Name = "android.permission.WAKE_LOCK")]
namespace XamarinFormsAzure.Droid
{
    [BroadcastReceiver(Permission = Gcm.Constants.PERMISSION_GCM_INTENTS)]

    [IntentFilter(new[] { Intent.ActionBootCompleted })] // Permite o GCM gerenciar push com app fechado

    [IntentFilter(new string[] { Gcm.Constants.INTENT_FROM_GCM_MESSAGE }, Categories = new[] { "@PACKAGE_NAME@" })]
    [IntentFilter(new string[] { Gcm.Constants.INTENT_FROM_GCM_REGISTRATION_CALLBACK }, Categories = new[] { "@PACKAGE_NAME@" })]
    [IntentFilter(new string[] { Gcm.Constants.INTENT_FROM_GCM_LIBRARY_RETRY }, Categories = new[] { "@PACKAGE_NAME@" })]
    [IntentFilter(new string[] { "com.google.firebase.INSTANCE_ID_EVENT" }, Categories = new[] { "@PACKAGE_NAME@" })]
    public class GcmBroadcastReceiver : GcmBroadcastReceiverBase<GcmService>
    {
        // Não precisa de nada aqui :)
    }
}
