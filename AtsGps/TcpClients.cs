using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AtsGps {
    public class TcpClients : ConcurrentDictionary<Guid, TcpTracker> {


        public int TrackersCount
        {
           get {
                int count = 0;
                foreach (TcpTracker tcpTracker2 in this.Values) {
                    if (!String.IsNullOrEmpty(tcpTracker2.Imei)) {
                        count++;
                    }
                }
                return count;
            }
        }

    }
}
