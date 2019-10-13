using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WOA_DEVServer.Network.Users
{
    public class User
    {
        public string username;
        public string rank;
        public TcpClient tcp_id;
    }
}
