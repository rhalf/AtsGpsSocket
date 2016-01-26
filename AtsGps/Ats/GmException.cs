using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AtsGps.Ats {


    public class GmException : Exception {

        public GmException (Int32 code, String imei) {
            set(code, imei, null);
        }
        public GmException (Int32 code, String imei, Object obj) {
            set(code, imei, obj);
        }

        private void set (Int32 code, String imei, Object obj) {
            switch (code) {
                case GmException.WRONG_CHECKSUM:
                    this.Description = "Wrong CheckSum";
                    break;
                case GmException.WRONG_DATA_LENGTH:
                    this.Description = "Wrong DataLength";
                    break;
                case GmException.UNKNOWN_PROTOCOL:
                    this.Description = "Unknown Protocol";
                    break;
                case GmException.WRONG_DATA_FORMAT:
                    this.Description = "Wrong data format";
                    break;
            }

            this.Object = obj;
            this.Imei = imei;
            this.Code = code;
        }

        public Object Object {
            get;
            set;
        }
        public String Imei {
            get;
            set;
        }
        public String Description {
            get;
            set;
        }
        public Int32 Code {
            get;
            set;
        }


        public const Int32 WRONG_CHECKSUM = 0X61010D0A;
        public const Int32 WRONG_DATA_LENGTH = 0X61020D0A;
        public const Int32 WRONG_DATA_FORMAT = 0X61030D0A;

        public const Int32 UNKNOWN_PROTOCOL = 0X62010D0A;



    }
}
