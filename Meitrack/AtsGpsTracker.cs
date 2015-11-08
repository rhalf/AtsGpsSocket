using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AtsGpsTracker.Interface;

namespace AtsGpsTracker {

    public class Mvt100 : GpsDevice {

        public bool Acc { get; set; }
        public int AccuracyHorizontal { get; set; }
        public int AccuracyVertical { get; set; }
        public double Altitude { get; set; }
        public double BatteryVoltage { get; set; }
        public DateTime DateTime { get; set; }
        public double Direction { get; set; }
        public int EngineRpm { get; set; }
        public int EventCode { get; set; }
        public double Fuel { get; set; }
        public int GprsSignal { get; set; }
        public GpsDeviceType GpsDeviceType { get; set; }
        public int GpsSatellite { get; set; }
        public int GpsSignal { get; set; }
        public bool HarshBraking { get; set; }
        public long Imei { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Mileage { get; set; }
        public bool PowerCut { get; set; }
        public double PowerVoltage { get; set; }
        public bool RelayAccCut { get; set; }
        public string RfidTagUid { get; set; }
        public int RunningTime { get; set; }
        public bool Sos { get; set; }
        public int Speed { get; set; }
        public double Temperature { get; set; }

        public void Parse (string data) {
            throw new NotImplementedException();
        }
    }
}
