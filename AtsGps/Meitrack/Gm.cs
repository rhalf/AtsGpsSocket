using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsGps.Meitrack {
    public class Gm {

        public String Raw {
            get;
            set;
        }
        public String Unit {
            get;
            set;
        }
        public String TimeStamp {
            get;
            set;
        }
        public String Latitude {
            get;
            set;
        }
        public String Longitude {
            get;
            set;
        }

        public String Speed {
            get;
            set;
        }
        public String Orientation {
            get;
            set;
        }
        public String Mileage {
            get;
            set;
        }
        public String Data {
            get;
            set;
        }
        public String LastTime {
            get;
            set;

        }

        //-----------------------------
        public static DateTime toDateTime (double unixTimeStamp) {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp);
            return dateTime.ToLocalTime();
        }
        public static long toUnixTimestamp (DateTime dateTime) {
            long epoch = (dateTime.Ticks - 621355968000000000) / 10000000;
            return epoch;
        }
        public static long toUnixTimestamp (String dateTime) {
            Byte[] bytes = ASCIIEncoding.UTF8.GetBytes(dateTime);


            String year = ASCIIEncoding.UTF8.GetString(new Byte[] { bytes[0], bytes[1] });
            String month = ASCIIEncoding.UTF8.GetString(new Byte[] { bytes[2], bytes[3] });
            String day = ASCIIEncoding.UTF8.GetString(new Byte[] { bytes[4], bytes[5] });
            String hour = ASCIIEncoding.UTF8.GetString(new Byte[] { bytes[6], bytes[7] });
            String minute = ASCIIEncoding.UTF8.GetString(new Byte[] { bytes[8], bytes[9] });
            String second = ASCIIEncoding.UTF8.GetString(new Byte[] { bytes[10], bytes[11] });


            DateTime dateTimeNew = new DateTime(
                Int32.Parse("20" + year),
                Int32.Parse(month),
                Int32.Parse(day),
                Int32.Parse(hour),
                Int32.Parse(minute),
                Int32.Parse(second)
                );

            var epoch = (dateTimeNew - new DateTime(1970, 1, 1)).TotalSeconds;

            return (long)epoch;
        }
     
    }
}
