using System;
using System.Collections.Concurrent;
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
        //----------------------------------------------Events
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged (String propertyName) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public delegate void EventHandler (Object sender, Log log);
        public event EventHandler Event;
        protected void triggerEvent (Log log) {
            this.Event(this, log);
        }
        public delegate void DataReceicedHandler (Object sender, Byte[] data);
        public event DataReceicedHandler DataReceived;
        protected void triggerDataReceived (Byte[] data) {
            this.DataReceived(this, data);
        }
        //----------------------------------------------Initialized
        private Manufacturer manufacturer;
        private TcpListener tcpListener;
        private IPAddress ipAddress;
        private Int32 port = 0;
        private Thread threadTcpListener;
        private Int32 connections = 0;
        private Int64 sendBytes = 0;
        private Int64 receivePacketSize = 0;
        private ConcurrentQueue<Object> bufferIn;
        private ConcurrentQueue<Object> bufferOut;
        private Boolean isEnabled;
        //----------------------------------------------Getter/Setter
        public Manufacturer Manufacturer {
            get { return manufacturer; }
            set {
                manufacturer = value;
                OnPropertyChanged("Manufacturer");
            }
        }
        public Int32 Connections {
            get {
                return connections;
            }
            set {
                connections = value;
                OnPropertyChanged("TcpClient");
            }
        }
        public IPAddress IpAddress {
            get {
                return ipAddress;
            }
            set {
                ipAddress = value;
                OnPropertyChanged("IpAddress");
            }
        }
        public Int32 Port {
            get {
                return port;
            }
            set {
                port = value;
                OnPropertyChanged("Port");
            }
        }
        public Int64 SendBytes {
            get {
                return sendBytes;
            }
            set {
                sendBytes = value;
                OnPropertyChanged("SendPacketSize");
            }
        }
        public Int64 ReceiveBytes {
            get {
                return receivePacketSize;
            }
            set {
                receivePacketSize = value;
                OnPropertyChanged("ReceivePacketSize");
            }
        }
        public ConcurrentQueue<Object> BufferIn {
            get {
                return bufferIn;
            }
            set {
                bufferIn = value;
                OnPropertyChanged("InPackets");
            }
        }
        public ConcurrentQueue<Object> BufferOut {
            get {
                return bufferOut;
            }
            set {
                bufferOut = value;
                OnPropertyChanged("OutPackets");
            }
        }
        public Boolean IsEnabled {
            get {
                return isEnabled;
            }
            set {
                isEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }
        //----------------------------------------------Functions
        //public TcpManager (String ip, Int32 port) {
        //    try {
        //        this.IpAddress = ip;
        //        this.Port = port;
        //        tcpListener = new TcpListener(this.ipAddress, port);
        //        TcpClients = new List<TcpClient>();
        //    } catch (Exception exception) {
        //        throw exception;
        //    }
        //}
        public TcpManager () {
            try {
                this.BufferIn = new ConcurrentQueue<object>();
                this.BufferOut = new ConcurrentQueue<object>();
            } catch (Exception exception) {
                throw exception;
            }
        }
        public void Start () {
            try {
                tcpListener = new TcpListener(this.ipAddress, this.port);
                tcpListener.Start();

                threadTcpListener = new Thread(threadTcpListenerRun);
                threadTcpListener.Start(tcpListener);

                Log serverLog = new Log("Server starts..", LogType.RUNNING);
                Event(this, serverLog);
            } catch (Exception exception) {
                Log serverLog = new Log(exception.Message, LogType.ERROR);
                Event(this, serverLog);
                tcpListener.Stop();
                tcpListener = null;
                GC.Collect();
            }
        }
        public void Stop () {
            try {
                tcpListener.Stop();
                tcpListener = null;
                GC.Collect();
            } catch (Exception exception) {
                throw exception;
            } finally {
                Log serverLog = new Log("Server stopped..", LogType.STOP);
                Event(this, serverLog);
            }
        }
        public void Refresh () {
            OnPropertyChanged("Manufacturer");
            OnPropertyChanged("IpAddress");
            OnPropertyChanged("Port");
            OnPropertyChanged("Connections");
            OnPropertyChanged("SendBytes");
            OnPropertyChanged("ReceiveBytes");
            OnPropertyChanged("BufferIn");
            OnPropertyChanged("BufferOut");
        }


        private void threadTcpListenerRun (object state) {
            TcpListener tcpListener = (TcpListener)state;
            while (true) {
                try {
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();
                    ThreadPool.QueueUserWorkItem(new WaitCallback(threadTcpClientRun), tcpClient);
                } catch {
                    return;
                }
            }
        }

        public virtual void threadTcpClientRun (object state) {
            throw new NotImplementedException();
        }


    }
}
