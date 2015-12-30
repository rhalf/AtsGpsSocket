using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace AtsGps {
    public class TcpTracker {
        public String Imei
        {
            get;
            set;
        }
        public TcpClient TcpClient
        {
            get;
            set;
        }
    }
}
