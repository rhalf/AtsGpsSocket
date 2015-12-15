using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AtsGps {
    public class TqatCommand {
        public String Imei {
            get;
            set;
        }
        public String Command {
            get;
            set;
        }

        public String[] Param {
            get;
            set;
        }
      
    }
}
