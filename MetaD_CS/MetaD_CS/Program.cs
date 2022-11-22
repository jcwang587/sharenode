using System;
using System.IO;
using System.Text;
using System.Timers;
using Renci.SshNet;
using Renci.SshNet.Common;

namespace ssh
{
    class Program
    {
        public static void Main()
        {
            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(Sshandls);
            aTimer.Interval = 5000;
            aTimer.Enabled = true;

            Console.WriteLine("Press \'q\' to quit the sample.");
            while (Console.Read() != 'q') ;
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
                client.Connect();
                StringBuilder output = new();
                ShellStream stream = client.CreateShellStream("stream", 0, 0, 0, 0, 587);
                stream.WriteLine("shotgun list-jobs");
                stream.Flush();
                Thread.Sleep(5000);
                var reader = new StreamReader(stream);

                Console.WriteLine(reader.ReadLine());
                client.Disconnect();
            }

            if (File.Exists("B:\\projects\\85_Metad_LLTO\\05_LLTO_U1_0K_H001_W005_BIN250\\DONE"))
            {
                Console.WriteLine("The job is done " + DateTime.Now.ToShortTimeString().ToString());

                using SshClient client = new(conInfo);
                client.Connect();
                var ouput = client.RunCommand("cd /home/jiachengwang/projects/85_Metad_LLTO/05_LLTO_U1_0K_H001_W005_BIN250; ls");
                Console.WriteLine(ouput.Result.ToString());
                client.Disconnect();
            }
            else
            {
                Console.WriteLine("The job is running " + DateTime.Now.ToShortTimeString().ToString());
            }
        }
    }
}



