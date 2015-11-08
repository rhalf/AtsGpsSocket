using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AtsGpsSocket;

namespace AtsGpsSocket {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        TcpManager tcpManager;



        public MainWindow () {
            InitializeComponent();
        }

        private void display (ServerLog serverLog) {
            try {
                Dispatcher.Invoke(new Action(() => {
                    listViewServerLog.Items.Add(serverLog);
                }));
            } catch (Exception exception) {
                Debug.Write(exception);
            }
        }

        private void button_Click (object sender, RoutedEventArgs e) {
            try {

                if (button.Content.ToString() == "Start") {

                    tcpManager = new TcpManager(comboBoxIp.Text, int.Parse(textBoxPort.Text));
                    tcpManager.Event += TcpManage_Event;
                    tcpManager.Start();
                    Timer timerUpdateGui = new Timer(new TimerCallback(updateGui), null, 0, 0);
                    button.Content = "Stop";
                } else {
                    if (tcpManager != null) {
                        tcpManager.Stop();
                        tcpManager = null;
                        button.Content = "Start";
                    }
                }
            } catch (Exception exception) {
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK,MessageBoxImage.Error);
            }

        }





        private void Window_Closing (object sender, System.ComponentModel.CancelEventArgs e) {
            try {
                if (tcpManager == null)
                    return;
                tcpManager.Stop();
            } catch {

            }
        }


        private void Window_Loaded (object sender, RoutedEventArgs e) {
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
                while (tcpManager != null) {
                    Dispatcher.Invoke(new Action(() => {
                        labelTcpClientCount.Content = tcpManager.TcpClientCount;
                    }));
                    Thread.Sleep(1000);
                }
            } catch (Exception exception) {
                ServerLog serverLog = new ServerLog(exception.Message, LogType.SERVER_WARNING);
                display(serverLog);
            }
        }


        private void TcpManage_Event (ServerLog serverLog) {
            display(serverLog);
        }

        private void buttonClearServerLog_Click (object sender, RoutedEventArgs e) {
            listViewServerLog.Items.Clear();
        }
    }


}
