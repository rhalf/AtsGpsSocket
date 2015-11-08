using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace AtsGpsSocket {
    public class TcpManager {

        private TcpListener tcpListener;
        private IPAddress IpAddress;

        Thread threadTcpListener;

        public int TcpClientCount {
            get {
                return listTcpClient.Count;
            }
        }
        

        List<object> listTcpClient = new List<object>() { };

        public delegate void EventHandler(ServerLog serverLog);
        public event EventHandler Event;

        public TcpManager (string ip, int port) {
            try {
                IpAddress = IPAddress.Parse(ip);
                tcpListener = new TcpListener(IpAddress, port);
            } catch (Exception exception) {
                throw exception;
            }
        }

        public void Start () {
            try {
                tcpListener.Start();
                threadTcpListener = new Thread(threadTcpListenerRun);
                threadTcpListener.Start(tcpListener);
                ServerLog serverLog = new ServerLog("Server start successfully", LogType.SERVER_RUNNING);
                Event(serverLog);
            } catch (Exception exception) {
                ServerLog serverLog = new ServerLog(exception.Message, LogType.SERVER_ERROR);
                Event(serverLog);
                tcpListener.Stop();
            }
        }

        public void Stop () {
            try {
                foreach (TcpClient tcpClient in listTcpClient) {
                    tcpClient.Close();
                }
                tcpListener.Stop();
            } catch (Exception exception) {
                throw exception;
            } finally {
                ServerLog serverLog = new ServerLog("Server stop successfully", LogType.SERVER_STOP);
                Event(serverLog);
            }
        }

        private void threadTcpListenerRun (object obj) {
            TcpListener tcpListener = (TcpListener)obj;
            try {
                while (true) {
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();
                    ThreadPool.QueueUserWorkItem(new WaitCallback(threadTcpClientRun), tcpClient);
                }
            } catch (Exception exception) {
                ServerLog serverLog = new ServerLog("TcpListener : " + exception.Message, LogType.SERVER_ERROR);
                Event(serverLog);
            } finally {
                foreach (TcpClient tcpClient in listTcpClient) {
                    tcpClient.Close();
                }
                tcpListener.Stop();
            }
        }



        private void threadTcpClientRun (object obj) {
            TcpClient tcpClient = (TcpClient)obj;
            lock (listTcpClient) {
                listTcpClient.Add(tcpClient);
            }
            try {
                Byte[] bytes = new Byte[256];
                String data = null;
                int index;

                NetworkStream stream = tcpClient.GetStream();

                if (stream == null)
                    return;
                //while ((index = stream.Read(bytes, 0, bytes.Length)) != 0) {

                byte[] msg = { 0x01 };

                while (stream.CanRead) {

                    index = stream.Read(bytes, 0, bytes.Length);

                    if (index == 0)
                        return;

                    // Translate data bytes to a ASCII string.
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, index);

                    if (!data.Equals("\r\n")) {
                        ServerLog serverLog = new ServerLog(data, LogType.SERVER_INCOMING_DATA);
                        Event(serverLog);
                    }


                    //byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);
                    int number = (int)bytes[0];
                    ServerLog serverLog1 = new ServerLog(number.ToString(), LogType.SERVER_INCOMING_DATA);
                    Event(serverLog1);
              
                    //if (bytes[0] == 0x08) {
                    //    stream.Write(msg, 0, msg.Length);  
                    //    msg[0]++;
                    //}

                   
                }
            } catch (Exception exception) {
                ServerLog serverLog = new ServerLog("TcpClient : " + exception.Message, LogType.SERVER_ERROR);
                Event(serverLog);
            } finally {
                lock(listTcpClient) {
                    listTcpClient.Remove(tcpClient);
                }
            }
        }
    }
}
