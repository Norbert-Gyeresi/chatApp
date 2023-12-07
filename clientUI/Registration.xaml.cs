using Microsoft.Maui.Controls;
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using clientUI.Models;
using System.Text;
using clientUI;
//using Windows.Media.Protection.PlayReady;

namespace ClientUI
{
    public partial class RegistrationPage : ContentPage
    {
        private TcpClient client;
        private StreamWriter writer;
        private StreamReader reader;

        public RegistrationPage()
        {
            InitializeComponent();
            InitializeClient();
            ConnectPls();
        }

        private void InitializeClient()
        {
            this.client = new TcpClient();
        }

        private async void RegisterButton_Clicked(object sender, System.EventArgs e)
        {
            // Itt hajtsd végre a regisztrációs logikát
            string userName = UserNameEntry.Text;
            string password = PasswordEntry.Text;

            //User user = new User(userName, password);

            try
            {
                string message = $"reg|{userName},{password}";
                await writer.WriteLineAsync(message);
                await writer.FlushAsync();
            }
            catch (Exception ex)
            {

            }
        }

        private async void LogInButton_Clicked(object sender, System.EventArgs e)
        {
            string userName = UserNameEntry.Text;
            string password = PasswordEntry.Text;

            try
            {
                
                string message = $"sign|{userName},{password}";
                await Console.Out.WriteLineAsync(message);
                await writer.WriteLineAsync(message);
                await writer.FlushAsync();
                Thread.Sleep(300);
                message = await reader.ReadLineAsync();

                if (message.Split('|')[0] == "log") 
                {

                    await Navigation.PushAsync(new MainPage(client, userName));
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async void ConnectPls()
        {
            try
            {
                await client.ConnectAsync("192.168.0.202", 8888);
                writer = new StreamWriter(client.GetStream(), Encoding.UTF8);
                reader = new StreamReader(client.GetStream(), Encoding.UTF8);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
