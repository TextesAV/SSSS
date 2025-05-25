using System;
using System.Drawing;
using Server.Connectings;
using Server.Helper;

namespace Server.Messages;

internal class HandlerError
{
	public static void Read(Clients client, object[] objects)
	{
		Console.WriteLine("Error: " + (string)objects[1]);
		Methods.AppendLogs(client.IP, "Error: " + (string)objects[1], Color.Red);
	}
}
