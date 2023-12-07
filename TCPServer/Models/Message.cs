using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer.Models
{
    internal class Message
    {
        public string Sender { get; set; }
        public string Reciever { get; set; }
        public string MessageStr { get; set; }
        public TimeSpan TimeSent { get; set; }
    }
}
