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
        public static void Main()
        {
            // Prefer UTF-8 as the output encoding
            Console.OutputEncoding = Encoding.UTF8;

            // ASCII banner
            Console.WriteLine(FiggleFonts.Standard.Render("SemiAuto Shotgun"));

            var name = Prompt.Input<string>("What's your name?");
            Console.WriteLine($"Hello, {name}!");
            var secret = Prompt.Password("Type new password");
            Console.WriteLine("Password OK");

            // Function list
            var function = Prompt.Select("Select function", new[] { "Seattle", "London", "Tokyo" });
            Console.WriteLine($"Function {function} is selected");

            // Set up a timer
            System.Timers.Timer aTimer = new();
            aTimer.Elapsed += new ElapsedEventHandler(Sshandls);
            aTimer.Interval = 5000;
            aTimer.Enabled = true;

            Console.WriteLine("Press \'q & Enter\' to quit.");
            while (Console.Read() != 'q') ;
        }

        static void ConnectGroup(string username, string password)
        {
            // Set connect information
            ConnectionInfo conInfo = new("baigroup.duckdns.org", 22, username, new AuthenticationMethod[]
            {
               new PasswordAuthenticationMethod(username, password)
            });
        }


        static void Sshandls(object? source, ElapsedEventArgs e)
        {
            // Set connect information
            ConnectionInfo conInfo = new("baigroup.duckdns.org", 22, "jiachengwang", new AuthenticationMethod[]
            {
               new PasswordAuthenticationMethod("jiachengwang", "wjc19950910")
            });

            using (SshClient client = new(conInfo))
            {
                // Connect to server
                client.Connect();

                // Create a shell stream for output
                StringBuilder output = new();
                ShellStream stream = client.CreateShellStream("stream", 0, 0, 0, 0, 587);

                // Run command shotgun list-jobs
                stream.WriteLine("shotgun list-jobs");
                stream.Flush();
                Thread.Sleep(5000);
                var reader = new StreamReader(stream);

                string? line;
                int read_init = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains("Found")) { read_init = 1; }

                    if (read_init==1)
                    {
                        string content = reader.ReadToEnd();
                        string trimmed = content[..content.LastIndexOf("\r\n")];
                        Console.WriteLine(trimmed);
                        break;
                    }
                }
                client.Disconnect();
            }

            // Condition if the job is done
            if (File.Exists("B:\\projects\\85_Metad_LLTO\\05_LLTO_U1_0K_H001_W005_BIN250\\DONE"))
            {
                Console.WriteLine("The job is done " + DateTime.Now.ToShortTimeString().ToString());

                using SshClient client = new(conInfo);
                client.Connect();
                var ouput = client.RunCommand("cd /home/jiachengwang/projects/85_Metad_LLTO/05_LLTO_U1_0K_H001_W005_BIN250; ls");
                Console.WriteLine(ouput.Result.ToString());
                client.Disconnect();
            }

            // Condition if the job is on going
            else
            {
                Console.WriteLine("The job is running " + DateTime.Now.ToShortTimeString().ToString());
            }
        }
    }
}


