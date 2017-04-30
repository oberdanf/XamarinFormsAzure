using System.Collections.Generic;
using System.Text;

namespace XamarinFormsAzure
{
    public class NotificationDetailsViewModel : BaseViewModel
    {
        public NotificationDetailsViewModel(Dictionary<string, string> notificationContent)
        {
            var sb = new StringBuilder();
            foreach (var item in notificationContent)
            {
                sb.AppendLine($"Chave: {item.Key} - Valor {item.Value}");
            }

            NotificationContent = sb.ToString();
        }

        public string NotificationContent { get; set; }
    }
}
