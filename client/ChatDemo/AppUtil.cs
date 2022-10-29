using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace ChatDemo
{
    class AppUtil
    {
        public static void CloseApp()
        {
            Console.WriteLine("Connection is closed");
            Console.ReadKey();
        }

        public static string getIPAddress()
        {
            string hostName = Dns.GetHostName();
            var host = Dns.GetHostByName(hostName);
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "";
        }
    }
}
