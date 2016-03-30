
using AtsGps.Ats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Diagnostics;
using Newtonsoft.Json;

namespace AtsGps.Teltonika {
    public class Teltonika {

        public static bool ParseGm (byte[] raw, ref Gm gm) {

            //00000000000000C50802000001520D315578011EB133890F0E74C40023010809000042160B010002000300B300B4004
            //501F00050041503C800EF000709001BB50010B6000C4225C6180000CD5494CE008E03C700000000F10000A6CE480000
            //0000014E0000000000000000000001520BDD0C30011EB1355B0F0E8061001000A80C000042160B010002000300B300B
            //4004501F00050041504C800EF000709001BB50014B60009422585180000CD53C3CE008E03C700000000F10000A6CE48
            //00000000014E000000000000000002000080C5

            //00000000000000C5
            //08                ->  Unit Identifier     -> 8  
            //02                ->  Record Count        -> 9

            //Record 1
            //000001520D315578  ->  Timestamp           -> 10
            //01                ->  Priority            -> 18            
            //1EB13389          ->  Longitude           -> 19
            //0F0E74C4          ->  Latitude            -> 23
            //0023              ->  Altitude            -> 27
            //0108              ->  Angle               -> 29
            //09                ->  Visible Satellites  -> 31
            //0000              ->  Speed               -> 32

            //IO
            //42
            //16
            //0B
            //01
            //00
            //02
            //00
            //03
            //00
            //B3
            //00
            //B4

            //004501F0
            //00

            //2nd record
            //50041503C800EF000709001BB50010B6000C4225C6180000CD5494CE008E03C700000000F10000A6CE4800000000014
            //E0000000000000000000001520BDD0C30011EB1355B0F0E8061001000A80C000042160B010002000300B300B4004501
            //F00050041504C800EF000709001BB50014B60009422585180000CD53C3CE008E03C700000000F10000A6CE480000000
            //0014E000000000000000002000080C5

            //gm.Latitude
            //gm.Longitude
            //gm.Orientation
            //==============================================================================================

            //throw new Exception(BitConverter.ToString(raw).Replace("-",""));


            byte[] value = null;

            try {
                //value = new byte[] { raw[9] };
                gm.RecordCount = raw[9];

                value = new byte[] { raw[17], raw[16], raw[15], raw[14], raw[13], raw[12], raw[11], raw[10] };
                long timeStamp = BitConverter.ToInt64(value, 0) / 1000;
                gm.TimeStamp = timeStamp.ToString();

                value = new byte[] { raw[22], raw[21], raw[20], raw[19] };
                int longitude = BitConverter.ToInt32(value, 0);
                gm.Longitude = (longitude / 10000000d).ToString();

                value = new byte[] { raw[26], raw[25], raw[24], raw[23] };
                int latitude = BitConverter.ToInt32(value, 0);
                gm.Latitude = (latitude / 10000000d).ToString();

                value = new byte[] { raw[28], raw[27] };
                string altitude = BitConverter.ToInt16(value, 0).ToString();

                value = new byte[] { raw[30], raw[29] };
                gm.Orientation = BitConverter.ToInt16(value, 0).ToString();

                value = new byte[] { raw[31] };
                int gpsSatellites = value[0];

                value = new byte[] { raw[33], raw[32] };
                gm.Speed = BitConverter.ToInt16(value, 0).ToString();

                //==========================IO 
                // events
                value = new byte[] { raw[34] };
                int events = value[0];

                //elements in record
                value = new byte[] { raw[35] };
                int ioElementTotalCount = value[0];

                int indexStart = 0;
                int indexOn = 0;

                List<Io> ioArray1b = new List<Io>();
                List<Io> ioArray2b = new List<Io>();
                List<Io> ioArray4b = new List<Io>();
                List<Io> ioArray8b = new List<Io>();


                #region elements1b
                value = new byte[] { raw[36] };
                int ioElementCount1b = value[0];

                indexStart = 37;
                indexOn = 0;

                for (int index = 0; index < ioElementCount1b; index++) {
                    indexOn = indexStart + (index * 2);
                    ioArray1b.Add(
                        new Io() {
                            Id = raw[indexOn],
                            Value = raw[indexOn + 1]
                        });
                }
                //throw new Exception(JsonConvert.SerializeObject(ioArray1b));
                #endregion  elements1b

                #region elements2b

                value = new byte[] { raw[indexOn + 2] };
                int ioElementCount2b = value[0];

                indexStart = indexOn + 3;
                indexOn = 0;

                for (int index = 0; index < ioElementCount2b; index++) {
                    indexOn = indexStart + (index * 3);
                    ioArray2b.Add(
                        new Io() {
                            Id = raw[indexOn],
                            Value = BitConverter.ToInt16(
                                new byte[] {
                                    raw[indexOn + 2],
                                    raw[indexOn + 1]
                                }, 0)
                        });
                }
                //throw new Exception(JsonConvert.SerializeObject(ioArray2b));
                #endregion elements2b
                #region elements4b

                value = new byte[] { raw[indexOn + 3] };
                int ioElementCount4b = value[0];

                indexStart = indexOn + 4;
                indexOn = 0;

                for (int index = 0; index < ioElementCount4b; index++) {
                    indexOn = indexStart + (index * 5);
                    ioArray4b.Add(
                        new Io() {
                            Id = raw[indexOn],
                            Value = BitConverter.ToInt32(
                                new byte[] {
                                    raw[indexOn + 4],
                                    raw[indexOn + 3],
                                    raw[indexOn + 2],
                                    raw[indexOn + 1]
                                }, 0)
                        });
                }
                //throw new Exception(JsonConvert.SerializeObject(ioArray4b));

                #endregion elements4b

                //===============================
                int acc = 0;
                foreach (Io io in ioArray1b) {
                    //EF - Ignition IO ID 239
                    if (io.Id == 0xEF)
                        acc = io.Value;
                }

                int mileAge = 0;
                foreach (Io io in ioArray4b) {
                    //EF - Odometer IO ID 199 
                    if (io.Id == 0xC7)
                        mileAge = io.Value;
                }
                //===============================

                gm.Mileage = mileAge.ToString();

                StringBuilder sb = new StringBuilder();

                sb.Append((gpsSatellites > 0) ? "1" : "0");
                sb.Append(",35");
                sb.Append("," + gpsSatellites.ToString());
                sb.Append(",26,");
                sb.Append(mileAge.ToString());
                sb.Append(",1.0,4,2156297,427|1|2582|5C1E,");
                sb.Append("0,0,0,0,0,0,0,0,");
                sb.Append("0," + acc.ToString() + ",0,0,0,0,0,0,");
                sb.Append("0004,0000,0000,0847,0000,00000001");

                gm.Data = sb.ToString();

                gm.LastTime = "0";

                StringBuilder sb1 = new StringBuilder();
                foreach (Io io in ioArray1b) {
                    sb1.Append(io.Id.ToString() + ":" + io.Value.ToString() + ",");
                }

                gm.Raw = sb1.ToString();//BitConverter.ToString(raw).Replace("-", "");

            } catch (Exception exception) {
                throw new GmException(GmException.UNKNOWN_PROTOCOL, gm.Unit, exception);
            }
            return true;
        }

