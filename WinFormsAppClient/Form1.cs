using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsAppClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void buttonSend_Click(object sender, EventArgs e)
        {

            if (textBoxIP.Text!=IPAddress.Loopback.ToString() && textBoxPort.Text!="11000")
            {


                try
                {
                    IPAddress address = IPAddress.Parse(textBoxIP.Text);
                    IPEndPoint iPEndPoint = new IPEndPoint(address, int.Parse(textBoxPort.Text));
                    Socket sendSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.IP);
                    byte[] buff = Encoding.Unicode.GetBytes(textBoxInfo.Text);
                    await sendSocket.SendToAsync(new ArraySegment<byte>(buff), SocketFlags.None, iPEndPoint);
                    sendSocket.Shutdown(SocketShutdown.Send);
                    sendSocket.Close();
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
              
            }
            else
            {

                IPAddress address = IPAddress.Loopback;
                IPEndPoint endPoint = new IPEndPoint(address, 11000);
                Socket sendSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.IP);
                byte[] buff = Encoding.Unicode.GetBytes(textBoxInfo.Text);
                await sendSocket.SendToAsync(new ArraySegment<byte>(buff), SocketFlags.None, endPoint);
                sendSocket.Shutdown(SocketShutdown.Send);
                sendSocket.Close();
            }


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBoxIP.Text = IPAddress.Loopback.ToString();
            textBoxPort.Text = "11000";
        }
    }
}
