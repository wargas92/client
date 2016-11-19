using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.ObjectModel;

namespace PDS_progetto_client
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Socket s;
        ObservableCollection<Process> items;
        GridView myGridView;
        ListView lvProcess;
        IPAddress ip;
        int port;
        Thread t;
        private object _stocksLock = new object();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void onClickEvent(object sender, RoutedEventArgs e)
        {
            int n;
            


            label.Visibility = System.Windows.Visibility.Hidden;

            if (!int.TryParse(ServerPort.Text, out port) || !IPAddress.TryParse(ServerIP.Text, out ip))
            {
                label.Content = "Errore Parametri";
                label.Width = double.NaN;
                label.Visibility = System.Windows.Visibility.Visible;
                myGrid.UpdateLayout();

            }
            else {
                
                ProcessView();

            }
   



        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key >= Key.A && e.Key <= Key.Z) || e.Key==Key.Space || e.Key == Key.Back || e.Key == Key.Delete || e.Key == Key.CapsLock )
                return;
            TextBox t = (TextBox)sender;
            if (t.Text.Length == 0) t.Text = e.Key.ToString();
             else t.Text = t.Text.ToString() + "+" + e.Key;
             t.SelectionStart = t.Text.Length;
            


        }
        private void Receiving(){

            byte[] bufferR = new byte[1025];

            IPEndPoint ipe = new IPEndPoint(ip, port);
            s = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            string str = "";
            bool bR = true;

            s.Connect(ipe);
            while (true)
            {
                while (bR)
                {
                    bR = (s.Receive(bufferR, 0, 1024, 0) == 1024);
                    str = str + Encoding.UTF8.GetString(bufferR);
                }
                List<string> processes = str.Split('\n').ToList<string>();
                for (int i = 0; i < processes.Count - 1; i++)
                {
                    List<string> process = processes[i].Split(',').ToList<string>();
                        items.Add(new Process() { Pid = int.Parse(process[0]), Name = process[1], cName = process.Count >= 3 ? process[2] : "", wName = process.Count >= 4 ? process[3] : "" });
            
                    //items.Add(new Process() { Pid = 123, Name = "zzzzzzzz", wName ="aaaaaaaaaaaa", cName ="aaaaaaaaaaaaaaa" });
                }
           
            }

        }
        private void ProcessView() {

                sPanel.Children.Clear();
                items = new ObservableCollection<Process>();
    

    

            BindingOperations.EnableCollectionSynchronization(items, _stocksLock);
        myGridView = new GridView();
                myGridView.AllowsColumnReorder = true;
                myGridView.ColumnHeaderToolTip = "Process Information";
                GridViewColumn gvc1 = new GridViewColumn();
                gvc1.DisplayMemberBinding = new Binding("Name");
                gvc1.Header = "Name";
                gvc1.Width = double.NaN;
                myGridView.Columns.Add(gvc1);
                GridViewColumn gvc2 = new GridViewColumn();
                gvc2.DisplayMemberBinding = new Binding("Pid");
                gvc2.Header = "Pid";
                gvc2.Width = double.NaN;
                myGridView.Columns.Add(gvc2);
                GridViewColumn gvc3 = new GridViewColumn();
                gvc3.DisplayMemberBinding = new Binding("wName");
                gvc3.Header = "Window Title";
                gvc3.Width = double.NaN;
                myGridView.Columns.Add(gvc3);
                GridViewColumn gvc4 = new GridViewColumn();
                gvc4.DisplayMemberBinding = new Binding("cName");
                gvc4.Header = "ClassName";
                gvc4.Width = double.NaN;
                myGridView.Columns.Add(gvc4);

                
                lvProcess = new ListView();

                lvProcess.ItemsSource = items;
                lvProcess.Width = sPanel.Width;
                lvProcess.View = myGridView;
                sPanel.Children.Add(lvProcess);
                TextBox tb = new TextBox();
                tb.Name = "CommandtoSend";
                tb.Width = double.NaN;
                tb.KeyUp += Window_KeyDown;
                sPanel.Children.Add(tb);
                Button b = new Button();
                b.Name = "SendtoServer";
                b.Width = double.NaN;
                b.Content = "Click To send";

                sPanel.Children.Add(b);
            
           t = new Thread(new ThreadStart(()=> { Receiving(); }));
            t.Start();



            }
         



        private void MainWindows_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            sPanel.UpdateLayout();
            
        }

        private void ServerPort_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
