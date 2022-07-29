using System.Net;
using System.Net.Sockets;
using System.Text;
namespace Client
{
    public partial class Form1 : Form
    {
        const int PORT = 8088;
        const string IP = "127.0.0.1";
       
        IPEndPoint iPEnd = new IPEndPoint(IPAddress.Parse(IP), PORT);
        Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            if(textBox1.Text != "")
            {
                byte[] data = Encoding.Unicode.GetBytes(textBox1.Text);
                clientSocket.Send(data);
                int bytes = 0;
                byte[] buffer = new byte[1024];
                StringBuilder builder = new StringBuilder();
                do
                {
                    bytes = clientSocket.Receive(buffer);
                    builder.Append(Encoding.Unicode.GetString(buffer, 0, bytes));
                } while (clientSocket.Available > 0);
                label2.Text = builder.ToString();


            }



        }

        private void Form1_Load(object sender, EventArgs e)
        {
            clientSocket.Connect(iPEnd);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
        }
    }
}