using System;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;


namespace ClientTCP.Core
{   

    public class Client
    {
        private string host = "127.0.0.1";
        private int port = 8888;
        private bool isConnected = false;

        static TcpClient client;
        static NetworkStream stream;

        public string UserName { get; set; }
        public string Host 
        { 
            get
            {
                return this.host;
            } 

            set
            {
                if(Regex.IsMatch(value, @"^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$"))
                {
                   this.host = value;
                }
            } 
        }
        public int Port
        {
            get
            {
                return this.port;
            }
            set 
            {
                if(value.ToString().Length == 4)
                {
                    this.port = value;
                }
            }
        }
        public bool IsConnected { get => isConnected;}

        public Client() { }

        public void Connection(string host, int port)
        {
            Host = host;
            Port = port;
        }

        public void Start()
        {            
            client = new TcpClient();
            try
            {
                client.Connect(host, port);
                isConnected = true;
                stream = client.GetStream();

                string message = UserName ?? "Uknown";
                byte[] data = Encoding.Unicode.GetBytes(message);
                stream.Write(data, 0, data.Length);

                Thread receiveThread = new(new ThreadStart(ReceiveMessage));
                receiveThread.Start();
                Console.WriteLine(" > Добро пожаловать, {0}", UserName ?? "Uknown");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void SendMessage(string text)
        {
            string message = text;
            byte[] data = Encoding.Unicode.GetBytes(message);
            stream.Write(data, 0, data.Length);           
        }

        public void ReceiveMessage()
        {
            while (true)
            {
                try
                {
                    byte[] data = new byte[64];
                    StringBuilder builder = new();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    string message = builder.ToString();
                    Console.WriteLine(message);
                    Console.WriteLine(" > ");
                }
                catch
                {
                    Console.WriteLine(" > EROR: Not connection!");
                    Console.ReadLine();
                    Disconnect();
                }
            }
        }

        public void Disconnect()
        {
            if (stream != null)
                stream.Close();
            if (client != null)
                client.Close(); 
        }
    }
}
