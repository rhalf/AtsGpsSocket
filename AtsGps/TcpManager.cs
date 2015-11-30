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
        public delegate void DataReceicedHandler (Object sender, Object data);
        public event DataReceicedHandler DataReceived;
        protected void triggerDataReceived (Object data) {
            this.DataReceived(this, data);
        }
        //----------------------------------------------Initialized
        private Device device;
        private TcpListener tcpListener;
        private IPAddress ipAddress;
        private Int32 port = 0;
        //private Thread threadTcpListener;
        private Int32 packets = 0;
        private Int64 sendBytes = 0;
        private Int64 receiveBytes = 0;
        private Boolean isEnabled;
        private Boolean isActivated;
        //----------------------------------------------Getter/Setter
        public Int32 Id {
            get;
            set;
        }
        public Device Device {
            get { return device; }
            set {
                device = value;
                OnPropertyChanged("Manufacturer");
            }
        }
        public Int32 Packets {
            get {
                return packets;
            }
            set {
                packets = value;
                OnPropertyChanged("Packets");
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
                OnPropertyChanged("SendBytes");
            }
        }
        public Int64 ReceiveBytes {
            get {
                return receiveBytes;
            }
            set {
                receiveBytes = value;
                OnPropertyChanged("ReceiveBytes");
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
        public Boolean IsActivated {
            get {
                return isActivated;
            }
            set {
                isActivated = value;
                OnPropertyChanged("IsActivated");
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
           
            } catch (Exception exception) {
                throw exception;
            }
        }
        public void Start () {
            try {
                if (tcpListener == null) {
                    tcpListener = new TcpListener(this.ipAddress, this.port);
                }
                tcpListener.Start();
                tcpListener.BeginAcceptTcpClient(new AsyncCallback(asyncBeginAccept), tcpListener);

                //if (threadTcpListener == null) {
                //    threadTcpListener = new Thread(threadTcpListenerRun);
                //    threadTcpListener.Start(tcpListener);
                //}

                this.isActivated = true;
                Log serverLog = new Log("Server starts..", LogType.RUNNING);
                Event(this, serverLog);
            } catch (Exception exception) {
                this.isActivated = false;
                Log serverLog = new Log(exception.Message, LogType.ERROR);
                Event(this, serverLog);
                tcpListener.Stop();
                tcpListener = null;
            }
        }
        public void Stop () {
            try {
                if (this.isActivated == true) {
                    tcpListener.Stop();
                }
                this.isActivated = false;
                Log serverLog = new Log("Server stopped..", LogType.STOP);
                Event(this, serverLog);
            } catch (Exception exception) {
                throw exception;
            } finally {
                GC.Collect();
            }
        }
        public void Refresh () {
            OnPropertyChanged("Manufacturer");
            OnPropertyChanged("IpAddress");
            OnPropertyChanged("Port");
            OnPropertyChanged("Packets");
            OnPropertyChanged("SendBytes");
            OnPropertyChanged("ReceiveBytes");
            OnPropertyChanged("BufferIn");
            OnPropertyChanged("BufferOut");
        }

        #region Asynchronous methods
        private void asyncBeginAccept (IAsyncResult iAsyncResult) {
            try {

                TcpListener tcpListener = (TcpListener)iAsyncResult.AsyncState;
                using (TcpClient tcpClient = tcpListener.EndAcceptTcpClient(iAsyncResult)) {
                    this.Packets++;
                    this.Communicate(tcpClient.GetStream());
                    tcpListener.BeginAcceptTcpClient(new AsyncCallback(asyncBeginAccept), tcpListener);
                }
            } catch (Exception exception) {

            } finally {

            }

        }
        #endregion

        //private void threadTcpListenerRun (object state) {
        //    TcpListener tcpListener = (TcpListener)state;
        //    while (true) {
        //        if (this.isActivated == true) {
        //            try {
        //                TcpClient tcpClient = tcpListener.AcceptTcpClient();
        //                ThreadPool.QueueUserWorkItem(new WaitCallback(threadTcpClientRun), tcpClient);
        //            } catch {

        //            }
        //        }
        //    }
        //}

        //private void threadTcpClientRun (object state) {
        //    try {
        //        lock (this) {
        //            this.Packets++;
        //        }

        //        using (TcpClient tcpClient = (TcpClient)state) {
        //            this.Communicate((NetworkStream)tcpClient.GetStream());
        //        }

        //    } catch (Exception exception) {
        //        Log serverLog = new Log(exception.Message, LogType.ERROR);
        //        this.triggerEvent(serverLog);
        //    }
        //}

        protected virtual void Communicate (NetworkStream networkStream) {
            throw new NotImplementedException();
        }

    }
}
