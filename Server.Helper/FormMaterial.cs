using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;
using Newtonsoft.Json;
using Server.Data;

namespace Server.Helper;

public class FormMaterial : MaterialForm
{
	public static Color PrimaryColor;

	private IContainer components;

	public FormMaterial()
	{
		InitializeComponent();
		MaterialSkinManager instance = MaterialSkinManager.Instance;
		instance.ColorSchemeChanged += delegate
		{
			Refresh();
		};
		if (File.Exists("local\\Settings.json"))
		{
			GetColorScheme(JsonConvert.DeserializeObject<Settings>(File.ReadAllText("local\\Settings.json")).Style, instance);
		}
		else
		{
			GetColorScheme(Randomizer.random.Next(15), instance);
		}
		instance.Theme = MaterialSkinManager.Themes.LIGHT;
	}

	public static void GetColorScheme(int index, MaterialSkinManager materialSkinManager)
	{
		switch (index % 16)
		{
		case 0:
			PrimaryColor = ToColor(9315498);
			break;
		case 1:
			PrimaryColor = ToColor(2001125);
			break;
		case 2:
			PrimaryColor = ToColor(15022389);
			break;
		case 3:
			PrimaryColor = ToColor(4431943);
			break;
		case 4:
			PrimaryColor = ToColor(16485376);
			break;
		case 5:
			PrimaryColor = ToColor(16635957);
			break;
		case 6:
			PrimaryColor = ToColor(6174129);
			break;
		case 7:
			PrimaryColor = ToColor(35195);
			break;
		case 8:
			PrimaryColor = ToColor(44225);
			break;
		case 9:
			PrimaryColor = ToColor(236517);
			break;
		case 10:
			PrimaryColor = ToColor(12634675);
			break;
		case 11:
			PrimaryColor = ToColor(3754411);
			break;
		case 12:
			PrimaryColor = ToColor(16011550);
			break;
		case 13:
			PrimaryColor = ToColor(16757504);
			break;
		case 14:
			PrimaryColor = ToColor(14162784);
			break;
		case 15:
			PrimaryColor = ToColor(8172354);
			break;
		default:
			PrimaryColor = ToColor(9315498);
			break;
		}
		switch (index % 16)
		{
		case 0:
			materialSkinManager.ColorScheme = new ColorScheme(Primary.Purple600, Primary.Purple700, Primary.Purple800, Accent.Purple200, TextShade.WHITE);
			break;
		case 1:
			materialSkinManager.ColorScheme = new ColorScheme(Primary.Blue600, Primary.Blue700, Primary.Blue800, Accent.Blue200, TextShade.WHITE);
			break;
		case 2:
			materialSkinManager.ColorScheme = new ColorScheme(Primary.Red600, Primary.Red700, Primary.Red800, Accent.Red200, TextShade.WHITE);
			break;
		case 3:
			materialSkinManager.ColorScheme = new ColorScheme(Primary.Green600, Primary.Green700, Primary.Green800, Accent.Green200, TextShade.WHITE);
			break;
		case 4:
			materialSkinManager.ColorScheme = new ColorScheme(Primary.Orange600, Primary.Orange700, Primary.Orange800, Accent.Orange200, TextShade.WHITE);
			break;
		case 5:
			materialSkinManager.ColorScheme = new ColorScheme(Primary.Yellow600, Primary.Yellow700, Primary.Yellow800, Accent.Yellow200, TextShade.WHITE);
			break;
		case 6:
			materialSkinManager.ColorScheme = new ColorScheme(Primary.DeepPurple600, Primary.DeepPurple700, Primary.DeepPurple800, Accent.DeepPurple200, TextShade.WHITE);
			break;
		case 7:
			materialSkinManager.ColorScheme = new ColorScheme(Primary.Teal600, Primary.Teal700, Primary.Teal800, Accent.Teal200, TextShade.WHITE);
			break;
		case 8:
			materialSkinManager.ColorScheme = new ColorScheme(Primary.Cyan600, Primary.Cyan700, Primary.Cyan800, Accent.Cyan200, TextShade.WHITE);
			break;
		case 9:
			materialSkinManager.ColorScheme = new ColorScheme(Primary.LightBlue600, Primary.LightBlue700, Primary.LightBlue800, Accent.LightBlue200, TextShade.WHITE);
			break;
		case 10:
			materialSkinManager.ColorScheme = new ColorScheme(Primary.Lime600, Primary.Lime700, Primary.Lime800, Accent.Lime200, TextShade.WHITE);
			break;
		case 11:
			materialSkinManager.ColorScheme = new ColorScheme(Primary.Indigo600, Primary.Indigo700, Primary.Indigo800, Accent.Indigo200, TextShade.WHITE);
			break;
		case 12:
			materialSkinManager.ColorScheme = new ColorScheme(Primary.DeepOrange600, Primary.DeepOrange700, Primary.DeepOrange800, Accent.DeepOrange200, TextShade.WHITE);
			break;
		case 13:
			materialSkinManager.ColorScheme = new ColorScheme(Primary.Amber600, Primary.Amber700, Primary.Amber800, Accent.Amber200, TextShade.WHITE);
			break;
		case 14:
			materialSkinManager.ColorScheme = new ColorScheme(Primary.Pink600, Primary.Pink700, Primary.Pink800, Accent.Pink200, TextShade.WHITE);
			break;
		case 15:
			materialSkinManager.ColorScheme = new ColorScheme(Primary.LightGreen600, Primary.LightGreen700, Primary.LightGreen800, Accent.LightGreen200, TextShade.WHITE);
			break;
		default:
			materialSkinManager.ColorScheme = new ColorScheme(Primary.Purple600, Primary.Purple700, Primary.Purple800, Accent.Purple200, TextShade.WHITE);
			break;
		}
	}

	private static Color ToColor(int argb)
	{
		return Color.FromArgb((argb & 0xFF0000) >> 16, (argb & 0xFF00) >> 8, argb & 0xFF);
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Server.Helper.FormMaterial));
		base.SuspendLayout();
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(800, 450);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "FormMaterial";
		this.Text = "FormMaterial";
		base.ResumeLayout(false);
	}
}
