using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Linq;

namespace XamarinFormsAzure
{
    public class QueueStorageViewModel : BaseViewModel
    {
        public QueueStorageViewModel()
        {
            SendMessageCommand = new Command(OnSendMessage);
            ReadMessagesCommand = new Command(OnReadMessages);
            Messages = new ObservableCollection<string>();
        }

        public ICommand SendMessageCommand { get; set; }

        public ICommand ReadMessagesCommand { get; set; }

        public string Message { get; set; }

        public ObservableCollection<string> Messages { get; set; }

        public async void OnSendMessage(object sender)
        {
            try
            {
                IsBusy = true;
                Feedback = string.Empty;

                if (!string.IsNullOrEmpty(Message))
                {
                    await App.CurrentApp.QueueStorageService.AddMessageAsync(Message);
                    GiveFeedback("Mensagem enviada com sucesso!", true);
                    Message = string.Empty;
                }
                else
                {
                    GiveFeedback("Você precisa escrever uma mensagem para enviar!", false);
                }
            }
            catch (Exception ex)
            {
                GiveFeedback("Ocorreu um erro ao enviar mensagem!", false);
                System.Diagnostics.Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void OnReadMessages(object obj)
        {
            try
            {
                IsBusy = true;
                IEnumerable<string> messages = await App.CurrentApp.QueueStorageService.GetMessagesAsync();
                Messages = new ObservableCollection<string>(messages.Any() ? messages : new[] { "Não há nenhuma mensagem na Queue" });
                GiveFeedback("Mensagens carregadas!", true);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                GiveFeedback("Não foi possível carregar as mensagens :(", false);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}