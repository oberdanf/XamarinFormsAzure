using Xamarin.Forms;

namespace XamarinFormsAzure
{
    public partial class SignalRPage : ContentPage
    {
        public SignalRPage()
        {
            InitializeComponent();
            BindingContext = new SignalRViewModel();
        }
    }
}
