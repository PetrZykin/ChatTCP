using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ClientTCP.Core
{
    internal class Program
    {
        

        static void Main()
        {
            CommandListener commandListener = new();

            Console.WriteLine(" > Введите имя");

            while (true)
            {
                Console.Write(" > ");
                var ex = commandListener.CommandParse(Console.ReadLine());
                if (ex == false)
                {
                    Console.WriteLine("EROR");
                }
            }
        } 
    }
}

