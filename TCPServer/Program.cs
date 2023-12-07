using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TCPServer.Models;
using TCPServer.Data;
class Server
{
    private TcpListener tcpListener;
    private ConcurrentDictionary<Guid, TcpClient> clients = new ConcurrentDictionary<Guid, TcpClient>();
    private Repository repository = new Repository();

    public Server()
    {
        tcpListener = new TcpListener(IPAddress.Any, 8888);
        tcpListener.Start();

        Console.WriteLine("A szerver elindult, várakozás a kliensekre...");

        Task.Run(async () => await AcceptClientsAsync());
    }

    private async Task AcceptClientsAsync()
    {
        while (true)
        {
            TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();
            Guid clientId = Guid.NewGuid();

            clients.TryAdd(clientId, tcpClient);

            Console.WriteLine($"Kliens csatlakozott. Azonosító: {clientId}");

            // Kezeld a kliens üzeneteit aszinkron módon
            Task.Run(() => HandleClientMessagesAsync(clientId, tcpClient));
        }
    }

    private async Task HandleClientMessagesAsync(Guid clientId, TcpClient tcpClient)
    {
        try
        {
            StreamReader reader = new StreamReader(tcpClient.GetStream(), Encoding.UTF8);

            while (true)
            {
                string message = await reader.ReadLineAsync();

                if (message == null)
                {
                    break;
                }
                else if (message.Split('|').Length == 2)
                {
                    if (message.Split('|')[0] == "reg")
                    {
                        var userData = message.Split('|')[1].Split(',');

                        await repository.Registration(userData[0], userData[1]);

                        //User user = new User{ UserName = userData[0], Password = userData[1] };
                    }
                    else if (message.Split('|')[0] == "sign")
                    {
                        var userData = message.Split('|')[1].Split(',');

                        if (await repository.LogIn(userData[0], userData[1]))
                        {
                            StreamWriter writer = new StreamWriter(tcpClient.GetStream(), Encoding.UTF8);
                            await writer.WriteLineAsync($"log|{clientId},{userData[0]}");
                            await writer.FlushAsync();
                        }
                    }
                    if (message.Split('|')[0] == "msg")
                    {
                        var msgData = message.Split('|')[1].Split(",");

                        await repository.MessageSave(msgData);
                    }
                }

                Console.WriteLine($"Kliens ({clientId}): {message}");

                // Küldd az üzenetet minden más kliensnek (kivéve a küldőt)
                foreach (var otherClient in clients)
                {
                    if (otherClient.Key != clientId)
                    {
                        StreamWriter writer = new StreamWriter(otherClient.Value.GetStream(), Encoding.UTF8);
                        await writer.WriteLineAsync($"Kliens ({clientId}): {message}");
                        await writer.FlushAsync();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hiba a kliens ({clientId}) üzenetkezelése során: {ex.Message}");
        }
        finally
        {
            clients.TryRemove(clientId, out _);
            Console.WriteLine($"A kliens ({clientId}) lecsatlakozott.");
        }
    }

    static void Main()
    {
        Server server = new Server();
        Console.ReadLine(); // Várakozás a kilépésre
    }
}

