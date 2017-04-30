using Xamarin.Forms;

namespace XamarinFormsAzure
{
    public partial class QueueStoragePage : ContentPage
    {
        public QueueStoragePage()
        {
            InitializeComponent();
            BindingContext = new QueueStorageViewModel();
        }
    }
}
