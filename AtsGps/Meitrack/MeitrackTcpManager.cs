using System;
using System.Net.Sockets;

namespace AtsGps.Meitrack {
    public class MeitractTcpManager : TcpManager {
        public MeitractTcpManager (string ip, int port) : base(ip, port) { }


        public override void threadTcpClientRun (object obj) {
            TcpClient tcpClient = (TcpClient)obj;
            lock (base.listTcpClient) {
                listTcpClient.Add(tcpClient);
                base.TcpClientCount = listTcpClient.Count;
            }
            try {
                Byte[] bytesReceived = new Byte[256];
       
                int index = 0;

                NetworkStream stream = tcpClient.GetStream();

                if (stream == null)
                    return;


                byte[] msg = { 0x01 };

                while (stream.CanRead) {
                    //Receive message
                    index = stream.Read(bytesReceived, 0, bytesReceived.Length);

                    if (index == 0)
                        return;

                    Byte[] bytesExact = new Byte[index];
                    Array.Copy(bytesReceived, bytesExact, index);
                    base.triggerDataReceived(bytesExact);
                    //base.triggerEvent(new ServerLog("Data Received", LogType.SERVER_INCOMING_DATA));

                    //message = System.Text.Encoding.ASCII.GetString(readBytes, 0, index);
                    //Byte[] data = new Byte[index];
                    //Array.Copy(readBytes, data, index);
                    //string hex = BitConverter.ToString(data);
                    //message = message.Trim();

                    //serverLog = new ServerLog(message, LogType.SERVER_INCOMING_DATA);
                    //base.OnEvent(serverLog);

                    //Send message
                    //if (readBytes[0] == 0x08) {

                    //    byte[] writeBytes = System.Text.Encoding.ASCII.GetBytes(message);
                    //    stream.Write(writeBytes, 0, writeBytes.Length);
                    //}

                }
            } catch (Exception exception) {
                ServerLog serverLog = new ServerLog(exception.Message, LogType.SERVER_ERROR);
                base.triggerEvent(serverLog);
            } finally {
                lock (listTcpClient) {
                    listTcpClient.Remove(tcpClient);
                    base.TcpClientCount = listTcpClient.Count;
                }
                tcpClient.Close();
            }
        }
    }
}
