using System;
using System.IO;

namespace Patcher
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			string[] toPatch = new string[args[0]];
			for (int i = 1; i <= args[0]; i++) {
				toPatch[i] = args[i];
			}

			Console.WriteLine ("Begin Patching...");

			for (int i = 0; i < toPatch.Length; i++) {
				Path dir = System.Reflection.Assembly.GetEntryAssembly().Location;
				Console.WriteLine(dir);
			}
		}
	}
}
