using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsGps.Meitrack {

    public enum Meitrack {
        MVT100,
        T1
    }

    public enum EventCode {
        NONE = 0,
        /// <summary>
        /// <para>SOS </para>
        /// </summary>
        INPUT1_ACTIVE = 1,
        /// <summary>
        /// <para>Ignition On: MVT100-MVT340-T322X  </para>
        /// <para>Door Open: MVT380-MVT600-T1-MVT800-T333-T3 </para>
        /// <para>In2 Active Other models</para>
        /// </summary>
        INPUT2_ACTIVE = 2,
        /// <summary>
        /// <para>Ignition On: MVT600-T1&T333-T3  </para>
        /// <para>Door Open: MVT800-T322X  </para>
        /// <para>In3 Active: other models</para>
        /// </summary>
        INPUT3_ACTIVE = 3,
        /// <summary>
        /// <para>Ignition On: MVT380-MVT800  </para>
        /// <para>In4 Active: other models</para>
        /// </summary>
        INPUT4_ACTIVE = 4,
        /// <summary>
        /// <para>In5 Active </para>
        /// </summary>
        INPUT5_ACTIVE = 5,
        /// <summary>
        /// <para>In1 Inactive </para> 
        /// </summary>
        INPUT1_INACTIVE = 9,
        /// <summary>
        /// <para>Ignition Off: MVT100-MVT340-T322X  </para>
        /// <para>Door Close: MVT380-MVT600-T1-MVT800-T333-T3 </para>
        /// <para>In2 Inactive: other models</para>
        /// </summary>
        INPUT2_INACTIVE = 10,
        /// <summary>
        /// <para>Ignition Off: MVT600-T1-T333-T3 </para>
        /// <para>Door Close: MVT800-T322X </para>
        /// <para>In3 Inactive: other models </para>
        /// </summary>
        INPUT3_INACTIVE = 11,
        /// <summary>
        /// <para>Ignition Off: MVT380-MVT800 </para>
        /// <para>In4 Inactive: other models </para>
        /// </summary>
        INPUT4_INACTIVE = 12,
        /// <summary>
        /// <para>In5 Inactive: other models </para>
        /// </summary>
        INPUT5_INACTIVE = 13,
        /// <summary>
        /// <para>Low Battery</para>
        /// </summary>
        LOW_BATTERY = 17,
        /// <summary>
        /// <para>Low Ext-Battery </para>
        /// </summary>
        LOW_EXTERNAL_BATTERY = 18,
        /// <summary>
        /// <para>Speeding  </para>
        /// </summary>
        SPEEDING = 19,
        /// <summary>
        /// <para>Enter Fence N (N means the number of the fence) </para> 
        /// </summary>
        ENTER_GEOFENCE = 20,
        /// <summary>
        /// <para>Exit Fence N (N means the number of the fence)</para>
        /// </summary>
        EXIT_GEOFENCE = 21,
        /// <summary>
        /// <para>Ext-Battery On </para>
        /// <para>Tracker connected: TC68S </para>
        /// </summary>
        EXTERNAL_BATTERY_ON = 22,
        /// <summary>
        /// <para>Ext-Battery Cut </para>
        /// <para>Tracker removed: TC68S</para>
        /// </summary>
        EXTERNAL_BATTERY_CUT = 23,
        /// <summary>
        /// <para>Lose GPS Signal </para>
        /// </summary>
        LOST_GPS_SIGNAL = 24,
        /// <summary>
        /// <para>GPS Recovery</para>
        /// </summary>
        GPS_SIGNAL_RECOVERY = 25,
        /// <summary>
        /// <para>Enter Sleep </para>
        /// </summary>
        ENTER_SLEEP = 26,
        /// <summary>
        /// <para>Exit Sleep </para>
        /// </summary>
        EXIT_SLEEP = 27,
        /// <summary>
        /// <para>GPS Antenna Cut </para>
        /// </summary>
        GPS_ANTENNA_CUT = 28,
        /// <summary>
        /// <para>Power On </para>
        /// </summary>
        DEVICE_REBOOT = 29,
        /// <summary>
        /// <para></para>
        /// </summary>
        IMPACT_OR_FALL = 30,
        /// <summary>
        /// <para>/</para>
        /// </summary>
        HEART_BEAT = 31,
        /// <summary>
        /// <para>Heading Change</para>
        /// </summary>
        HEADING_CHANGE = 32,
        /// <summary>
        /// <para>Distance </para>
        /// </summary>
        DISTANCE_INTERVAL_TRACKING = 33,
        /// <summary>
        /// <para>Now  </para>
        /// </summary>
        REPLY_CURRENT = 34,
        /// <summary>
        /// <para>Interval</para>
        /// </summary>
        TIME_INTERVAL_TRACKING = 35,
        /// <summary>
        /// <para>Tow </para>
        /// </summary>
        TOW = 36,
        /// <summary>
        /// <para>(only for GPRS) </para>
        /// </summary>
        RFID = 37,
        /// <summary>
        /// <para>(only for GPRS) </para>
        /// </summary>
        PICTURE = 39,
        /// <summary>
        /// <para>Stop moving </para>
        /// </summary>
        STOP_MOVING = 41,
        /// <summary>
        /// <para>Start Moving  </para>
        /// </summary>
        START_MOVING = 42,
        /// <summary>
        /// <para>GSM Jammed </para>
        /// </summary>
        GSM_JAMMED = 44,
        /// <summary>
        /// <para>Temp High </para>
        /// </summary>
        TEMPERATURE_HIGH = 50,
        /// <summary>
        /// <para>Temp Low </para>
        /// </summary>
        TEMPERATURE_LOW = 51,
        /// <summary>
        /// <para>Fuel Full </para>
        /// </summary>
        FUEL_FULLED = 52,
        /// <summary>
        /// <para>Fuel Empty </para>
        /// </summary>
        FUEL_EMPTY = 53,
        /// <summary>
        /// <para>Armed </para>
        /// </summary>
        ARMED = 56,
        /// <summary>
        /// <para>Disarmed </para>
        /// </summary>
        DISARMED = 57,
        /// <summary>
        /// <para>Stealing</para>
        /// </summary>
        STEALING = 58,
        /// <summary>
        /// <para>/</para>
        /// </summary>
        PRESS_INPUT1_TO_CALL = 65,
        /// <summary>
        /// <para>/</para>
        /// </summary>
        PRESS_INPUT2_TO_CALL = 66,
        /// <summary>
        /// <para>/</para>
        /// </summary>
        PRESS_INPUT3_TO_CALL = 67,
        /// <summary>
        /// <para>/</para>
        /// </summary>
        PRESS_INPUT4_TO_CALL = 68,
        /// <summary>
        /// <para>/</para>
        /// </summary>
        PRESS_INPUT5_TO_CALL = 69,
        /// <summary>
        /// <para>/</para>
        /// </summary>
        REJECT_INCOMING_CALL = 70,
        /// <summary>
        /// <para>/</para>
        /// </summary>
        GET_LOCATION_BY_CALL = 71,
        /// <summary>
        /// <para>/</para>
        /// </summary>
        AUTO_ANSWER_INCOMING_CALL = 72,
        /// <summary>
        /// <para>/</para>
        /// </summary>
        LISTEN_IN = 73,
        /// <summary>
        /// <para>Fall</para>
        /// </summary>
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
        /// <summary>
        /// <para>Maintenance</para>
        /// </summary>
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

    public enum CommandType {
        /// <summary>
        /// None
        /// </summary>
        NONE = 0x00,
        /// <summary>
        /// Real-Time Location Query (GPRS) – A10 
        /// </summary>
        REAL_TIME_LOCATION = 0xA10,
        /// <summary>
        /// Setting a Heartbeat Packet Reporting Interval (GPRS) – A11 
        /// </summary>
        SET_HEARTBEAT_REPORT = 0xA11,
        TRACK_BY_TIME_INTERVAL = 0xA12,
        SET_DIRECTION_CHANGE_REPORT = 0xA13,
        TRACK_BY_DISTANCE = 0xA14,
        /// <summary>
        /// Setting the Parking Scheduled Tracking Function (GPRS) – A15 
        /// </summary>
        SET_PARKING_SCHEDULE = 0xA15,
        /// <summary>
        /// Enable the Parking Scheduled Tracking Function (GPRS) – A16 
        /// </summary>
        ENABLE_PARKING_SCHEDULE = 0xA16,
        /// <summary>
        /// Enable/Disable the RFID COntrol OUT1 Function -A17 
        /// </summary>
        ENABLE_DISABLE_RFID_CONTROL = 0xA17,
        /// <summary>
        /// 3D-Shake-Wakeup-A19
        /// </summary>
        ACCELEROMETER_SHAKE_WAKE_UP = 0xA19,
        /// <summary>
        /// SETTING GPRS Parameter-A21
        /// </summary>
        SET_GPS_PARAMETER = 0xA21,
        /// <summary>
        /// SETTING DNS Server IP address-A22
        /// </summary>
        SET_DNS_SERVER_IPADDRESS = 0xA22,
        /// <summary>
        /// SETTING Stand BY GPRS Serever-A23
        /// </summary>
        SET_STANDBY_GPRS = 0xA23,
        /// <summary>
        /// SETTING Combined Function Phone Number-A71
        /// </summary>
        SET_COMBINED_FUNCTION_PHONE_NUMBER = 0xA71,
        /// <summary>
        /// SETTING Litsen in Phone Number-A72
        /// </summary>
        Set_LISTEN_IN_PHONENUMBER = 0xA72,
        /// <summary>
        /// Setting the Smart Sleep Mode-A73
        /// </summary>
        SET_SMART_SLEEP_MODE = 0xA73,
        /// <summary>
        /// Automatic Event Report – AAA 
        /// </summary>
        AUTOMATIC_EVENT_REPORT = 0xAAA,
        /// <summary>
        /// Deleting a GPRS Event in the Cache Zone – AFF 
        /// </summary>
        DELETING_GPRS_EVENT_CACHE_ZONE = 0xAFF,
        /// <summary>
        /// Setting a Geo-Fence – B05  
        /// </summary>
        SET_GEOFENCE = 0xB05,
        /// <summary>
        /// Deleting a Geo-Fence – B06  
        /// </summary>
        DELETING_GEOFENCE = 0xB06,
        /// <summary>
        /// Setting the Speeding Alarm Function – B07  
        /// </summary>
        SET_SPEEDING_ALARM_FUNCTION = 0xB07,
        /// <summary>
        /// Setting the Towing Alarm Function – B08   
        /// </summary>
        SET_TOWING_ALARM_FUNCTION = 0xB08,
        /// <summary>
        /// Turning Off the Indicator – B31   
        /// </summary>
        TURNING_OFF_INDICATOR = 0XB31,
        /// <summary>
        /// Setting a Recording Interval – B34    
        /// </summary>
        SET_RECORDING_INTERVAL = 0xB34,
        /// <summary>
        /// Setting the SMS Time Zone – B35 
        /// </summary>
        SET_SMS_TIME_ZONE = 0xB35,
        /// <summary>
        /// Setting the GPRS Time Zone – B36  
        /// </summary>
        SET_GPRS_TIME_ZONE = 0xB36,
        /// <summary>
        /// Checking the Engine First to Determine Tracker Running Status – B60   
        /// </summary>
        CHECK_ENGINE_DETERMINE_TRACKER_RUNNING_STATUS = 0xB60,
        /// <summary>
        /// Setting SMS Event Characters – B91   
        /// </summary>
        SET_SMS_EVENT_CHARACTERS = 0xB91,
        /// <summary>
        /// Setting a GPRS Event Flag – B92   
        /// </summary>
        SET_GPRS_EVENT_FLAG = 0xB92,
        /// <summary>
        /// Reading a GPRS Event Flag – B93
        /// </summary>
        READ_GPRS_EVENT_FLAG = 0xB93,
        /// <summary>
        /// Setting a Photographing Event Flag (MVT600/T1/T333/T3) – B96 
        /// </summary>
        SET_PHOTOGRAPHIC_EVENT_FLAG = 0xB96,
        /// <summary>
        /// Reading a Photographing Event Flag (MVT600/T1/T333) – B97  
        /// </summary>
        READ_PHOTOGRAPHIC_EVENT_FLAG = 0xB97,
        /// <summary>
        /// Setting Event Authorization – B99   
        /// </summary>
        SET_EVENT_AUTHORIZATION = 0xB99,
        /// <summary>
        /// Output Control – C01 
        /// </summary>
        OUTPUT_CONTROL = 0xC01,
        /// <summary>
        /// The GPRS Platform Control Device Sends an SMS – C02 
        /// </summary>
        GPRS_PLATFORM_CONTROL_DEVICE_SEND_SMS = 0xC02,
        /// <summary>
        /// Setting a GPRS Event Transmission Mode – C03  
        /// </summary>
        SET_GPRS_EVENT_TANSMISSION_MODE = 0xC03,
        /// <summary>
        /// GPRS Information Display (LCD Display) – C13 
        /// </summary>
        GPRS_INFORMATION_LCD_DISPLAY = 0xC13,
        /// <summary>
        /// Registering a Temperature Sensor Number (MVT600/T1/T333/T3/MVT800) – C40 
        /// </summary>
        REGISTER_TEMPERATURE_SENSOR_NUMBER = 0xC40,
        /// <summary>
        /// Deleting a Registered Temperature Sensor (MVT600/T1/T333/MVT800/T3) – C41  
        /// </summary>
        DELETE_REGISTERED_TEMPERATURE_SENSOR = 0xC41,
        /// <summary>
        /// Reading the Temperature Sensor SN and Number
        /// <para>(MVT600/T1/T333/MVT800/T3) – C42</para>
        /// </summary>
        READING_TEMPERATURE_SENSOR_SN_NUMBER = 0xC42,
        /// <summary>
        /// Setting a Temperature Value for the High/Low Temperature Alarm and Logical Name - C43
        /// </summary>
        SET_TEMPERATURE_VALUE_HIGH_LOW_TEMP_ALARM_LIGICAL_NAME = 0xC43,
        /// <summary>
        /// Reading Temperature Sensor Parameters (MVT600/T1/ T333/MVT800/T3) – C44 
        /// </summary>
        READ_TEMPERATURE_SENSOR_PARAMETERS = 0xC44,
        /// <summary>
        /// Checking Temperature Sensor Parameters (MVT600/T1/T333/MVT800/T3) – C46   
        /// </summary>
        CHECK_TEMPERATURE_SENSOR_PARAMETER = 0xC46,
        /// <summary>
        /// Setting Fuel Parameters (MVT600/T1/T333/MVT800/T3) – C47 
        /// </summary>
        SET_FUEL_PARAMETERS = 0xC47,
        /// <summary>
        /// Reading Fuel Parameters (MVT600/T1/T333/MVT800/T3) – C48  
        /// </summary>
        READ_FUEL_PARAMETERS = 0xC48,
        /// <summary>
        /// Obtaining a Picture (MVT600/T1/T333/T3) – D00   
        /// </summary>
        OBTAINING_PICTURE = 0xD00,
        /// <summary>
        /// Obtaining the Picture List (MVT600/T1/T333/T3) – D01    
        /// </summary>
        OBTAININ_PICTURE_LIST = 0xD01,
        /// <summary>
        /// Deleting a Picture (MVT600/T1/T333/T3) – D02     
        /// </summary>
        DELETE_PICTURE = 0xD02,
        /// <summary>
        /// Timely Photograghing (MVT600/T1/T333/T3) – D03      
        /// </summary>
        TIMELY_PHOTOGRAPHING = 0xD03,
        /// <summary>
        /// Authorizing an RFID Card (MVT600/T1/T333/T3) – D10       
        /// </summary>
        AUTHORIZING_RFID_CARD = 0xD10,
        /// <summary>
        /// Authorizing RFID Cards in Batches (MVT600/T1/T333/T3) – D11       
        /// </summary>
        AUTHORIZING_RFID_CARDS_BATCHES = 0xD11,
        /// <summary>
        /// Checking Whether a RFID Is Authorized (MVT600/T1/T333/T3) – D12       
        /// </summary>
        CHECK_RFID_AUTHORIZATION = 0xD12,
        /// <summary>
        /// Reading an Authorized RFID (MVT600/T1/T333/T3) – D13        
        /// </summary>
        READ_AUTHORIZED_RFID = 0xD13,
        /// <summary>
        /// Deleting an Authorized RFID (MVT600/T1/T333/T3) – D14         
        /// </summary>
        DELETE_AUTHORIZED_RFID = 0xD14,
        /// <summary>
        /// Deleting Authorized RFIDs in Batches (MVT600/T1/T333/T3) – D15        
        /// </summary>
        DELETE_AUTHORIZED_RFID_BATCHES = 0xD15,
        /// <summary>
        /// Checking the Checksum of the Authorized RFID Database (MVT600/T1/T333/T3) – D16        
        /// </summary>
        CHECK_CHECKSUM_AUTHORISED_RFID_DATABASE = 0xD16,
        /// <summary>
        /// Setting the Maintenance Mileage (TC68S) – D65      
        /// </summary>
        SETTING_MAINTENACE_MILEAGE = 0xD65,
        /// <summary>
        /// Setting Maintenance Time (TC68S) – D66       
        /// </summary>
        SET_MAINTENANCE_TIME = 0xD66,
        /// <summary>
        /// Reading the Tracker Firmware Version and SN – E91       
        /// </summary>
        READ_TRACKER_FIRMWARE_VERSION_SN = 0xE91,
        /// <summary>
        /// Restarting the GSM Module – F01     
        /// </summary>
        READ_GSM_MODULE = 0xF01,
        /// <summary>
        /// Setting the Mileage and Run Time – F08 
        /// </summary>
        SET_MILEAGE_RUN_TIME = 0xF8,
        /// <summary>
        /// Deleting SMS/GPRS Cache Data – F09 
        /// </summary>
        DELETE_SMS_GPRS_CACHE_DATA = 0xF09,
        /// <summary>
        /// Restoring Initial Settings – F11 
        /// </summary>
        RESTORING_INITIAL_SETTINGS = 0xF11
    }


}
