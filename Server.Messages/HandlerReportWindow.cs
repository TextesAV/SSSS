using Server.Connectings;

namespace Server.Messages;

internal class HandlerReportWindow
{
	public static void Read(Clients client, object[] objects)
	{
		Clients[] array = Program.form.ClientsAll();
		client.Hwid = (string)objects[1];
		Clients[] array2 = array;
		foreach (Clients clients in array2)
		{
			if (clients.Hwid == client.Hwid)
			{
				clients.ReportWindow = client;
				return;
			}
		}
		client.Disconnect();
	}
}
