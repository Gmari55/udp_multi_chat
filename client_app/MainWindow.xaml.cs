using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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

namespace client_app
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const string serverIp = "127.0.0.1";
        const int serverPort = 3344;

        UdpClient client = new UdpClient();
        IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(serverIp), serverPort);

        private bool isListening = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Listen()
        {
            while (isListening)
            {
                try
                {
                    var response = await client.ReceiveAsync();
                    string message = Encoding.UTF8.GetString(response.Buffer);
                    string time =(DateTime.Now.ToString ("HH:mm"));
                    chatList.Items.Add(time+" "+message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void SendMessage(string message)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(message);
                client.SendAsync(data, data.Length, serverEndPoint);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
        
      
        private void SendBtnClick(object sender, RoutedEventArgs e)
        {
            string text = msgTextBox.Text;
            string name= nametxtbox.Text;

            if (string.IsNullOrWhiteSpace(text)) return;

            SendMessage(name+" "+text);
        }

        private void AboutMenuClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Simple multi chat applicaiton using UDP protocol.", "About");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SendMessage("<leave>");
            isListening = false;
        }

        private void LeaveMenuClck(object sender, RoutedEventArgs e)
        {
            SendMessage("<leave>");
            isListening = false;
        }

        private void JoinMenuClicik(object sender, RoutedEventArgs e)
        {
            SendMessage("<join>");

            if (!isListening)
            {
                isListening = true;
                Listen();
            }
        }
    }
}
