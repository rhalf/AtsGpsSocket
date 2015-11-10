using System;

namespace AtsGps.Meitrack {

    //$$<Data identifier><Data length>,<IMEI>,<Command type>,<Event code>,<(-)Latitude>,<(-)Longitude>,<Date and
    //time>,<Positioning status>,<Number of satellites>,<GSM signal strength>,<Speed>,<Direction>,<Horizontal positioning
    //accuracy>,<Altitude>,<Mileage>,<Run time>,<Base station info>,<I/O port status>,<Analog input value>,<RFID>/<Picture
    //name>/<Geo-fence number>/<Temperature sensor No./<Assisted event info>,<Customized data>,<Protocol version>,<Fuel
    //percentage>,<Temperature sensor 1 value|Temperature sensor 2 value|……Temperature sensor n value><*Checksum>\r\n

    abstract class MeitrackTrackerCommand {
        protected virtual Boolean Parse (Byte[] bytes) {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Indicates the GPRS data packet header from the server to the tracker or
        /// Indicates the GPRS data packet header from the tracker to the server. 
        /// Ex.
        /// The header type is ASCII. (Hexadecimal is represented as 0x40.) Ex. "@@" or 
        /// The header type is ASCII. (Hexadecimal is represented as 0x24.) Ex. "$$"
        /// </summary>
        protected Byte[] DataPacketHeader {
            get;
            set;
        }
        /// <summary>
        /// Has one byte. The type is the ASCII, and its value ranges from 0x41 to 0x7A.
        /// Ex. 
        /// "Q" 
        /// </summary>
        protected Byte DataIdentifier {
            get;
            set;
        }
        /// <summary>
        /// Indicates the length of characters from the first comma (,) to "\r\n". The data length is decimal.
        /// Ex. 25 
        /// </summary>
        protected Int32 DataLength {
            get;
            set;
        }
        /// <summary>
        /// Indicates the tracker IMEI. The number type is ASCII. It has 15 digits generally.
        /// Ex. "353358017784062"
        /// </summary>
        protected String Imei {
            get;
            set;
        }
        /// <summary>
        /// Command type Hexadecimal Ex. AAA
        /// </summary>
        protected CommandType CommandType {
            get;
            set;
        }
        /// <summary>
        /// Event code Decimal Ex. 1
        /// </summary>
        protected EventCode EventCode {
            get;
            set;
        }
        protected Coordinate Coordinate {
            get;
            set;
        }

        protected DateTime DateTime {
            get;
            set;
        }
        protected Boolean PositioningStatus {
            get;
            set;
        }
        protected Int32 GpsSatelliteCount {
            get;
            set;
        }
        /// <summary>
        /// GSM signal strength Value: 0–31 
        /// </summary>
        protected Int32 GsmSignal {
            get;
            set;
        }
        /// <summary>
        /// Speed Unit: km/h 
        /// </summary>
        protected Int32 Speed {
            get;
            set;
        }
        /// <summary>
        /// Indicates the driving direction. The unit is degree. When the value is 0, the direction is north.The value ranges from 0 to 359. 
        /// </summary>
        protected Int32 Direction {
            get;
            set;
        }
        protected Int32 HorizontalPositioningAccuracy {
            get;
            set;
        }
        protected Int32 Altitude {
            get;
            set;
        }
        protected Int32 MileAge {
            get;
            set;
        }
        protected Int32 RunTime {
            get;
            set;
        }

        protected Int32[] BaseStationInfo {
            get;
            set;
        }
        protected Int32[] DigitalIo {
            get;
            set;
        }
        protected Int32[] AnalogIo {
            get;
            set;
        }

    }
}
