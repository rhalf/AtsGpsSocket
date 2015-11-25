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

        MeitrackTcpManager meitrackTcpManager;

        private ServerLogs serverLogs;

        public MainWindow () {
            InitializeComponent();
        }
        private void display (Log log) {
            try {
                Dispatcher.Invoke(new Action(() => {
                    serverLogs.Add(log);
                }));
            } catch (Exception exception) {
                Debug.Write(exception);
            }
        }
        private void button_Click (object sender, RoutedEventArgs e) {
            try {

                if (button.Content.ToString() == "Start") {

                    meitrackTcpManager = new MeitrackTcpManager();
                    meitrackTcpManager.IpAddress = IPAddress.Parse(comboBoxIp.Text);
                    meitrackTcpManager.Port = Int32.Parse(textBoxPort.Text);
                    meitrackTcpManager.Event += MeitrackTcpManager_Event;
                    meitrackTcpManager.DataReceived += MeitrackTcpManager_DataReceived;
                    meitrackTcpManager.Start();

                    labelTcpClientCount.DataContext = meitrackTcpManager;

                    button.Content = "Stop";
                } else {
                    meitrackTcpManager.Stop();
                    button.Content = "Start";

                }
            } catch (Exception exception) {
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private void MeitrackTcpManager_DataReceived (Object sender, Byte[] data) {
            try {
          
                MeitrackGprsCommand meitrackGprsCommand = MeitrackGprsCommand.Parse(data);


            } catch (Exception exception) {
                Debug.Write(exception);
            }
        }
        private void MeitrackTcpManager_Event (Object sender, Log log) {
            display(log);
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

        private void buttonClearServerLog_Click (object sender, RoutedEventArgs e) {
            serverLogs.Clear();
        }
        private void listViewServerLog_SelectionChanged (object sender, SelectionChangedEventArgs e) {
            foreach (var item in e.AddedItems.OfType<Log>()) {
                Clipboard.SetText(item.Description);
            }
        }
    }


}
