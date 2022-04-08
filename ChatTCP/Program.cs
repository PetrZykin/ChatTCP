using ChatTCP.Core;
using System;
using System.Threading;

namespace ChatTCP
{
    internal class Program
    {
        static ServerObject server;
        static Thread listenThread;
        static void Main(string[] args)
        {
            try
            {
                if (args.Length == 2)
                    server = new ServerObject(args[0], int.Parse(args[1]));
                else
                    server = new ServerObject();
                listenThread = new Thread(new ThreadStart(server.Listen));
                listenThread.Start();
            }
            catch (Exception ex)
            {
                server.Disconnect();
                Console.WriteLine(ex.Message);
            }
        }
    }
}
