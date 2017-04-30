using Xamarin.Forms;

namespace XamarinFormsAzure
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
            BindingContext = new HomePageViewModel();
        }
    }
}
