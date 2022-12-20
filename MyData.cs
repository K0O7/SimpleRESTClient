using System;
using System.Net;

namespace MyWebClient
{
    class MyData
    {
        public static void info()
        {
            Console.WriteLine(DateTime.Now);
            Console.WriteLine("Bartosz Walkowiak, 254546");
            Console.WriteLine(Environment.UserName);
            Console.WriteLine(Environment.OSVersion);
            Console.WriteLine(Environment.Version);
            Console.WriteLine(getIPv4Address());
        }

        public static string getIPv4Address()
        {
            IPAddress[] addr = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            return addr[addr.Length - 1].ToString();
        }
    }
}
