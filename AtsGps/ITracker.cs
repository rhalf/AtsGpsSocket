using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsGps.Interface {

    public enum GpsDeviceType {
        Mvt100,
        FM1100
    }

    interface GpsDevice {

        void Parse (String data);
        //=========================================================================
        /// <summary>
        /// Get or Set Imei(International Mobile Station Equipment Identity).
        /// Must be unique.
        /// </summary>
        long Imei { get; set; }
        /// <summary>
        /// Get or Set DateTime of the device.
        /// </summary>
        DateTime DateTime { get; set; }

        GpsDeviceType GpsDeviceType { get; set; }
        /// <summary>
        /// Get or Set EventCodes base on each event of certain devices.
        /// </summary>
        int EventCode { get; set; }
        //=========================================================================
        /// <summary>
        /// Get or Set Altitude measured in meter.
        /// </summary>
        Double Altitude { get; set; }
        Double Latitude { get; set; }
        Double Longitude { get; set; }
        /// <summary>
        /// Get or Set Temperature measured in celcius.
        /// </summary>
        Double Temperature { get; set; }
        /// <summary>
        /// Get or Set Direction measured in degrees.
        /// </summary>
        Double Direction { get; set; }
        /// <summary>
        /// Get or Set Direction measured in degrees.
        /// </summary>
        /// <summary>
        /// Get or Set BatteryVoltage measured in voltage.
        /// </summary>
        Double BatteryVoltage { get; set; }
        /// <summary>
        /// Get or Set PowerVoltage(External) measured in voltage.
        /// </summary>
        Double PowerVoltage { get; set; }


        //=========================================================================
        /// <summary>
        /// Get or Set Speed measured in kilometer/s.
        /// </summary>
        int Speed { get; set; }
        /// <summary>
        /// Get or Set Mileage measured in meter.
        /// </summary>
        int Mileage { get; set; }
        /// <summary>
        /// Get or Set GpsSignal. Max = 100, Min = 0.
        /// </summary>
        int GpsSignal { get; set; }
        /// <summary>
        /// Get or Set number of GpsSatellite connected.
        /// </summary>
        int GpsSatellite { get; set; }
        /// <summary>
        /// Get or Set GprsSignal. Max = 100, Min = 0.
        /// </summary>
        int GprsSignal { get; set; }
    
        /// <summary>
        /// Get or Set RunningTime measured in second.
        /// </summary>
        int RunningTime { get; set; }
        /// <summary>
        /// Get or Set AccuracyVertical measured in meter.
        /// </summary>
        int AccuracyVertical { get; set; }
        /// <summary>
        /// Get or Set AccuracyHorizontal measured in meter.
        /// </summary>
        int AccuracyHorizontal { get; set; }
        /// <summary>
        /// Get or Set Acc true = On and false = Off.
        /// </summary>
        Boolean Acc { get; set; }
        /// <summary>
        /// Get or Set Sos true = On and false = Off.
        /// </summary>
        Boolean Sos { get; set; }
        /// <summary>
        /// Get or Set RelayAccCut true = Cut and false = Not Cut.
        /// </summary>
        Boolean RelayAccCut { get; set; }
        /// <summary>
        /// Get or Set PowerCut true = Cut and false = Not Cut.
        /// </summary>
        Boolean PowerCut { get; set; }
        
        /*
        //=========================================================================
        /// <summary>
        /// Get or Set HarshBraking true = Yes and false = No.
        /// </summary>
        Boolean HarshBraking { get; set; }
        /// <summary>
        /// Get or Set RfidTag Serial Number.
        /// </summary>
        string RfidTagUid { get; set; }
        /// <summary>
        /// Get or Set number of EngineRpm.
        /// </summary>  
        int EngineRpm { get; set; }
        /// <summary>
        /// Get or Set Fuel measured in Liters.
        /// </summary>
        Double Fuel { get; set; }
        */
    }
}
