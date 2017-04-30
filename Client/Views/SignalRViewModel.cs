using System;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinFormsAzure.Services;
using System.Diagnostics;
using Plugin.DeviceInfo;

namespace XamarinFormsAzure
{
    public class SignalRViewModel : BaseViewModel
    {
        private SignalRClient signalRClient;

        private const string MESSAGE_TOKEN = "TDC2017";

        public SignalRViewModel()
        {
            SendInfoCommand = new Command(OnSendInfo);
            StartServiceCommand = new Command(OnStartService);
            StopServiceCommand = new Command(OnStopService);
            this.signalRClient = new SignalRClient();
            signalRClient.OnInformationReceived += OnMessageReceived;
        }

        public ICommand SendInfoCommand { get; set; }

        public ICommand StartServiceCommand { get; set; }

        public ICommand StopServiceCommand { get; set; }

        public string InfoText { get; set; }

        public string Message { get; set; }

        private void OnSendInfo(object sender)
        {
            var cdi = CrossDeviceInfo.Current;
            string deviceId = $"{cdi.Model} {cdi.Platform} {cdi.Version}";
            GiveFeedback("Enviando mensagem...", true);
            this.signalRClient.SendMessage(deviceId, Message, MESSAGE_TOKEN);
            GiveFeedback("Mensagem enviada...", true);
        }

        private void OnMessageReceived(string id, string message)
        {
            Debug.WriteLine($"\nId: {id} - Mensagem: {message}");
            GiveFeedback("Mensagem recebida...", true);
            Device.BeginInvokeOnMainThread(() => InfoText += $"\nId: {id} - Mensagem: {message}");
        }

        private async void OnStartService(object sender)
        {
            try
            {
                GiveFeedback("Iniciando serviço...", true);
                await this.signalRClient.Start().ConfigureAwait(false);
                GiveFeedback("Serviço iniciado!", true);
            }
            catch (Exception ex)
            {
                GiveFeedback("Ocorreu um erro ao iniciar o serviço...", false);
                Debug.WriteLine(ex);
            }
        }

        private async void OnStopService(object sender)
        {
            try
            {
                GiveFeedback("Parando serviço...", true);
                await this.signalRClient.Stop();
                GiveFeedback("Serviço parado.", true);
            }
            catch (Exception ex)
            {
                GiveFeedback("Ocorreu um erro ao parar o serviço", false);
                Debug.WriteLine(ex);
            }
        }
    }
}