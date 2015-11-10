using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AtsGps.Meitrack {
    public class MeitrackGprsCommand {
        //MEITRACK GPRS Protocol
        //For MT80i/MT88/MT90/MVT100/MVT340 and MVT380/MVT800 /MVT600/T1/TC68/TC68S
        //Version V1.6 Confidential Internal Documentation

        //Created by: Rhalf Wendel D Caacbay
        //Created on: 20151108

        public virtual Boolean Parse (Byte[] bytes) {
            try {
                this.DataPacketHeader = new Byte[] { bytes[0], bytes[1] };
                this.DataIdentifier = bytes[2];

                String dataLength = ASCIIEncoding.Unicode.GetString(new Byte[] { bytes[3], bytes[4] });
                this.DataLength = Int32.Parse(dataLength);


                String[] meitrackGprsCommand = ASCIIEncoding.Unicode.GetString(bytes).Split(',');

                this.Imei = meitrackGprsCommand[1];
                this.CommandType = (CommandType) Int32.Parse(meitrackGprsCommand[2]);


                return true;
            } catch (Exception exception) {
                Debug.Write(exception);
                return false;
            }

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
    }
}
