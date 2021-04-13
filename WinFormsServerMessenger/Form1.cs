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

namespace WinFormsServerMessenger
{
    public partial class Server : Form
    {
        public Server()
        {
            InitializeComponent();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            textBox3.Text += "Server begin work" + Environment.NewLine;

            Task.Run(async () => {
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.IP);

                if (textBoxIP.Text!=IPAddress.Loopback.ToString() && textBoxPort.Text!="11000")
                {
                    IPAddress iPAddress = IPAddress.Parse(textBoxIP.Text);
                    IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, int.Parse(textBoxPort.Text));
                    byte[] buff2 = new byte[1024];
                    EndPoint ep2 = new IPEndPoint(IPAddress.Any, 11000);
                    socket.Bind(iPEndPoint);
                    do
                    {

                        await socket.ReceiveFromAsync(new ArraySegment<byte>(buff2), SocketFlags.None, iPEndPoint).ContinueWith(t => {
                            SocketReceiveFromResult res = t.Result;
                            StringBuilder builder = new StringBuilder();
                            builder.AppendLine($"{res.ReceivedBytes} байт получено из {res.RemoteEndPoint}");
                            builder.AppendLine(Encoding.Unicode.GetString(buff2, 0, res.ReceivedBytes));
                            textBox3.BeginInvoke(new Action<string>(AddText), builder.ToString());
                        });
                    }
                    while (true);

                }
                else
                {


                    IPAddress address = IPAddress.Loopback;
                    IPEndPoint endPoint = new IPEndPoint(address, 11000);
                    byte[] buff = new byte[1024];
                    EndPoint ep = new IPEndPoint(IPAddress.Any, 11000);
                    socket.Bind(endPoint);
                    do
                    {

                        await socket.ReceiveFromAsync(new ArraySegment<byte>(buff), SocketFlags.None, ep).ContinueWith(t => {
                            SocketReceiveFromResult res = t.Result;
                            StringBuilder builder = new StringBuilder();
                            builder.AppendLine($"{res.ReceivedBytes} байт получено из {res.RemoteEndPoint}");
                            builder.AppendLine(Encoding.Unicode.GetString(buff, 0, res.ReceivedBytes));
                            textBox3.BeginInvoke(new Action<string>(AddText), builder.ToString());
                        });
                    }
                    while (true);

                }


               
            });




        }


        private void AddText(string str)
        {
            StringBuilder builder = new StringBuilder(textBox3.Text);
            builder.AppendLine(str);
            textBox3.Text = builder.ToString();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void Server_Load(object sender, EventArgs e)
        {
            textBoxIP.Text = IPAddress.Loopback.ToString();
            textBoxPort.Text = "11000";
        }
    }
}
