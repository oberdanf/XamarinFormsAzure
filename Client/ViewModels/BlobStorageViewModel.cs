using System;
using System.Windows.Input;
using Xamarin.Forms;
using System.IO;
using System.Threading.Tasks;
using Plugin.Media.Abstractions;

namespace XamarinFormsAzure
{
    public class BlobStorageViewModel : BaseViewModel
    {
        public BlobStorageViewModel()
        {
            TakePictureCommand = new Command(OnTakePicture);
            PickPictureCommand = new Command(OnPickPicture);
            ImageSource = "http://www.sawyoo.com/postpic/2010/11/star-wars-facebook-avatar-profile_698654.jpg";
        }

        public ICommand TakePictureCommand { get; set; }

        public ICommand PickPictureCommand { get; set; }

        public ImageSource ImageSource { get; set; }

        private async void OnTakePicture(object obj)
        {
            try
            {
                await GetPictureAsync(() => App.CurrentApp.MediaService.TakePhotoAsync());
            }
            catch (Exception ex)
            {
                GiveFeedback("Ocorreu um erro ao tirar foto...", false);
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        private async void OnPickPicture(object obj)
        {
            try
            {
                await GetPictureAsync(() => App.CurrentApp.MediaService.PickPhotoAsync());
            }
            catch (Exception ex)
            {
                GiveFeedback("Ocorreu um erro ao escolher foto...", false);
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        private async Task GetPictureAsync(Func<Task<MediaFile>> getPictureAsync)
        {
            try
            {
                IsBusy = true;
                GiveFeedback("Escolhendo foto...", true);
                var file = await getPictureAsync();
                if (file != null)
                {
                    GiveFeedback("Foto escolhida!", true);
                    string imageName;
                    using (var stream = file.GetStream())
                    {
                        GiveFeedback("Fazendo upload pro Azure...", true);
                        imageName = await App.CurrentApp.BlobStorageService.UploadImageAsync(stream);
                    }

                    file.Dispose();

                    GiveFeedback("Baixando imagem do Azure...", true);
                    byte[] imgBytes = await App.CurrentApp.BlobStorageService.GetImageAsync(imageName);
                    ImageSource = ImageSource.FromStream(() => new MemoryStream(imgBytes));
                    GiveFeedback("Imagem carregada do Azure!", true);
                }
            }
            catch (Exception ex)
            {
                GiveFeedback("Ocorreu um erro ao fazer upload ou buscar a imagem do servidor.", false);
                System.Diagnostics.Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}