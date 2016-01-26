using AtsGps.Ats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AtsGps.Meitrack {
    public class Meitrack {
        public static bool ParseGm (byte[] raw, int length, out Gm gm) {
            String packet = null;
            String[] packetArray = null;
            gm = new Gm();

            if (raw[0] == 36 && raw[1] == 36) {
                #region Validate
                //Device    1           2   3       4       5           6       7  8 9 10 11 12  13  14         15          16         17           18                19    20
                //@@Q25,353358017784062,A10,35,24.963245,51.598091,151111064417,A,10,13,0,28,0.9,0,20164489,15062657,427|1|238E|6375,0200,0013|0000|0000|0A8B|0842,00000039,*6A

                //1,35,9,24,1450984,1.1,8,484707,427 | 2 | 008F | 2DE9,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0003,0000,0000,0A68,0432,00000001
                try {
                    packet = ASCIIEncoding.UTF8.GetString(raw, 0, length).Trim(new char[] { '\0', '\r', '\n' });
                    packetArray = packet.Split(',');
                    gm.Raw = packet;
                    gm.Unit = packetArray[1];
                    gm.Identifier = packet.Substring(2, 1);
                    if (packetArray[3].Contains("OK")) {
                        return false;
                    }
                } catch (Exception exception) {
                    throw new GmException(GmException.UNKNOWN_PROTOCOL, exception.Message, packet);
                }


                //if (!Meitrack.CheckSum(ASCIIEncoding.UTF8.GetBytes(packet))) {
                //    throw new GmException(GmException.WRONG_CHECKSUM, gm.Unit, packet);
                //}

                //if (!Meitrack.CheckDataLength(packet)) {
                //    throw new GmException(GmException.WRONG_DATA_LENGTH, gm.Unit, packet);
                //}
                #endregion



                try {
                    gm.TimeStamp = Gm.toUnixTimestamp(packetArray[6]).ToString();
                    gm.Latitude = packetArray[4];
                    gm.Longitude = packetArray[5];
                    gm.Speed = packetArray[10];
                    gm.Orientation = packetArray[11];
                    gm.Mileage = Math.Round((Double.Parse(packetArray[14]) / 1000d), 2).ToString();
                } catch {
                    throw new GmException(GmException.UNKNOWN_PROTOCOL, "Parsing Error on Primary Properties.", packet);
                }


                //1,			            //GPS signal status
                //35,			            //Event code(Decimal)
                //11,			            //Number of satellites(Decimal)
                //26,			            //GSM signal status(Decimal)
                //17160691, 		        //Mileage(Decimal)unit: meter
                //0.7, 			            //hpos accuracy(Decimal)
                //18, 			            //Altitude(Decimal)unit: meter
                //18661240, 		        //Run time(Decimal)unit: second
                //427|2|0078|283F, 	        //Base station information(binary|binary|hex|hex)           (8)
                //==============================================0200
                //0,0,0,0,0,0,0,0,          //Io port lowbyte (low bit start from left)                 (9)
                //0,1,0,0,0,0,0,0,          //Io port lowbyte (low bit start from left)                 (17)
                //==============================================
                //000B,0000,0000,0A6E,0434, //Analog input value                                        (25)
                //00000001 		            //System mark    

                //gm_id = 10682
                //gm_time = 1428992007(1)
                //gm_lat = 25.891435(2)
                //gm_lng = 51.508528(3)
                //gm_speed = 0(4)
                //gm_ori = 117(5)
                //gm_mileage = 3627.813(6)
                //gm_data = 1,35,11,26,6596078,1.0,4,2156297,427|1|2582|5C1E,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0004,0000,0000,0847,0000,00000001(7)

                StringBuilder sb = new StringBuilder();
                try {
                    if (packetArray[7].Equals("V")) {
                        sb.Append("0,");
                    } else {
                        sb.Append("1,");
                    }

                    sb.Append(packetArray[3] + ","); //Event code(Decimal)
                    sb.Append(packetArray[8] + ","); //Number of satellites(Decimal)
                    sb.Append(packetArray[9] + ","); //GSM signal status(Decimal)
                    sb.Append(packetArray[14] + ","); //Mileage(Decimal)unit: meter
                    sb.Append(packetArray[12] + ","); //hpos accuracy(Decimal)
                    sb.Append(packetArray[13] + ","); //Altitude(Decimal)unit: meter
                    sb.Append(packetArray[15] + ","); //Run time(Decimal)unit: second
                    sb.Append(packetArray[16] + ","); //Base station information(binary|binary|hex|hex)           (8)
                } catch {
                    throw new GmException(GmException.UNKNOWN_PROTOCOL, "Parsing Error on Secondary Properties.", packet);
                }

                try {
                    Int32 io = Int32.Parse(packetArray[17], System.Globalization.NumberStyles.HexNumber);

                    for (int index = 0; index < 16; index++) {
                        if (((io >> (index)) & 1) == 1) {
                            sb.Append("1,");
                        } else {
                            sb.Append("0,");
                        }
                    }
                } catch {
                    for (int index = 0; index < 16; index++) {
                        sb.Append("0,");
                    }
                }
                String[] analog = packetArray[18].Split('|');
                sb.Append((String.IsNullOrEmpty(analog[0]) ? "0" : analog[0]) + ",");
                sb.Append((String.IsNullOrEmpty(analog[1]) ? "0" : analog[1]) + ",");
                sb.Append((String.IsNullOrEmpty(analog[2]) ? "0" : analog[2]) + ",");
                sb.Append((String.IsNullOrEmpty(analog[3]) ? "0" : analog[3]) + ",");
                sb.Append((String.IsNullOrEmpty(analog[4]) ? "0" : analog[4]) + ",");
                sb.Append("00000001");


                gm.Data = sb.ToString();
                gm.LastTime = "0";
                return true;

            } else {
                return false;
            }
        }
        public static TqatCommand ParseCommand (byte[] raw) {
            TqatCommand tqatCommand = new AtsGps.TqatCommand();
            try {
                String stringData = ASCIIEncoding.UTF8.GetString(raw).TrimEnd();
                String[] stringDataArray = stringData.Split(' ');
                tqatCommand.Imei = stringDataArray[0];
                tqatCommand.Command = stringDataArray[1];
                String param = String.Join(" ", stringDataArray, 2, stringDataArray.Length - 2);
                tqatCommand.Param = param.Split(' ');
                return tqatCommand;
            } catch (Exception exception) {
                throw exception;
            }
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
                String stringData = ASCIIEncoding.UTF8.GetString(data, 0, data.Length);
                String[] tempData = stringData.Split('*');

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
                String data = String.Join(",", arrayRaw, 0, arrayRaw.Length);
                Int32 actualLength = data.Length + 2;
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
