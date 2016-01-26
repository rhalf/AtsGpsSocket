using AtsGps;
using AtsGps.Meitrack;
using AtsGps.Ats;


using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using AtsGps.Teltonika;

namespace AtsGpsSocketTool {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        MeitrackTcpManager meitrackTcpManager;
        TqatCommandTcpManager tqatCommandTcpManager;

        ConcurrentDictionary<String, String[]> commands;
        
 
        private void display (Log log) {
            try {
                Dispatcher.Invoke(new Action(() => {
                    dataGridServerLog.Items.Add(log);
                    dataGridServerLog.Items.Refresh();
                }));
            } catch (Exception exception) {
                Debug.Write(exception);
            }
        }
        private void TqatCommandTcpManager_DataReceived (object sender, object data) {
            String[] command = (String[]) data;
            AtsGps.Log log = new Log(command[0] + ":" + command[1], LogType.COMMAND);
            commands.TryAdd(command[0], command);
            display(log);
        }
        private void TqatCommandTcpManager_Event (object sender, Log log) {
            display(log);
        }
        private void MeitrackTcpManager_DataReceived (Object sender, object data) {
            try {
                meitrackTcpManager.Refresh();
                Gm gm = (Gm)data;
                //AtsGps.Log log = new Log(gm.Raw, LogType.MVT100);
                //display(log);
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
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

            IPHostEntry host;
            host = Dns.GetHostEntry(Dns.GetHostName());



            foreach (IPAddress ip in host.AddressList) {
                if (ip.AddressFamily.ToString() == "InterNetwork") {
                    if (ip.AddressFamily == AddressFamily.InterNetwork) {
                        comboBoxIp.Items.Add(ip.ToString());
                    }
                }
            }

            commands = new ConcurrentDictionary<string, string[]>();

            meitrackTcpManager = new MeitrackTcpManager();
            meitrackTcpManager.Event += MeitrackTcpManager_Event;
            meitrackTcpManager.DataReceived += MeitrackTcpManager_DataReceived;
            meitrackTcpManager.BufferOut = commands;

            tqatCommandTcpManager = new TqatCommandTcpManager();
            tqatCommandTcpManager.Event += TqatCommandTcpManager_Event; ;
            tqatCommandTcpManager.DataReceived += TqatCommandTcpManager_DataReceived;
            
        }
        private void buttonClearServerLog_Click (object sender, RoutedEventArgs e) {
            dataGridServerLog.Items.Clear();
        }
        private void listViewServerLog_SelectionChanged (object sender, SelectionChangedEventArgs e) {
            foreach (var item in e.AddedItems.OfType<Log>()) {
                Clipboard.SetText(item.Description);
            }
        }
        private void button_Click (object sender, RoutedEventArgs e) {
            try {

                if (button.Content.ToString() == "Start") {

                    meitrackTcpManager.IpAddress = comboBoxIp.Text;
                    meitrackTcpManager.Port = Int32.Parse(textBoxPort.Text);
                    meitrackTcpManager.Start();
                    meitrackTcpManager.TcpClients = new TcpClients();

                    //tqatCommandTcpManager.IpAddress = comboBoxIp.Text;
                    //tqatCommandTcpManager.Port = 8001;
                    //tqatCommandTcpManager.Start();


                    groupTcpManager.DataContext = meitrackTcpManager;

                    button.Content = "Stop";
                } else {
                    groupTcpManager.DataContext = null;

                    meitrackTcpManager.Stop();

                    tqatCommandTcpManager.Stop();

                    button.Content = "Start";

                }
            } catch (Exception exception) {
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        public MainWindow () {
            InitializeComponent();
        }


    }


}
