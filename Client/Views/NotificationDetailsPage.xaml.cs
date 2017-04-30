using System.Collections.Generic;

using Xamarin.Forms;

namespace XamarinFormsAzure
{
    public partial class NotificationDetailsPage : ContentPage
    {
        public NotificationDetailsPage(Dictionary<string, string> notificationContent)
        {
        	InitializeComponent();
            BindingContext = new NotificationDetailsViewModel(notificationContent);
        }
    }
}
