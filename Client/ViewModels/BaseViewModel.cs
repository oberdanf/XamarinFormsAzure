using System.Threading.Tasks;
using PropertyChanged;
using Xamarin.Forms;

namespace XamarinFormsAzure
{
    [ImplementPropertyChanged]
    public abstract class BaseViewModel
    {
        public bool IsBusy { get; set; }

        public string Feedback { get; set; }

        public Color FeedbackColor { get; set; }

        protected async Task DisplayAlert(string title, string message, string cancel = null)
        {
            await App.CurrentApp.DisplayAlert(title, message, cancel);
        }

        protected void GiveFeedback(string message, bool positive)
        {
            Feedback = message;
            FeedbackColor = positive ? Color.Green : Color.Red;
        }
    }
}
