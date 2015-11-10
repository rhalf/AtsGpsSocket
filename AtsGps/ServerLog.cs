using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsGps {

    public enum LogType {
        SERVER,
        SERVER_ERROR,
        SERVER_WARNING,
        SERVER_RUNNING,
        SERVER_STOP,
        SERVER_INCOMING_DATA
    }

    public class ServerLog {

        public DateTime DateTime {
            get;
            set;
        }
        public string Description {
            get;
            set;
        }
        public LogType LogType {
            get;
            set;
        }


        public ServerLog (string description, LogType logType) {
            this.DateTime = DateTime.Now;
            this.Description = description;
            this.LogType = logType;
        }

    }
}
