using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsGpsTracker.Meitrack {

    public enum EventCode {
        NONE = 0,                   //0
        INPUT1_ACTIVE = 1,          //SOS PRESSED
        INPUT2_ACTIVE = 2,          //  
        INPUT3_ACTIVE = 3,
        INPUT4_ACTIVE = 4,
        INPUT5_ACTIVE = 5,

        INPUT1_INACTIVE = 9,        //SOS RELEASED
        INPUT2_INACTIVE = 10,
        INPUT3_INACTIVE = 11,
        INPUT4_INACTIVE = 12,
        INPUT5_INACTIVE = 13,

        LOW_BATTERY = 17,
        LOW_EXTERNAL_BATTERY = 18,
        SPEEDING = 19,
        ENTER_GEOFENCE = 20,
        EXIT_GEOFENCE = 21,
        EXTERNAL_BATTERY_ON = 22,
        EXTERNAL_BATTERY_CUT = 23,
        LOST_GPS_SIGNAL = 24,
        GPS_SIGNAL_RECOVERY = 25,
        ENTER_SLEEP = 26,
        EXIT_SLEEP = 27,
        GPS_ANTENNA_CUT = 28,
        DEVICE_REBOOT = 29,
        IMPACT_OR_FALL = 30,
        HEART_BEAT = 31,
        HEADING_CHANGE = 32,
        DISTANCE_INTERVAL_TRACKING = 33,
        REPLY_CURRENT = 34,
        TIME_INTERVAL_TRACKING = 35,
        TOW = 36,
        RFID = 37,
        PICTURE = 39,

        STOP_MOVING = 41,
        START_MOVING = 42,

        GSM_JAMMED = 44,

        TEMPERATURE_HIGH = 50,
        TEMPERATURE_LOW = 51,
        FUEL_FULLED = 52,
        FUEL_EMPTY = 53,

        ARMED = 56,
        DISARMED = 57,

        PRESS_INPUT1_TO_CALL = 65,
        PRESS_INPUT2_TO_CALL = 66,
        PRESS_INPUT3_TO_CALL = 67,
        PRESS_INPUT4_TO_CALL = 68,
        PRESS_INPUT5_TO_CALL = 69,
        REJECT_INCOMING_CALL = 70,
        GET_LOCATION_BY_CALL = 71,
        AUTO_ANSWER_INCOMING_CALL = 72,
        LISTEN_IN = 73,

        FALL = 79,

        FAST_DECELERATE = 129,
        FAST_ACCELERATE = 130,
        RPM_HIGH = 131,
        RPM_RECOVERY_TO_NORMAL = 132,
        IDLE_OVERTIME = 133,
        IDLE_RECOVERY = 134,
        FATIGUE_DRIVING = 135,
        ENOUGH_REST = 136,
        ENGINE_TEMPERATURE_OVERHEAT = 137,
        SPEED_RECOVERY = 138,
        MAINTENANCE_NOTICE = 139,
        ENGINE_FAULT = 140,
        EXHAUST_EMISSION_FAULT = 141,
        HEALTH_ABNORMAL = 142,
        FUEL_LOW = 143,
        IGNITION_ON = 144,
        IGNITION_OFF = 145,
        HALT_TO_START = 146,
        START_TO_HALT = 147
    }

    public class CommandType {
        public static readonly int RealTime = 0xA10;
        public static readonly int SetHeartBeatReporting = 0xA11;
        public static readonly int TrackByTimeInterval = 0xA11;
        public static readonly int SetDirectionChangeReporting = 0xA11;
        public static readonly int TrackByDistance = 0xA11;
        //public static readonly int TrackByDistance = 0xA11;
        //public static readonly int TrackByDistance = 0xA11;
        //public static readonly int TrackByDistance = 0xA11;

    }
}
