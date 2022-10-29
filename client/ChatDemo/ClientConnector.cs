using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace ChatDemo
{
    class ClientConnector
    {
        Socket handler;

        public ClientConnector(string ip, int port_number)
        {
            // Connect to a Remoete server
            // Get Host IP Address
            IPAddress ipAddress = IPAddress.Parse(ip);
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, port_number);

            // Create a TCP/IP socket
            handler = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            handler.Bind(remoteEP);
            try
            {
                // Connect to Remote Endpoint
                handler.Connect(remoteEP);
                Console.WriteLine("Socket is connecting to {0}", handler.RemoteEndPoint.ToString());
            }
            catch (SocketException se)
            {
                AppUtil.CloseApp();
            }
        }

        public bool checkConnection()
        {
            bool blockingState = handler.Blocking;

            try
            {
                byte[] tmp = new byte[1];
                handler.Blocking = false;
                handler.Send(tmp, 0, 0);
                Console.WriteLine("Connected");
                return true;
            }
            catch (SocketException se)
            {
                if (se.NativeErrorCode.Equals(10035))
                {
                    Console.WriteLine("Staill Connected, but the Send would block");
                }
                else
                {
                    Console.WriteLine("Disconnected: error code {0}!", se.NativeErrorCode);
                }
            }
            finally
            {
                handler.Blocking = blockingState;
            }

            return false;
        }

        public void ReadMessage()
        {
            try
            {
                int bufferSize = 0;
                NetworkStream networkStream = new NetworkStream(handler);
                bufferSize = handler.ReceiveBufferSize;
                byte[] intStream = new byte[bufferSize];
                networkStream.Read(intStream, 0, bufferSize);
                string returnData = Encoding.ASCII.GetString(intStream);
                // var message = ReadToObject(returnData);
                if (returnData.Length > 0)
                {
                    Message message = MessageUtil.ReadToObject(returnData);
                    Console.WriteLine("{0} : {1} {2}", message.getName(), message.getMessage(), AppUtil.getIPAddress());
                }
            }
            catch (ArgumentNullException ane)
            {
                CloseSocket();
            }
            catch (Exception e)
            {
                AppUtil.CloseApp();
            }
        }

        public void WriteMessage(string name, string message)
        {
            Message messge = new Message();
            messge.setName(name);
            messge.setMessage(message);
            if(message.Equals("quit"))
            {
                AppUtil.CloseApp();
                return;
            }
            // Creation of messagge that 
            // we will send to Server 
            string data = MessageUtil.WriteFromObject(messge);
            byte[] outStream = Encoding.ASCII.GetBytes(data);
            int byteSent = handler.Send(outStream);
        }

        public void CloseSocket()
        {
            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
            AppUtil.CloseApp();
        }
    }
}
