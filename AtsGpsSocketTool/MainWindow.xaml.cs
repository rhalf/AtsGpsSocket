using AtsGps;
using AtsGps.Meitrack;

using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows;
using System.Windows.Controls;



namespace AtsGpsSocketTool {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        MeitractTcpManager meitrackTcpManager;

        private ServerLogs serverLogs;

        public MainWindow () {
            InitializeComponent();
        }

        private void display (ServerLog serverLog) {
            try {
                Dispatcher.Invoke(new Action(() => {
                    serverLogs.Add(serverLog);
                }));
            } catch (Exception exception) {
                Debug.Write(exception);
            }
        }

        private void button_Click (object sender, RoutedEventArgs e) {
            try {

                if (button.Content.ToString() == "Start") {

                    meitrackTcpManager = new MeitractTcpManager(comboBoxIp.Text, int.Parse(textBoxPort.Text));
                    meitrackTcpManager.Event += MeitrackTcpManager_Event;
                    meitrackTcpManager.Start();
                    Timer timerUpdateGui = new Timer(new TimerCallback(updateGui), null, 0, 0);
                    button.Content = "Stop";
                } else {
                    if (meitrackTcpManager != null) {
                        meitrackTcpManager.Stop();
                        meitrackTcpManager = null;
                        button.Content = "Start";
                    }
                }
            } catch (Exception exception) {
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK,MessageBoxImage.Error);
            }

        }

        private void MeitrackTcpManager_Event (ServerLog serverLog) {

            if (serverLog.LogType == LogType.SERVER_INCOMING_DATA) {
                MeitrackGprsCommand meitrackGprsCommand = new MeitrackGprsCommand();
                meitrackGprsCommand.Parse(serverLog.Description);
            } else {
                display(serverLog);

            }


        }

        private void Window_Closing (object sender, System.ComponentModel.CancelEventArgs e) {
            try {
                if (meitrackTcpManager == null)
                    return;
                meitrackTcpManager.Stop();
            } catch {

            }
        }


        private void Window_Loaded (object sender, RoutedEventArgs e) {
            this.Title = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + " - " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

            serverLogs = new ServerLogs();
            this.dataGridServerLog.ItemsSource = serverLogs;

            IPHostEntry host;
            host = Dns.GetHostEntry(Dns.GetHostName());



            foreach (IPAddress ip in host.AddressList) {
                if (ip.AddressFamily.ToString() == "InterNetwork") {
                    if (ip.AddressFamily == AddressFamily.InterNetwork) {
                        comboBoxIp.Items.Add(ip.ToString());
                    }
                }
            }
        }
        private void updateGui (object state) {
            try {
                while (meitrackTcpManager != null) {
                    Dispatcher.Invoke(new Action(() => {
                        labelTcpClientCount.Content = meitrackTcpManager.TcpClientCount;
                    }));
                    Thread.Sleep(1000);
                }
            } catch (Exception exception) {
                ServerLog serverLog = new ServerLog(exception.Message, LogType.SERVER_WARNING);
                display(serverLog);
            }
        }


     

        private void buttonClearServerLog_Click (object sender, RoutedEventArgs e) {
            serverLogs.Clear();
        }

        private void listViewServerLog_SelectionChanged (object sender, SelectionChangedEventArgs e) {
            foreach (var item in e.AddedItems.OfType<ServerLog>()) {
                Clipboard.SetText(item.Description);
            }
        }
    }


}
