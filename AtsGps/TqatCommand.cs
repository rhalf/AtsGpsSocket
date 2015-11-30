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

        public void Parse (Byte[] data) {
            try {
                String stringData = ASCIIEncoding.UTF8.GetString(data).TrimEnd();
                String[] stringDataArray = stringData.Split(',');
                this.Imei = stringDataArray[0];
                this.Command = stringDataArray[1];
            } catch(Exception exception) {
                throw exception;
            }
        }
    }
}
