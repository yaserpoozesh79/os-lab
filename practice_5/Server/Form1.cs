using System;
using System.Drawing;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using EasyFileTransfer;
using System.IO;

namespace Sample.Server
{

    public partial class Form1 : Form
    {

        private Thread transferServerThread;

        public Form1()
        {
            InitializeComponent();
            label3.Text = GetLocalIPAddress();
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            try
            {
                EftServer server = new("./", Convert.ToInt32(Port.Text));
                this.transferServerThread = new(server.StartServer);
                this.transferServerThread.Start();
                status.ForeColor = Color.Green;
                status.Text = "Online";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void Form1_Leave(object sender, EventArgs e)
        {
            this.Dispose();
            this.transferServerThread.Abort();
            Application.Exit();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void Port_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
