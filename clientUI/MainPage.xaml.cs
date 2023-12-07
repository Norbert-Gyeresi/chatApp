using Microsoft.Maui.Controls;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace clientUI
{
    public partial class MainPage : ContentPage
    {
        private TcpClient client;
        private StreamWriter writer;
        private string userName;

        public MainPage(TcpClient client, string username)
        {
            
            this.client = client;
            this.writer = new StreamWriter(client.GetStream(), Encoding.UTF8);
            this.userName = username;
            InitializeComponent();
        }

        private async void SendMessageButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                string message = $"msg|{MessageEntry.Text},{userName},all,{DateTime.UtcNow.TimeOfDay}";
                await writer.WriteLineAsync(message);
                await writer.FlushAsync();
                StatusLabel.Text = $"Üzenet elküldve: {message}";
            }
            catch (Exception ex)
            {
                StatusLabel.Text = $"Hiba az üzenet küldése közben: {ex.Message}";
            }
        }
    }
}
