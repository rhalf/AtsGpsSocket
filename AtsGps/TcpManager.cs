using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;

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
            if (this.Event != null) {
                this.Event(this, log);
            }
        }
        public delegate void DataReceicedHandler (Object sender, Object data);
        public event DataReceicedHandler DataReceived;
        protected void triggerDataReceived (Object data) {
            if (this.DataReceived != null) {
                this.DataReceived(this, data);
            }
        }
        //----------------------------------------------Initialized
        //private Device device;
        private TcpListener tcpListener;

        //private IPAddress ipAddress;
        //private Int32 port = 0;
        //private Int32 packets = 0;
        //private Int64 sendBytes = 0;
        //private Int64 receiveBytes = 0;
        //private Boolean isEnabled;
        //private Boolean isActivated;
        //private ConcurrentDictionary<String, String[]> bufferOut;
        //private ConcurrentQueue<Object> bufferIn;
        //private List<TcpClient> tcpClient;
        //----------------------------------------------Getter/Setter
        public Int64 Id
        {
            get;
            set;
        }
        public Device Device
        {
            get;
            set;
        }
        public long PacketReceived
        {
            get;
            set;
        }
        public long PacketSent
        {
            get;
            set;
        }
        public String IpAddress
        {
            get;
            set;
        }
        public Int32 Port
        {
            get;
            set;
        }
        public long ByteSent
        {
            get;
            set;
        }
        public long ByteReceived
        {
            get;
            set;
        }
        public Boolean IsEnabled
        {
            get;
            set;
        }
        public Boolean IsActivated
        {
            get;
            set;
        }
        public ConcurrentDictionary<String, String[]> BufferOut
        {
            get;
            set;
        }
        public ConcurrentQueue<Object> BufferIn
        {
            get;
            set;
        }
        public TcpClients TcpClients
        {
            get;
            set;
        }
        public TcpManager () {
        }
        public void Start () {
            try {
                this.Id = 0;
                this.TcpClients = new TcpClients();

                if (tcpListener == null) {
                    tcpListener = new TcpListener(new IPEndPoint(IPAddress.Parse(this.IpAddress), this.Port));
                }

                tcpListener.Start();
                tcpListener.BeginAcceptTcpClient(new AsyncCallback(asyncBeginAccept), tcpListener);

                this.IsActivated = true;
                Log serverLog = new Log("Server starts...", LogType.SERVER);
                Event(this, serverLog);
            } catch (Exception exception) {
                this.IsActivated = false;
                Log serverLog = new Log(exception.Message, LogType.SERVER);
                Event(this, serverLog);

                if (tcpListener != null) {
                    this.Stop();
                }
                triggerEvent(new Log(exception.Message, LogType.SERVER));
                throw exception;
            }
        }
        public void Stop () {
            Log serverLog;
            try {
                tcpListener.Stop();
                tcpListener = null;
                Thread.Sleep(500);
                if (this.IsActivated == true) {
                    foreach (TcpTracker tcpTracker in this.TcpClients.Values) {
                        if (tcpTracker.TcpClient.Connected && tcpTracker.TcpClient != null) {
                            tcpTracker.TcpClient.Close();
                        }
                    }
                }
                this.IsActivated = false;
                serverLog = new Log("Server stopped...", LogType.SERVER);
                Event(this, serverLog);
            } catch (Exception exception) {
                triggerEvent(new Log(exception.Message, LogType.SERVER));
                throw exception;
            } finally {
                GC.Collect();
            }
        }
        public void Refresh () {
            if (this.IsActivated) {
                OnPropertyChanged("Manufacturer");
                OnPropertyChanged("IpAddress");
                OnPropertyChanged("Port");
                OnPropertyChanged("Packets");
                OnPropertyChanged("SendBytes");
                OnPropertyChanged("ReceiveBytes");
                OnPropertyChanged("BufferIn");
                OnPropertyChanged("BufferOut");
                OnPropertyChanged("TcpClients");
            }
        }
        #region Asynchronous methods
        private void asyncBeginAccept (IAsyncResult iAsyncResult) {
            try {
                this.Id++;
                Task newTask = Task.Factory.StartNew(() => {
                    try {
                        TcpClient tcpClient = tcpListener.EndAcceptTcpClient(iAsyncResult);
                        tcpClient.ReceiveTimeout = 60000; //1 min
                        tcpClient.SendTimeout = 60000; //1 min


                        TcpTracker tcpTracker = new TcpTracker() {
                            Id = this.Id,
                            Imei = "",
                            TcpClient = tcpClient,
                            DataIn = "",
                            DataOut = "",
                            DateTime = DateTime.Now
                        };

                        while (!this.TcpClients.ContainsKey(tcpTracker.Id)) {
                            this.TcpClients.TryAdd(tcpTracker.Id, tcpTracker);
                        }

                        if (tcpClient.Connected && tcpClient != null) {
                            this.Communicate(tcpTracker);
                        }

                        while (this.TcpClients.ContainsKey(tcpTracker.Id)) {
                            this.TcpClients.TryRemove(tcpTracker.Id, out tcpTracker);
                        }

                    } catch (Exception exception) {
                        if (this.IsActivated) {
                            triggerEvent(new Log(exception.Message, LogType.SERVER_BEGINACCEPT));
                        }
                    }
                }, TaskCreationOptions.LongRunning);

                tcpListener.BeginAcceptTcpClient(new AsyncCallback(asyncBeginAccept), tcpListener);
            } catch (Exception exception) {
                triggerEvent(new Log(exception.Message, LogType.SERVER_BEGINACCEPT));
            }
        }

        private void asyncProcessTcp (object state) {


        }

        protected virtual void Communicate (TcpTracker tcpTracker) {
            throw new NotImplementedException();
        }
        #endregion

    }
}
