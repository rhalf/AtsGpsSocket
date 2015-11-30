﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace AtsGps {
    public class TqatCommandTcpManager : TcpManager {
        public TqatCommandTcpManager () : base() {

        }

        protected override void Communicate (NetworkStream networkStream) {
            try {
                //------------------------------------------------Receive message
                Byte[] bufferIn = new Byte[256];
                Int32 count = networkStream.Read(bufferIn, 0, bufferIn.Length);

                base.ReceiveBytes += count;

                TqatCommand tqatCommand = new TqatCommand();
                tqatCommand.Parse(bufferIn);
                base.triggerDataReceived(tqatCommand);
                send(networkStream, new Byte[] { 0x90, 0x00, 0x0D, 0x0A });
            } catch (Exception exception) {
                send(networkStream, new Byte[] { 0x60, 0x00, 0x0D, 0x0A });
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