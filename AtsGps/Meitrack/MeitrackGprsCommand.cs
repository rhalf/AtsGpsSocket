using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AtsGps.Meitrack {
    //MEITRACK GPRS Protocol
    //For MT80i/MT88/MT90/MVT100/MVT340 and MVT380/MVT800 /MVT600/T1/TC68/TC68S
    //Version V1.6 Confidential Internal Documentation
    //Created by: Rhalf Wendel D Caacbay
    //Created on: 20151108
    public class MeitrackGprsCommand {
       /// <summary>
        /// Indicates the GPRS data packet header from the server to the tracker or
        /// Indicates the GPRS data packet header from the tracker to the server. 
        /// Ex.
        /// The header type is ASCII. (Hexadecimal is represented as 0x40.) Ex. "@@" or 
        /// The header type is ASCII. (Hexadecimal is represented as 0x24.) Ex. "$$"
        /// </summary>
        protected String DataPacketHeader {
            get;
            set;
        }
        /// <summary>
        /// Has one byte. The type is the ASCII, and its value ranges from 0x41 to 0x7A.
        /// Ex. 
        /// "Q" 
        /// </summary>
        protected Int32 DataIdentifier {
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

        protected MeitrackTrackerCommand MeitrackTrackerCommand {
            get;
            set;
        }

        /// <summary>
        /// $$`Data identifier``Data length`,`IMEI`,`Command type`,`Command``*Checksum`\r\n 
        /// </summary>
        /// <param name="bytes">Raw data from the socket</param>
        /// <param name="length">Length of the raw data from the socket</param>
        /// <returns>Return true if parsed successfully</returns>
        public static MeitrackGprsCommand Parse (Byte[] bytes) {
            MeitrackGprsCommand meitrackGprsCommand = new MeitrackGprsCommand();
            //Header
            meitrackGprsCommand.DataPacketHeader = ASCIIEncoding.UTF8.GetString(new Byte[] { bytes[0], bytes[1] });

            if (!meitrackGprsCommand.DataPacketHeader.Equals("@@") && !meitrackGprsCommand.DataPacketHeader.Equals("$$")) {
                throw new Exception("Protocol format is not Meitrack.");
            }

            try {
                String[] gprsCommand = ASCIIEncoding.UTF8.GetString(bytes).Split(','); 
                //Data Validation
                //-Get CheckSum
                String[] data = ASCIIEncoding.UTF8.GetString(bytes).Split('*');
                Byte[] checkSumByte = new Byte[4];
                Int32 checkSum = Int32.Parse(data[1].Trim(), System.Globalization.NumberStyles.HexNumber);
                //-Formulate Format (@@Q25,353358017784062,A10*)
                String packetTranslated = gprsCommand[0] + "," + gprsCommand[1] + "," + gprsCommand[2] + "*";
                Byte[] packets = ASCIIEncoding.UTF8.GetBytes(packetTranslated);
                Int32 packetsSum = 0;
                for (int index = 0; index < packets.Length; index++) {
                    packetsSum += (Int32)packets[index];
                }

                //if (packetsSum != checkSum) {
                //    throw new Exception("Wrong checkSum. Data is corrupted or altered");
                //}

                //If Data Is Valid
                //DataIdentifier
                meitrackGprsCommand.DataIdentifier = (Int32) bytes[2];
                String dataLength = ASCIIEncoding.UTF8.GetString(new Byte[] { bytes[3], bytes[4], bytes[5] });
                meitrackGprsCommand.DataLength = Int32.Parse(dataLength);
                //Imei
                meitrackGprsCommand.Imei = gprsCommand[1];
                //CommandType
                meitrackGprsCommand.CommandType = (CommandType)Int32.Parse(gprsCommand[2],System.Globalization.NumberStyles.HexNumber);

                string meitrackTrackerCommand = null;
                for (int index = 3; index < gprsCommand.Length; index++) {
                    meitrackTrackerCommand += gprsCommand[index] + ",";
                }

                meitrackTrackerCommand = meitrackTrackerCommand.TrimEnd(',');
                meitrackGprsCommand.MeitrackTrackerCommand = new MeitrackTrackerCommand(meitrackTrackerCommand);

                return meitrackGprsCommand;
            } catch (Exception exception) {
                throw exception;
            }

        }
       
    }
}
