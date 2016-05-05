using AtsGps.Ats;
using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;

namespace AtsGps.Meitrack {
    public class MeitrackTcpManager : TcpManager {
        public MeitrackTcpManager () : base() { }


        protected override void Communicate (TcpTracker tcpTracker) {
            Gm gm = null;
            Byte[] bufferIn = new Byte[256];
            TcpClient tcpClient = tcpTracker.TcpClient;

            try {
                do {
                    NetworkStream networkStream = tcpClient.GetStream();
                    //------------------------------------------------Receive message
                    Array.Clear(bufferIn, 0, bufferIn.Length);
                    Int32 count = networkStream.Read(bufferIn, 0, bufferIn.Length);
                    if (count == 0) {
                        break;
                    }

                    base.PacketReceived++;
                    base.ByteReceived += count;
                    if (!Meitrack.ParseGm(bufferIn, bufferIn.Length, out gm)) {
                        continue;
                    }

                    if (gm != null) {
                        base.triggerDataReceived(gm);
                    }
                    //------------------------------------------------Send message if theres any
                    String dataOut = String.Empty;
                    if (this.BufferOut != null) {
                        if (this.BufferOut.ContainsKey(gm.Unit)) {
                            while (this.BufferOut.ContainsKey(gm.Unit)) {
                                String[] command;
                                if (this.BufferOut.TryRemove(gm.Unit, out command)) {
                                    dataOut = Meitrack.GenerateCommand(command, gm.Identifier);
                                    send(networkStream, ASCIIEncoding.UTF8.GetBytes(dataOut));
                                }
                            }
                        }
                    } 
                    //else {
                    //    send(networkStream, new byte[] { 0x00 });
                    //}


                    tcpTracker.Imei = gm.Unit;
                    tcpTracker.DataIn = gm.Raw;
                    tcpTracker.DataOut = dataOut;
                    tcpTracker.DateTime = DateTime.Now;

                    this.TcpClients.TrackersCount = countTrackers(this.TcpClients);

                } while (NetworkTool.IsConnected(tcpClient));
            } catch (GmException gmException) {
                if (gmException.Object != null) {
                    triggerEvent(new Log(gmException.Imei + " : " + gmException.Description + " : " + (String)gmException.Object, LogType.MVT100));
                } else {
                    triggerEvent(new Log(gmException.Imei + " : " + gmException.Description, LogType.MVT100));
                }
            } catch (Exception exception) {
                triggerEvent(new Log(exception.Message, LogType.SERVER_COMMUNICATE));
            } finally {
                if (!String.IsNullOrEmpty(gm.Unit)) {
                    tcpTracker.DateTime = DateTime.Now;
                    this.TcpClients.TrackersCount = countTrackers(this.TcpClients);
                }
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
