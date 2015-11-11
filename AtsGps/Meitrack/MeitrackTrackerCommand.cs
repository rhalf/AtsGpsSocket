using System;
using System.Text;

namespace AtsGps.Meitrack {

    //$$<Data identifier><Data length>,<IMEI>,<Command type>,<Event code>,<(-)Latitude>,<(-)Longitude>,<Date and
    //time>,<Positioning status>,<Number of satellites>,<GSM signal strength>,<Speed>,<Direction>,<Horizontal positioning
    //accuracy>,<Altitude>,<Mileage>,<Run time>,<Base station info>,<I/O port status>,<Analog input value>,<RFID>/<Picture
    //name>/<Geo-fence number>/<Temperature sensor No./<Assisted event info>,<Customized data>,<Protocol version>,<Fuel
    //percentage>,<Temperature sensor 1 value|Temperature sensor 2 value|……Temperature sensor n value><*Checksum>\r\n

    public class MeitrackTrackerCommand {
        public MeitrackTrackerCommand (String meitrackTrackerCommand) {
            try {
                

            } catch (Exception exception) {
                throw exception;
            }
            //this.Parse(meitrackTrackerCommand);
        }
        protected virtual Boolean Parse (String meitrackTrackerCommand) {
            throw new NotImplementedException();
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
