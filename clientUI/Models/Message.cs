using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace clientUI.Models
{
    internal class Message
    {
        public Message(User sender, User reciever, string messageStr)
        {
            Sender = sender;
            Reciever = reciever;
            MessageStr = messageStr;
            TimeSent = DateTime.UtcNow.TimeOfDay;
        }

        public User Sender { get; set; }
        public User Reciever { get; set; }
        public string MessageStr { get; set; }
        public TimeSpan TimeSent { get; }
    }
}
