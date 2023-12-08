using clientUI.Models;
using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace clientUI
{
    public partial class MainPage : ContentPage
    {
        private TcpClient client;
        private string userName;
        private StreamReader reader;
        private List<string> messages;

        [Obsolete]
        public MainPage(TcpClient client, string username)
        {
            
            this.client = client;
            this.userName = username;
            this.messages = new List<string>();
            InitializeComponent();
            //MessageStackLayout.VerticalOptions = LayoutOptions.FillAndExpand;
            Task.Run(async () => await Teszt());
            
            //Task T = new Task(async() => 
            //{
            //    while (true)
            //    {
            //        this.reader = new StreamReader(client.GetStream(), Encoding.UTF8);
            //        var v = await reader.ReadToEndAsync();
            //        AddMessageToScrollView($"message: {v}");
            //        messages.Add(await reader.ReadLineAsync());
            //        Thread.Sleep(3000);
            //    }
                
            
            //}, TaskCreationOptions.LongRunning);
            //T.Start();
            //_ = ListenForMessagesAsync();
            //Task.Run(() => { ListenForMessagesAsync().Wait(); });
        }

        [Obsolete]
        private async Task Teszt()
        {
            
            while (true)
            {
                StreamReader reader = new StreamReader(client.GetStream(), Encoding.UTF8);

                try
                {
                    var v = await reader.ReadLineAsync();
                    if (v != null)
                    {

                        AddMessageToScrollView(v);
                    }
                }
                catch (Exception e)
                {

                    var t = e.Message;
                    ;
                }
                
            }
        }

        private async void SendMessageButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                string message = $"msg|{MessageEntry.Text},{userName},all,{DateTime.UtcNow.TimeOfDay}";

                var messageWriter = new StreamWriter(client.GetStream(), Encoding.UTF8);
                //var reader = new StreamReader(client.GetStream(), Encoding.UTF8);

                await messageWriter.WriteLineAsync(message);
                await messageWriter.FlushAsync();

                //Thread.Sleep(300);
                //string response = await reader.ReadLineAsync();
                //AddMessageToScrollView($"szerver válasza: {response}");
                //messages.Add(response);

                //AddMessageToScrollView($"hossz: {this.messages.Count}");
            }
            catch (Exception ex)
            {
                //AddMessageToScrollView($"Hiba az üzenet küldése közben: {ex.Message}");
            }
        }

        [Obsolete]
        private void AddMessageToScrollView(string message)
        {
            
            Label messageLabel = new Label
            {
                Text = message,
                FontSize = 16,
                Margin = new Thickness(0, 5, 0, 0)
            };
            Device.BeginInvokeOnMainThread(() =>
            {
                MessageStackLayout.Children.Add(messageLabel);
                MessageScrollView.ScrollToAsync(messageLabel, ScrollToPosition.End, true);
            });
            Console.WriteLine(message);
            
        }

        //private async Task ListenForMessagesAsync()
        //{
        //    try
        //    {
        //        while (true)
        //        {
        //            // Continuously read incoming messages from the server
        //            string receivedMessage = null;

        //            using (var messageReader = new StreamReader(client.GetStream(), Encoding.UTF8))
        //            {
        //                receivedMessage = await messageReader.ReadLineAsync();
        //            }

        //            // Update UI with the received message
        //            Device.BeginInvokeOnMainThread(() =>
        //            {
        //                AddMessageToScrollView($"Received message: {receivedMessage}");
        //            });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle exceptions (e.g., connection closed)
        //        Device.BeginInvokeOnMainThread(() =>
        //        {
        //            AddMessageToScrollView($"Error while listening for messages: {ex.Message}");
        //        });
        //    }
        //}
    }
}
