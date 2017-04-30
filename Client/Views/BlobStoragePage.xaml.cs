using Xamarin.Forms;

namespace XamarinFormsAzure
{
    public partial class BlobStoragePage : ContentPage
    {
        public BlobStoragePage()
        {
            InitializeComponent();
            BindingContext = new BlobStorageViewModel();
        }
    }
}
