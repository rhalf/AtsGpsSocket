using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AtsGps {
    public class TcpClients : ConcurrentDictionary<long, TcpTracker> {


        public int TrackersCount
        {
            get;
            set;
        }

    }
}
