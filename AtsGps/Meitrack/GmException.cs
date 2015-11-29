using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AtsGps.Meitrack {


    public class GmException : Exception {

        public GmException (Int32 code, String imei) {
            switch (code) {
                case GmException.WRONG_CHECKSUM:
                    this.Description = "Wrong CheckSum";
                    break;
                case GmException.WRONG_DATA_LENGTH:
                    this.Description = "Wrong CheckSum";
                    break;
                case GmException.UNKNOWN_PROTOCOL:
                    this.Description = "Unknown Protocol";
                    break;
                case GmException.INVALID_GPS_DATA:
                    this.Description = "Invalid Gps Data";
                    break;
            }
            this.Imei = imei;
            this.Code = code;
        }

        public String Imei {
            get;
            set;
        }
        public  String Description {
            get;
            set;
        }
        public Int32 Code {
            get;
            set;
        }


        public const Int32 WRONG_CHECKSUM = 0X61010D0A;
        public const Int32 WRONG_DATA_LENGTH = 0X61020D0A;
        public const Int32 WRONG_DATA_FORMAT= 0X61030D0A;

        public const Int32 UNKNOWN_PROTOCOL = 0X62010D0A;
        public const Int32 INVALID_GPS_DATA = 0X63010D0A;


    }
}
