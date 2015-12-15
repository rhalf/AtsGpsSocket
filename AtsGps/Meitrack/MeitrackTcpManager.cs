using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;

namespace AtsGps.Meitrack {
    public class MeitrackTcpManager : TcpManager {
        public MeitrackTcpManager () : base() { }


        protected override void Communicate (TcpClient tcpClient) {

            lock (base.TcpClient) {
                base.TcpClient.Add(tcpClient);
            }
         

            try {
                while (tcpClient.Connected) {
                    NetworkStream networkStream = tcpClient.GetStream();

                    //------------------------------------------------Receive message
                    Byte[] bufferIn = new Byte[256];
                    Int32 count = networkStream.Read(bufferIn, 0, bufferIn.Length);
                    base.Packets++;
                    base.ReceiveBytes += count;
                    Byte[] incomingBytes = new Byte[count];
                    Array.Copy(bufferIn, incomingBytes, count);


                    Gm gm = Meitrack.ParseGm(incomingBytes);
                    base.triggerDataReceived(gm);
                    //------------------------------------------------Send message if theres any
                    if (this.BufferOut != null) {
                        if (this.BufferOut.ContainsKey(gm.Unit)) {
                            while (this.BufferOut.ContainsKey(gm.Unit)) {
                                String[] command;
                                if (this.BufferOut.TryRemove(gm.Unit, out command)) {
                                    String data = Meitrack.GenerateCommand(command);
                                    send(networkStream, ASCIIEncoding.UTF8.GetBytes(data));
                                    break;
                                }
                            }
                        }
                
                    }
                }
            } catch (GmException gmException) {
                triggerEvent(new Log(gmException.Imei + " : " + gmException.Description + " : " + ASCIIEncoding.UTF8.GetString((Byte[])gmException.Object), LogType.CLIENT));
            } catch (Exception exception) {
                Debug.Write(exception.Message);
            } finally {
               
            }
        
            lock (base.TcpClient) {
                base.TcpClient.Remove(tcpClient);
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
