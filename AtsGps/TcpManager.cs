using AtsGps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AtsGps {
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

        public delegate void EventHandler (ServerLog serverLog);
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
                ServerLog serverLog = new ServerLog("Server starts", LogType.SERVER_RUNNING);
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
                ServerLog serverLog = new ServerLog("Server stopped", LogType.SERVER_STOP);
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
                Byte[] readBytes = new Byte[256];
                String message = null;
                int index;

                NetworkStream stream = tcpClient.GetStream();

                if (stream == null)
                    return;


                byte[] msg = { 0x01 };

                while (stream.CanRead) {
                    //Receive message
                    index = stream.Read(readBytes, 0, readBytes.Length);

                    if (index == 0)
                        return;

                    message = System.Text.Encoding.ASCII.GetString(readBytes, 0, index);
                    Byte[] data = new Byte[index];
                    Array.Copy(readBytes, data, index);
                    string hex = BitConverter.ToString(data);
                    message = message.Trim();

                    ServerLog serverLog = new ServerLog(message, LogType.SERVER_INCOMING_DATA);
                    Event(serverLog);
                    serverLog = new ServerLog(hex, LogType.SERVER_INCOMING_DATA);
                    Event(serverLog);

                    //Send message
                    if (readBytes[0] == 0x08) {

                        byte[] writeBytes = System.Text.Encoding.ASCII.GetBytes(message);
                        stream.Write(writeBytes, 0, writeBytes.Length);
                    }

                }
            } catch (Exception exception) {
                ServerLog serverLog = new ServerLog("TcpClient : " + exception.Message, LogType.SERVER_ERROR);
                Event(serverLog);
            } finally {
                lock (listTcpClient) {
                    tcpClient.Close();
                    listTcpClient.Remove(tcpClient);
                }
            }
        }

    }
}
