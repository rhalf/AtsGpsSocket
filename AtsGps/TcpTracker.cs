using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace AtsGps {
    public class TcpTracker {

        private Guid _id;

        public TcpTracker () {
            _id = Guid.NewGuid();
        }

        public Guid Id {
            get {
                return _id;
            }
        }
        public String Imei {
            get;
            set;
        }
        public TcpClient TcpClient {
            get;
            set;
        }

        public String DataIn {
            get;
            set;
        }



        public String DataOut {
            get;
            set;
        }

        public DateTime DateTime {
            get;
            set;
        }

        public String Ip {
            get {
                IPEndPoint remote = (IPEndPoint)TcpClient.Client.RemoteEndPoint;
                return remote.Address.ToString();
            }
        }

        public String Port {
            get {
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
