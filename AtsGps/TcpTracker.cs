using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace AtsGps {
    public class TcpTracker {
        public long Id
        {
            get;
            set;
        }
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

        public String DataIn
        {
            get;
            set;
        }



        public String DataOut
        {
            get;
            set;
        }

        public DateTime DateTime
        {
            get;
            set;
        }

        public String Ip
        {
            get
            {
                if (this.TcpClient != null) {
                    IPEndPoint ep = this.TcpClient.Client.RemoteEndPoint as IPEndPoint;
                    return ep.Address.ToString();
                } else {
                    return "Unknown";
                }
            }
        }

        public String Port
        {
            get
            {
                if (this.TcpClient != null) {
                    IPEndPoint ep = this.TcpClient.Client.RemoteEndPoint as IPEndPoint;
                    return ep.Port.ToString();
                } else {
                    return "Unknown";
                }
            }
        }

    }
}
