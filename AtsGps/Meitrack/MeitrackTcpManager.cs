using System;
using System.Net.Sockets;
using System.Text;

namespace AtsGps.Meitrack {
    public class MeitrackTcpManager : TcpManager {
        public MeitrackTcpManager () : base() { }


        public override void threadTcpClientRun (object state) {
            try {
                lock (this) {
                    base.Connections = base.Connections + 1;
                }
                using (TcpClient tcpClient = (TcpClient)state) {
                    //------------------------------------------------BeginLoop
                    do {
                        if (!tcpClient.Connected) {
                            return;
                        }

                        NetworkStream networkStream;

                        networkStream = tcpClient.GetStream();
                        if (networkStream == null)
                            throw new Exception("Network Stream is null.");
                        //------------------------------------------------Receive message
                        if (!networkStream.DataAvailable)
                            continue;

                        Byte[] bytesReceived = new Byte[256];
                        Int32 index = networkStream.Read(bytesReceived, 0, bytesReceived.Length);
                        if (index == 0)
                            return;

                        base.ReceiveBytes += index;

                        Byte[] bytesExact = new Byte[index];
                        Array.Copy(bytesReceived, bytesExact, index);
                        base.triggerDataReceived(bytesExact);

                        //------------------------------------------------Send message
                        if (bytesReceived[0] == 13 && bytesReceived[1] == 10) {
                            Array.Clear(bytesReceived, 0, bytesReceived.Length);
                            continue;
                        }
                        String message = "OK";
                        byte[] writeBytes = System.Text.Encoding.ASCII.GetBytes(message);
                        networkStream.Write(writeBytes, 0, writeBytes.Length);


                        networkStream.Close();
                    } while (true);
                    //------------------------------------------------EndLoop

                }

            } catch (Exception exception) {
                Log serverLog = new Log(exception.Message, LogType.ERROR);
                base.triggerEvent(serverLog);
            } finally {
                lock (this) {
                    base.Connections = base.Connections - 1;
                }

            }
        }
    }
}
