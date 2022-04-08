using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientTCP.Core
{
    public class CommandListener
    {
        public delegate bool Action(string args);
        private readonly Dictionary<string, Action> dictionaryCommand;
        Client client;

        public CommandListener()
        {
            client = new();
            dictionaryCommand = new Dictionary<string, Action>() 
            {
                {"connect", Connection},
                {"setname", SetName},
                {"disconnect", Disconnection},
                {"send", Send},
                {"loglevel", LogLevel},
                {"quit", Quit}
            };
        }

        private bool SetName(string args)
        {
            client.UserName = args;
            return true;
        }

        private bool Quit(string args)
        {
            client.Disconnect();
            Console.WriteLine($" > Aplication exit!");
            Environment.Exit(0);
            return true;
        }
        private bool LogLevel(string args)
        {
            throw new NotImplementedException();
        }
        private bool Send(string args)
        {
            if(client.IsConnected == false)
            {
                Console.WriteLine($" > EROR: not connection");
                return false;
            }    

            client.SendMessage(args);
            Console.WriteLine($" > {args}");
            return true;
        }
        private bool Disconnection(string args)
        {
            client.Disconnect();
            Console.WriteLine($" > Connection terminated: {client.Host} / {client.Port}");
            return true;
        }
        private bool Connection(string args)
        {
            string[] command = args.ToLower().Split(' ');
            if(command.Length == 2)
            {
                client.Connection(command[0], int.Parse(command[1]));
                client.Start();
                Console.WriteLine($" > Connection to MSRG Echo server establishend: / {client.Host} / {client.Port}");
                return true;
            }                
            return false;
        }       

        public bool CommandParse(string command)
        {
            string commandName;

            if (command != null)
            {
                commandName = command.Split(new char[] { ' ' }).First();
            }
            else
            {
                return false;
            }

            foreach(var key in dictionaryCommand)
            {
                if (commandName == key.Key)
                {
                    key.Value(command.Replace(commandName + " ", ""));
                    return true;
                }                                  
            }
            return false;
        }        
    }
}
