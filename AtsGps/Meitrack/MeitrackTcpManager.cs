using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;

namespace AtsGps.Meitrack {
    public class MeitrackTcpManager : TcpManager {
        public MeitrackTcpManager () : base() { }


        protected override void Communicate (TcpClient tcpClient) {
            Gm gm = null;
            TcpTracker tcpPair = null;
            try {
                do {


                    NetworkStream networkStream = tcpClient.GetStream();

                    //------------------------------------------------Receive message
                    Byte[] bufferIn = new Byte[256];
                    //Array.Clear(bufferIn, 0, bufferIn.Length);

                    Int32 count = networkStream.Read(bufferIn, 0, bufferIn.Length);
                    base.Packets++;
                    base.ReceiveBytes += count;
                    Byte[] incomingBytes = new Byte[count];
                    Array.Copy(bufferIn, incomingBytes, count);


                    if (!Meitrack.ParseGm(incomingBytes, out gm)) {
                        continue;
                    }
                    tcpPair = new TcpTracker() { Imei = gm.Unit, TcpClient = tcpClient };
                    this.TcpTrackers.TryAdd(gm.Unit,tcpPair);


                    base.triggerDataReceived(gm);
                    //------------------------------------------------Send message if theres any
                    if (this.BufferOut != null) {
                        if (this.BufferOut.ContainsKey(gm.Unit)) {
                            if (this.BufferOut.ContainsKey(gm.Unit)) {
                                String[] command;
                                if (this.BufferOut.TryRemove(gm.Unit, out command)) {
                                    String data = Meitrack.GenerateCommand(command, gm.Identifier);
                                    send(networkStream, ASCIIEncoding.UTF8.GetBytes(data));
                                    break;
                                }
                            }
                        }
                    }
                } while (tcpClient.Connected);

               

            } catch (GmException gmException) {
                triggerEvent(new Log(gmException.Imei + " : " + gmException.Description + " : " + ASCIIEncoding.UTF8.GetString((Byte[])gmException.Object), LogType.MVT100));
            } catch (Exception exception) {
                Debug.Write(exception.Message);
            } finally {
                if (!String.IsNullOrEmpty(gm.Unit)) {
                    while (TcpTrackers.ContainsKey(gm.Unit)) {
                        this.TcpTrackers.TryRemove(gm.Unit, out tcpPair);
                    }
                }
            }
        }
        private void send (NetworkStream networkStream, Byte[] bufferOut) {
            //------------------------------------------------Send message
            networkStream.Write(bufferOut, 0, bufferOut.Length);
            networkStream.Flush();
            base.SendBytes += bufferOut.Length;
        }
    }
}
