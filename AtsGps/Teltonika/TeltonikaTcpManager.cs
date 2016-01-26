using AtsGps;
using AtsGps.Ats;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace AtsGps.Teltonika {
    public class TeltonikaTcpManager : TcpManager {
        public TeltonikaTcpManager () : base() { }

        protected override void Communicate (TcpTracker tcpTracker) {
            Gm gm = null;
            Byte[] bufferIn = new Byte[256];
            TcpClient tcpClient = tcpTracker.TcpClient;
            String dataOut = String.Empty;


            try {

                NetworkStream networkStream = tcpClient.GetStream();

                //------------------------------------------------Receive message
                //Communication 1
                Array.Clear(bufferIn, 0, bufferIn.Length);
                Int32 count = networkStream.Read(bufferIn, 0, bufferIn.Length);
                base.PacketReceived++;
                base.ByteReceived += count;
                Byte[] incomingBytes = new Byte[count];
                Array.Copy(bufferIn, incomingBytes, count);

                if (incomingBytes[0] != 0x00 || incomingBytes[1] != 0x0f) {
                    throw new GmException(GmException.UNKNOWN_PROTOCOL, "");
                }

                gm = new Gm();
                gm.Unit = ASCIIEncoding.UTF8.GetString(incomingBytes, 2, incomingBytes.Length - 2);
                send(networkStream, new byte[] { 0x01 });
                dataOut = "0x01";

                //Communication 2
                Array.Clear(bufferIn, 0, bufferIn.Length);
                count = networkStream.Read(bufferIn, 0, bufferIn.Length);
                base.PacketReceived++;
                base.ByteReceived += count;
                incomingBytes = new Byte[count];
                Array.Copy(bufferIn, incomingBytes, count);


                if (!Teltonika.ParseGm(incomingBytes, ref gm)) {
                    return;
                }

                //Communication 3
                send(networkStream, new byte[] { 0x02, 0x00, 0x00, 0x00 });
                dataOut = "0x00000002";



                //------------------------------------------------Send message if theres any

                //if (this.BufferOut != null) {
                //    if (this.BufferOut.ContainsKey(gm.Unit)) {
                //        if (this.BufferOut.ContainsKey(gm.Unit)) {
                //            String[] command;
                //            if (this.BufferOut.TryRemove(gm.Unit, out command)) {
                //                dataOut = Teltonika.GenerateCommand(command, gm.Identifier);
                //                send(networkStream, ASCIIEncoding.UTF8.GetBytes(dataOut));
                //            }
                //        }
                //    }
                //}

                base.triggerEvent(new Log(BitConverter.ToString(incomingBytes).Replace("-", ""), LogType.FM1100));
                base.triggerDataReceived(gm);
                tcpTracker.Imei = gm.Unit;
                tcpTracker.DataIn = gm.Raw;
                tcpTracker.DataOut = dataOut;
                tcpTracker.DateTime = DateTime.Now;

                this.TcpClients.TrackersCount = countTrackers(this.TcpClients);
                tcpClient.Close();



            } catch (GmException gmException) {
                triggerEvent(new Log(gmException.Imei + " : " + gmException.Description, LogType.FM1100));
            } catch (Exception exception) {
                triggerEvent(new Log(exception.Message, LogType.SERVER));
            } finally {
                if (!String.IsNullOrEmpty(gm.Unit)) {
                    tcpTracker.Imei = "";
                    tcpTracker.DataIn = "";
                    tcpTracker.DataOut = "";
                    tcpTracker.DateTime = DateTime.Now;
                    this.TcpClients.TrackersCount = countTrackers(this.TcpClients);
                }
                tcpClient.Close();
            }
        }
        private void send (NetworkStream networkStream, Byte[] bufferOut) {
            //------------------------------------------------Send message
            networkStream.Write(bufferOut, 0, bufferOut.Length);
            networkStream.Flush();
            base.ByteSent += bufferOut.Length;
            base.PacketSent++;
        }
        private int countTrackers (TcpClients tcpClients) {
            int count = 0;
            foreach (TcpTracker tcpTracker2 in tcpClients.Values) {
                if (!String.IsNullOrEmpty(tcpTracker2.Imei)) {
                    count++;
                }
            }
            return count;
        }
    }
}


