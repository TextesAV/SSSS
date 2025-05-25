using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Server.Helper;

namespace Server.Forms;

public class FormAbout : FormMaterial
{
	private IContainer components;

	private PictureBox pictureBox1;

	private Label label1;

	public FormAbout()
	{
		InitializeComponent();
	}

	private void FormAbout_Load(object sender, EventArgs e)
	{
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Server.Forms.FormAbout));
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.label1 = new System.Windows.Forms.Label();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		base.SuspendLayout();
		this.pictureBox1.Image = (System.Drawing.Image)resources.GetObject("pictureBox1.Image");
		this.pictureBox1.Location = new System.Drawing.Point(82, 78);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(313, 178);
		this.pictureBox1.TabIndex = 0;
		this.pictureBox1.TabStop = false;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 204);
		this.label1.Location = new System.Drawing.Point(27, 279);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(342, 160);
		this.label1.TabIndex = 1;
		this.label1.Text = "Создано спецально для тимы {D3AD T3AM}\r\n\r\n\r\nTelegram: https://t.me/Quqwaq\r\nDiscord: satanazastalina\r\nLolz: Dark_Ratio\r\n\r\nПри подержке Vitral 3.0";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(481, 474);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.pictureBox1);
		base.Name = "FormAbout";
		this.Text = "About";
		base.Load += new System.EventHandler(FormAbout_Load);
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
