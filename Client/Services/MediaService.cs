using System;
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;

namespace XamarinFormsAzure
{
    public class MediaService
    {
        public async Task<MediaFile> TakePhotoAsync()
        {
            return await GetPhotoAsync(() => CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Directory = "XamarinFormsAzure",
                Name = $"{DateTime.UtcNow.Ticks}.jpg",
                PhotoSize = PhotoSize.Small,
                CompressionQuality = 92,
                SaveToAlbum = true,
                DefaultCamera = CameraDevice.Front
            }));
        }

        public async Task<MediaFile> PickPhotoAsync()
        {
            return await GetPhotoAsync(() => CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
            {
                PhotoSize = PhotoSize.Small,
                CompressionQuality = 92,
            }));
        }

        public async Task<MediaFile> GetPhotoAsync(Func<Task<MediaFile>> getPhotoAsyncAction)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {

                await App.CurrentApp.DisplayAlert("Câmera", "Câmera indisponível");
                return null;
            }

            MediaFile file = await getPhotoAsyncAction();

            if (file != null)
            {
                System.Diagnostics.Debug.WriteLine($"Caminho do arquivo:\n Privado: {file.Path}\nPúblico: {file.AlbumPath}");
            }

            return file;
        }
    }
}
