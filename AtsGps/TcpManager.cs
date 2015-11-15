﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AtsGps {
    public class TcpManager : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged (String propertyName) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private TcpListener tcpListener;
        private IPAddress IpAddress;

        Thread threadTcpListener;



        public List<object> listTcpClient {
            get;
            set;
        }



        public delegate void EventHandler (ServerLog serverLog);
        public event EventHandler Event;
        protected void triggerEvent(ServerLog serverLog) {
            this.Event(serverLog);
        }

        public delegate void DataReceicedHandler (Byte[] data);
        public event DataReceicedHandler DataReceived;
        protected void triggerDataReceived(Byte[] data) {
            this.DataReceived(data);
        }


        Int32 tcpClientCount = 0;
        public int TcpClientCount {
            get {
                return tcpClientCount;
            }
            set {
                tcpClientCount = value;
                OnPropertyChanged("TcpClientCount");
            }
        }

        public TcpManager (string ip, int port) {
            try {
                IpAddress = IPAddress.Parse(ip);
                tcpListener = new TcpListener(IpAddress, port);
                listTcpClient = new List<object>();
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
                ServerLog serverLog = new ServerLog(exception.Message, LogType.SERVER_ERROR);
                Event(serverLog);
            } finally {
                foreach (TcpClient tcpClient in listTcpClient) {
                    tcpClient.Close();
                }
                tcpListener.Stop();
            }
        }

        public virtual void threadTcpClientRun (object obj) {
            throw new NotImplementedException();
        }

    }
}
