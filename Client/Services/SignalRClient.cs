using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Xamarin.Forms;
using XamarinFormsAzure.Models;

namespace XamarinFormsAzure.Services
{
    public class SignalRClient
    {
        private const string URL = "https://xamarinformsazureserver.azurewebsites.net";

        private const string HUB_NAME = "MobileHub";

        private const string ON_INFORMATION_RECEIVED = "OnInformationReceived";

        private const string EXCHANGE_INFORMATION = "ExchangeInformation";

        private readonly HubConnection connection;

        private readonly IHubProxy chatHubProxy;

        private MobileInformation mobileInformation;

        public delegate void InformationReceivedDelegate(string deviceId, string message);

        public event InformationReceivedDelegate OnInformationReceived;

        public SignalRClient()
        {
            this.connection = new HubConnection(URL);
            this.connection.StateChanged += HubConnectionStateChanged;

            this.chatHubProxy = this.connection.CreateHubProxy(HUB_NAME);
            this.chatHubProxy.On<string, string>(ON_INFORMATION_RECEIVED, (deviceId, message) => { OnInformationReceived?.Invoke(deviceId, message); });
        }

        public bool IsConnected { get { return this.connection.State == ConnectionState.Connected; } }

        public bool IsConnectedOrConnecting { get { return this.connection.State != ConnectionState.Disconnected; } }

        public void SendMessage(string deviceId, string message, string token)
        {
            try
            {
                if (IsConnected)
                {
                    Debug.WriteLine("Enviando Mensagem");
                    Debug.WriteLine($"{deviceId}\n{message}\n{token}");
                    this.chatHubProxy.Invoke(EXCHANGE_INFORMATION, deviceId, message, token);
                }
                else
                {
                    this.mobileInformation = new MobileInformation(null, deviceId, message, token);
                    Debug.WriteLine("O Client Hub ainda não está conectado. Guardando mensagem para enviar no futuro");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw e;
            }
        }

        public Task Start()
        {
            Debug.WriteLine("Iniciando SignalR client");
            return this.connection.Start();
        }

        public Task Stop()
        {
            Debug.WriteLine("Parando SignalR client");
            return Task.Run(() => this.connection.Stop());
        }

        private void HubConnectionStateChanged(StateChange stateChange)
        {
            if (stateChange.NewState == ConnectionState.Connected && this.mobileInformation != null)
            {
                Debug.WriteLine("Hub conectado - Enviando mensagem salva!");
                this.chatHubProxy.Invoke(EXCHANGE_INFORMATION, this.mobileInformation.DeviceId, this.mobileInformation.Message, this.mobileInformation.Token);

                this.mobileInformation = null;
            }
        }
    }
}
