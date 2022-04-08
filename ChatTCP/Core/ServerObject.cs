using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Text.RegularExpressions;

namespace ChatTCP.Core
{
    public class ServerObject
    {
        private readonly int Port = 8888;
        private readonly string Ip = "127.0.0.1";
        static TcpListener tcpListener;
        readonly List<User> clients = new();

        public ServerObject() { }
        public ServerObject(string Ip, int port)
        {
            try
            {
                if (Regex.IsMatch(Ip, @"^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$"))
                {
                    this.Ip = Ip;
                }
                if (999 < port && port < 9999)
                {
                    Port = port;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("EROR: " + ex.ToString());
            }
        }

        protected internal void AddConnection(User User)
        {
            clients.Add(User);
        }
        protected internal void RemoveConnection(string id)
        {            
            User client = clients.FirstOrDefault(c => c.Id == id);            
            if (client != null)
                clients.Remove(client);
        }        
        protected internal void Listen()
        {
            try
            {
                tcpListener = new TcpListener(IPAddress.Parse(Ip), Port);
                tcpListener.Start();
                Console.WriteLine("Сервер запущен. Ожидание подключений...");
 
                while (true)
                {
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();
 
                    User User = new(tcpClient, this);
                    Thread clientThread = new(new ThreadStart(User.Process));
                    clientThread.Start();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Disconnect();
            }
        } 
        
        protected internal void BroadcastMessage(string message, string id)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i].Id!= id) 
                {
                    clients[i].Stream.Write(data, 0, data.Length);
                }
            }
        }
        
        protected internal void Disconnect()
        {
            tcpListener.Stop();
 
            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].Close(); 
            }
            Environment.Exit(0);         }
    }
}