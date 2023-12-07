using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer.Models
{
    internal class User
    {
        [BsonElement("_id")]
        public ObjectId Id { get; set; }
        public string UserName { get; set; }

        public string Password { get; set; }

        public ICollection<Message> SentMessages{ get; set; } = new List<Message>();

        public ICollection<Message> RecievedMessages { get; set; } = new List<Message>();

    }
}
