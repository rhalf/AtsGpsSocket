using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace AtsGps {
    public class MeitractTcpManager : TcpManager {
        public MeitractTcpManager (string ip, int port) : base(ip, port) { }
    }
}