        public static String GenerateCommand (String[] command, String identifier) {
            String data = "";
            for (int index = 0; index < command.Length; index++) {
                data += "," + command[index];
            }
            data += "*";

            int length = data.Length + 2 + 2;
            data = "@@" + identifier + length.ToString() + data;
            data += GetCheckSum(data) + "\r\n";
            return data;
        }
        public static String GetCheckSum (String data) {
            Byte[] dataByte = ASCIIEncoding.UTF8.GetBytes(data);
            Int32 dataInt = 0;

            for (int index = 0; index < dataByte.Length; index++) {
                dataInt += dataByte[index];
            }

            String hex = dataInt.ToString("X2");
            hex = hex.Substring((hex.Length - 2));
            return hex;
        }
        public static Boolean CheckSum (Byte[] data) {
            try {
                String stringData = ASCIIEncoding.UTF8.GetString(data, 0, data.Length - 2);
                String[] tempData = stringData.Split('*');

                String checkSumData = tempData[1];

                Byte[] checkSumBytes = ASCIIEncoding.UTF8.GetBytes(tempData[0] + "*");
                Int32 result = 0;
                foreach (Byte byte1 in checkSumBytes) {
                    result += (Int32)byte1;
                }


                int checkCode = Int32.Parse(tempData[1], System.Globalization.NumberStyles.HexNumber);

                Byte[] checkSum1 = BitConverter.GetBytes(checkCode);
                Byte[] checkSum2 = BitConverter.GetBytes(result);

                if (checkSum1[0] == checkSum2[0]) {
                    return true;
                }
                return false;
            } catch {
                return false;
            }
        }
        public static Boolean CheckDataLength (String raw) {
            try {
                String[] arrayRaw = raw.Split(',');
                Int32 dataLength = Int32.Parse(arrayRaw[0].Substring(3));

                arrayRaw[0] = "";


                String data = String.Join(",", arrayRaw, 1, arrayRaw.Length - 1);
                Int32 actualLength = data.Length + 1;
                if (dataLength == actualLength) {
                    return true;
                } else {
                    return false;
                }
            } catch {
                return false;
            }
        }
    }
}
