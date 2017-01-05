using AtsGps.Ats;
using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace AtsGps.Meitrack {
    public class MeitrackTcpManager : TcpManager {
        public MeitrackTcpManager () : base() { }


        protected override void Communicate (TcpTracker tcpTracker) {
            int count = 0;
            Gm gm = null;
            Byte[] bufferIn = new Byte[512];
            TcpClient tcpClient = tcpTracker.TcpClient;

            try {

                using (NetworkStream networkStream = tcpClient.GetStream()) {
                    networkStream.ReadTimeout = 1 * 60 * 1000;
                    networkStream.WriteTimeout = 1 * 60 * 1000;

                    do {

                        Array.Clear(bufferIn, 0, bufferIn.Length);
                        if (!networkStream.CanRead) {
                            return;
                        }

                        if (!networkStream.DataAvailable) {

                            TimeSpan timeSpan = DateTime.Now.Subtract(tcpTracker.DateTime);
                            if (timeSpan.Minutes > 1) {
                                return;
                            }

                            Thread.Sleep(1000);
                            continue;
                        }

                        count = networkStream.Read(bufferIn, 0, bufferIn.Length);

                        base.PacketReceived++;
                        base.ByteReceived += count;

                        if (!Meitrack.ParseGm(bufferIn, bufferIn.Length, out gm)) {
                            return;
                        }

                        if (gm != null) {
                            base.triggerDataReceived(gm);
                        } else {
                            return;
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

                        tcpTracker.Imei = gm.Unit;
                        tcpTracker.DataIn = gm.Raw;
                        tcpTracker.DataOut = dataOut;
                        tcpTracker.DateTime = DateTime.Now;

                    } while (tcpTracker.TcpClient.Connected);
                }



            } catch (GmException gmException) {
                if (gmException.Object != null) {
                    triggerEvent(new Log(gmException.Imei + " : " + gmException.Description + " : " + (String)gmException.Object, LogType.MVT100));
                } else {
                    triggerEvent(new Log(gmException.Imei + " : " + gmException.Description, LogType.MVT100));
                }
            } catch (Exception exception) {
                triggerEvent(new Log(exception.Message, LogType.SERVER_COMMUNICATE));
            } finally {
                bufferIn = null;
                gm = null;
            }
        }
        private void send (NetworkStream networkStream, Byte[] bufferOut) {
            //------------------------------------------------Send message
            networkStream.Write(bufferOut, 0, bufferOut.Length);
            networkStream.Flush();
            base.ByteSent += bufferOut.Length;
            base.PacketSent++;
        }

    }
}
