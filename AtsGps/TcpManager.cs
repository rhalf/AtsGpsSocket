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
            if (this.Event!= null) {
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
        //public Int32 Id {
        //    get;
        //    set;
        //}



        public Device Device
        {
            get;
            set;
        }
        public Int32 Packets
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
        public Int64 SendBytes
        {
            get;
            set;
        }
        public Int64 ReceiveBytes
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

        public List<TcpClient> TcpClients
        {
            get;
            set;
        }
        public ConcurrentDictionary<String, TcpTracker> TcpTrackers
        {
            get;
            set;
        }

        public TcpManager () {
        }
        public void Start () {
            try {
                this.TcpClients = new List<TcpClient>();
               

                if (tcpListener == null) {
                    tcpListener = new TcpListener(IPAddress.Parse(this.IpAddress), this.Port);
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
                    tcpListener.Stop();
                    tcpListener = null;
                }
                triggerEvent(new Log(exception.Message, LogType.SERVER));
                throw exception;
            }
        }
        public void Stop () {
            Log serverLog;
            try {
                if (this.IsActivated == true) {
                    lock(this.TcpClients) {
                        foreach (TcpClient tcpClient in this.TcpClients) {
                            if (tcpClient.Connected && tcpClient != null) {
                                tcpClient.Close();
                            }
                        }
                    }
                    tcpListener.Stop();
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
                OnPropertyChanged("TcpTrackers");
            }
        }

        #region Asynchronous methods
        private void asyncBeginAccept (IAsyncResult iAsyncResult) {
            Task.Factory.StartNew(() => {
                try {
                    TcpListener tcpListener = (TcpListener)iAsyncResult.AsyncState;
                    TcpClient tcpClient = tcpListener.EndAcceptTcpClient(iAsyncResult);

                    lock (this.TcpClients) {
                        this.TcpClients.Add(tcpClient);
                    }

                    if (tcpClient.Connected && tcpClient != null) {
                        this.Communicate(tcpClient);
                    }

                    lock (this.TcpClients) {
                        this.TcpClients.Remove(tcpClient);
                    }
                } catch (Exception exception) {
                    if (this.IsActivated) {
                        triggerEvent(new Log(exception.Message, LogType.SERVER));
                    }
                } finally {

                }
            });

            try {
                if (this.IsActivated && tcpListener.Server != null) {
                    if (tcpListener.Server.IsBound)
                        tcpListener.BeginAcceptTcpClient(new AsyncCallback(asyncBeginAccept), tcpListener);
                }
            }catch (Exception exception) {
                triggerEvent(new Log(exception.Message, LogType.SERVER));
            }
        }


        #endregion



        protected virtual void Communicate (TcpClient tcpClient) {
            throw new NotImplementedException();
        }

    }
}
