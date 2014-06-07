using System;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Collections.Generic;
using System.Security.AccessControl;

namespace Patcher
{
	class MainClass
	{
		public static void Main (string[] args)
		{
            try
            {
                if (args.Length % 2 != 0) throw new Exception();

                List<DownloadFile> files = new List<DownloadFile>();

                for (int i = 0; i < args.Length; i += 2)
                {
                    DownloadFile f = new DownloadFile(args[i], args[i + 1]);
                    files.Add(f);
                }
                    if (files.Count > 0)
                    {

                        string localPath = "";
#if DEBUG
                        localPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\..\..\..\..\frohdo\";
#else
                    localPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
#endif
                        //Console.WriteLine(localPath);
                        WebClient client = new WebClient();

                        foreach(DownloadFile file in files)
                        {
                            try
                            {
                                Console.WriteLine("Patching File: " + file.uri);

                                byte[] data = client.DownloadData(file.uri);
                                if (!Directory.Exists(Path.GetDirectoryName(localPath + file.relSavePath)))
                                {
                                    Directory.CreateDirectory(Path.GetDirectoryName(localPath + file.relSavePath));
                                }
                                File.WriteAllBytes(localPath + file.relSavePath, data);
                            }
                            catch (System.Net.WebException)
                            {
                                Console.WriteLine("Remote File not Found! Maybe your Network connection or server issues!");
                            }
                        }
                        Console.WriteLine("Patching Done! -- Press any key to restart the game!");
                        try
                        {
                            Process.Start(localPath + @"\frohdo.exe");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                            Console.WriteLine("OH NO. Something went wrong within patching routine. Please try again or contact us via Facebook (PowerPukingPanda)");
                            Console.Read();
                            Console.Read();
                        }
                    }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("OH NO. Something went wrong within patching routine. Please try again or contact us via Facebook (PowerPukingPanda)");
                Console.Read();
                Console.Read();
            }
		}
	}

    class DownloadFile
    {
        public string uri;
        public string relSavePath;

        public DownloadFile(string uri, string relSavePath)
        {
            this.uri = uri;
            this.relSavePath = relSavePath;
        }
    }
}
