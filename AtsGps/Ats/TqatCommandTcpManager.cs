using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace AtsGps.Ats {
    public class TqatCommandTcpManager : TcpManager {
        public TqatCommandTcpManager () : base() {

        }

        protected override void Communicate (TcpTracker tcpTracker) {
            TcpClient tcpClient = tcpTracker.TcpClient;
            NetworkStream networkStream = tcpClient.GetStream();
            try {
                //------------------------------------------------Receive message
                Byte[] bufferIn = new Byte[256];
                Int32 count = networkStream.Read(bufferIn, 0, bufferIn.Length);
                base.PacketReceived++;
                base.ByteReceived += count;

                String data = ASCIIEncoding.UTF8.GetString(bufferIn, 0, count).TrimEnd();
                String[] command = data.Split(' ');
                base.triggerDataReceived(command);

             

            } catch (Exception exception) {
                send(networkStream, new Byte[] { 0x60, 0x00, 0x0D, 0x0A });
                triggerEvent(new Log(exception.Message, LogType.COMMAND));
            } finally {
                tcpClient.Close();
            }
        }


        private void send (NetworkStream networkStream, Byte[] bufferOut) {
            //------------------------------------------------Send message
            networkStream.Write(bufferOut, 0, bufferOut.Length);
            networkStream.Flush();
            base.ByteSent += bufferOut.Length;
        }
    }
}
