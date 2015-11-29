using System;
using System.Net.Sockets;
using System.Text;

namespace AtsGps.Meitrack {
    public class MeitrackTcpManager : TcpManager {
        public MeitrackTcpManager () : base() { }


        protected override void Communicate (NetworkStream networkStream) {
            //------------------------------------------------Receive message
            Byte[] bufferIn = new Byte[256];
            Int32 count = networkStream.Read(bufferIn, 0, bufferIn.Length);

            base.ReceiveBytes += count;

            try {

                Gm gm = new Gm();
                gm.Parse(bufferIn);
                base.triggerDataReceived(gm);
                send(networkStream, new Byte[] { 0x90, 0x00, 0x0D, 0x0A });

            } catch (GmException gmException) {
                Byte[] code = BitConverter.GetBytes(gmException.Code);
                Array.Reverse(code);
                send(networkStream, code);
            }



        }

        private void send (NetworkStream networkStream, Byte[] bufferOut) {
            //------------------------------------------------Send message
            networkStream.Write(bufferOut, 0, bufferOut.Length);
            networkStream.Flush();
        }
    }
}
