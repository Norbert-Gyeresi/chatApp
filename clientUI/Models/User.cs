using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clientUI.Models
{
    internal class User
    {
        public User(string userName, string password)
        {
            ID = Guid.NewGuid().ToString();
            UserName = userName;
            Password = password;
            SentMessages = new List<Message>();
            RecievedMessages = new List<Message>();
        }

        public string ID { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }

        public ICollection<Message> SentMessages { get; set; }

        public ICollection<Message> RecievedMessages { get; set; }

    }
}
