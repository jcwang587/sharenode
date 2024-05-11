using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Timers;
using Renci.SshNet;
using Renci.SshNet.Common;
using Sharprompt;
using Figgle;
using System.Security.Cryptography.X509Certificates;

namespace ssh
{
    class Program
    {
        public static class Common 
        {
            private static string name = "jiachengwang";

            public static string Name
            {
                get { return name; }
                set { name = value; }
            }
        }


        public static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine(FiggleFonts.Standard.Render("UNITY STATUS"));

            var username = Prompt.Input<string>("Enter your username:");
            Console.WriteLine($"Hello, {username}!");

            var password = Prompt.Password("Enter your password:");

            bool running = true;
            while (running)
            {
                var function = Prompt.Select("Select function", new[] { "Show System Info", "Quit" });
                switch (function)
                {
                    case "Show System Info":
                        ShowSystemInfo(username, password);
                        break;
                    case "Quit":
                        running = false;
                        break;
                }
            }
        }

        static void ShowSystemInfo(string username, string password)
        {
            try
            {
                var connectionInfo = new ConnectionInfo("baigroup.duckdns.org", 22, username,
                    new AuthenticationMethod[]
                    {
                        new PasswordAuthenticationMethod(username, password)
                    });

                using var client = new SshClient(connectionInfo);
                client.Connect();
                if (client.IsConnected)
                {
                    Console.WriteLine("Successfully connected to the server.");

                    var cmd = client.CreateCommand("ls -l");
                    var result = cmd.Execute();
                    Console.WriteLine("System Info:\n" + result);
                }
                else
                {
                    Console.WriteLine("Connection failed.");
                }
                client.Disconnect();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}

