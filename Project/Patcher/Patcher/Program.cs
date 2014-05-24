using System;
using System.IO;
using System.Net;
using System.Diagnostics;

namespace Patcher
{
	class MainClass
	{
		public static void Main (string[] args)
		{
            try
            {
                if (args.Length > 0)
                {
                    string localPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\frohdo_Data\";

                    Console.WriteLine(localPath);
                    //string remotePath = @"C:\Users\Dominic\Desktop\test_Data\";
                    WebClient client = new WebClient();

                    Console.WriteLine("Press any key to begin patching.");
                    Console.Read();
                    for (int i = 0; i < args.Length; i++)
                    {
                        try
                        {
                            Console.WriteLine("Patching File: " + args[i]);
                            client.DownloadFile(new Uri(args[i]), localPath + "test" + i);
                            Console.Read();
                        }
                        catch (System.Net.WebException)
                        {
                            Console.WriteLine("Remote File not Found! Maybe yout Network connection or server issues!");
                        }
                    }
                    Console.WriteLine("Patching Done! -- Press any key to restart the game!");
                    Console.Read();
                    Console.Read();
                    try
                    {
                        Process.Start(localPath + @"..\frohdo.exe");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("error: " + ex);
                        Console.Read();
                    }
                }
                Console.Read();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.Read();
            }
		}
	}
}
