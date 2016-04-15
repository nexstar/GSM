using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace sck_Client
{
    public class AsyncSocketListener
    {
        public static ManualResetEvent allDone = new ManualResetEvent(false);
        public static ManualResetEvent SnedDone = new ManualResetEvent(false);

        private static void StartUp(string[] args)
        {
            try
            {
                IPEndPoint ipe = new IPEndPoint(IPAddress.Parse("*1*"), "*2*"); // 請把*1*部分修改為Server的IP,*2*修改為Server Port code
                Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                string ip4 = Helper.GetIP4Address();
                Helper.Phone = args[0];
                Helper.Message = args[1];
                Helper.IP4 = ip4;
                client.BeginConnect(ipe, new AsyncCallback(ConnectCallBack), client);
                allDone.WaitOne();
                client.Shutdown(SocketShutdown.Both);
                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Case1 : " + e.ToString());
            }
        }
        //  First StartUp function ,Try BeginConnect 

        private static void Send(Socket client, string ip4_ph)
        {

            byte[] byteData = Encoding.Unicode.GetBytes(ip4_ph);
            client.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallBack), client);
            SnedDone.Set();
        }
        // Send Message  

        public static void ConnectCallBack(IAsyncResult ar)
        {
            try
            {

                Socket client = (Socket)ar.AsyncState;
                Send(client, Helper.max(Helper.Phone, Helper.Message, Helper.IP4));
                SnedDone.WaitOne();
                client.EndConnect(ar);
            }
            catch (Exception e)
            {
                Console.WriteLine("Case2 : " + e.ToString());
            }
        }
        // second function ,Test remote Server and Send Message 

        public static void SendCallBack(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;
                client.EndSend(ar);
                SnedDone.Set();
                allDone.Set();
                Environment.Exit(0);
            }
            catch (Exception e)
            {
                Console.WriteLine("Case3 : " + e.ToString());
            }
        }
        //cmd was closing when Send finish
        
        public static void Main(string[] args)
        {
            StartUp(args);
        }
        // Main Function
    }
    public class Helper
    {
        public static string Phone;
        public static string Message;
        public static string IP4;

        public static string max(string Phone, string Message, string IP4)
        {
            string PMI = Phone + "_" + Message + "_" + IP4;
            return PMI; //回傳電話_訊息_IP此字串
        }
        
        public static string GetIP4Address()
        {
            IPAddress[] ips = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress i in ips)
            {
                if (i.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return i.ToString();
                }
            }
            return "127.0.0.1";
        }
    }
    // catch local IP4 Address 
}
