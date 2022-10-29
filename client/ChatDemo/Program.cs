using System;
using System.Threading;

namespace ChatDemo
{
    class Program
    {
        static bool endApp = false;
        static string name = "";
        static int count_first_system_message = 0;
        static ClientConnector connector;
        static void Main(string[] args)
        {
            ExecuteClient();
            return;
        }

        static bool readName()
        {
            Console.WriteLine("Please enter your name");
            name = Console.ReadLine();
            return true;
        }

        static void ExecuteClient()
        {
            try {
                Console.WriteLine("Connecting to server...");
                connector = new ClientConnector(Config.endPoint, Config.port);
                if (connector.checkConnection())
                {
                    if(readName())
                    {
                        Console.WriteLine("Please start chat");
                        Thread ctWrite = new Thread(SendMessage);
                        ctWrite.Start();
                        Thread ctRead = new Thread(GetMessage);
                        ctRead.Start();
                    }
                } 
                else
                {
                    AppUtil.CloseApp();
                }
            }
            catch(Exception e)
            {
                AppUtil.CloseApp();
            }            
        }

        static void GetMessage()
        {
            while(!endApp)
            {
                connector.ReadMessage();

                if(count_first_system_message < 1)
                {
                }
                count_first_system_message += 1;
            }
        }

        static void SendMessage()
        {
            try { 
                while (!endApp)
                {
                    string messag_txt = Console.ReadLine();
                    connector.WriteMessage(name, messag_txt);
                }
            }
            catch (ObjectDisposedException e)
            {
                connector.CloseSocket();
            }
        }
    }
}
