using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsGps {

    public enum LogType {
        SERVER_BEGINACCEPT,
        SERVER_COMMUNICATE,
        SERVER,
        COMMAND,
        MVT100,
        T1,
        FM1100
    }

    public class Log {

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


        public Log (string description, LogType logType) {
            this.DateTime = DateTime.Now;
            this.Description = description;
            this.LogType = logType;
        }

    }
}
