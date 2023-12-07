using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using TCPServer.Models;

namespace TCPServer.Data
{
    internal class Repository
    {
        const string connectionUri = "mongodb+srv://macska:macska@cluster0.dzaxngd.mongodb.net/?retryWrites=true&w=majority";

        public Repository()
        {
            var settings = MongoClientSettings.FromConnectionString(connectionUri);

            settings.ServerApi = new ServerApi(ServerApiVersion.V1);

            this.Client = new MongoClient(settings);

            this.UserCollection = Client.GetDatabase("chatapp").GetCollection<User>("users");
        }

        public MongoClient Client { get; set; }

        public IMongoCollection<User> UserCollection { get; set; }

        public IMongoCollection<Message> MessageCollection { get; set; }

        public async Task<string> Registration(string username, string password)
        {
            var filter = Builders<User>.Filter.Eq(user => user.UserName, username);

            var user = await UserCollection.Find(filter).FirstOrDefaultAsync();
            
            if (user == null) 
            {
                UserCollection.InsertOne(new User { UserName = username, Password = password});

                return "fasza";
            }

            return "Already existing";
        }

        public async Task<bool> LogIn(string username, string password)
        {
            bool isOk = false;

            var filter = Builders<User>.Filter.Eq(user => user.UserName, username);

            var user = await UserCollection.Find(filter).FirstOrDefaultAsync();

            if (user.UserName == username && user.Password == password)
            {
                return !isOk;
                //redirect to masik ablak XD
            }

            return isOk; //"visszadobni ugyanazt az ablakot egy invalid messaggel";
        }

        public async Task<string> MessageSave(string[] data)
        {
            var filter = Builders<User>.Filter.Eq(user => user.UserName, data[1]);

            var user = await UserCollection.Find(filter).FirstOrDefaultAsync();

            user.SentMessages.Add(new Message { Sender = data[1], Reciever = data[2], MessageStr = data[0], TimeSent = TimeSpan.Parse(data[3]) });

            var update = Builders<User>.Update
              .Set(userUpdate => userUpdate.SentMessages, user.SentMessages);

            await UserCollection.UpdateOneAsync(filter, update);

            return "Already existing";
        }
    }
}
