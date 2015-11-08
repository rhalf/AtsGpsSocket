using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsGpsTracker.Meitrack {
    /// <summary>
    /// MEITRACK GPRS Protocol
    /// For MT80i/MT88/MT90/MVT100/MVT340 and MVT380/MVT800 /MVT600/T1/TC68/TC68S 
    /// Version V1.6 Confidential Internal Documentation 
    /// 
    /// Created by: Rhalf Wendel D Caacbay
    /// Created on: 20151108
    /// </summary
    interface IMeitrackGprs {
        /// <summary>
        /// Indicates the GPRS data packet header from the server to the tracker or
        /// Indicates the GPRS data packet header from the tracker to the server. 
        /// Ex.
        /// The header type is ASCII. (Hexadecimal is represented as 0x40.) Ex. "@@" or 
        /// The header type is ASCII. (Hexadecimal is represented as 0x24.) Ex. "$$"
        /// </summary>
        int DataPacketHeader {
            get;
            set;
        }
        /// <summary>
        /// Has one byte. The type is the ASCII, and its value ranges from 0x41 to 0x7A.
        /// Ex. 
        /// "Q" 
        /// </summary>
        int DataIdentifier {
            get;
            set;
        }
        /// <summary>
        /// Indicates the length of characters from the first comma (,) to "\r\n". The data length is decimal.
        /// Ex. 25 
        /// </summary>
        int DataLength {
            get;
            set;
        }
        /// <summary>
        /// Indicates the tracker IMEI. The number type is ASCII. It has 15 digits generally.
        /// Ex. "353358017784062"
        /// </summary>
        string Imei {
            get;
            set;
        }

        
    }
}
