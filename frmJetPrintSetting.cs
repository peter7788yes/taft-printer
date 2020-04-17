using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using 離線列印Client程式;

public class frmJetPrintSetting : Form
{
	private IContainer components;

	private Panel panelPrint;

	private Button btnCancel;

	private Button btnChangeSetting;

	private Label label2;

	private Label label1;

	private Label label4;

	private CheckBox cb_1;

	private Label label10;

	private Label label8;

	private Label label7;

	private Label label6;

	private Label label5;

	private CheckBox cb_9;

	private CheckBox cb_8;

	private CheckBox cb_7;

	private CheckBox cb_6;

	private CheckBox cb_5;

	private CheckBox cb_3;

	private CheckBox cb_2;

	private CheckBox cb_4;

	private Label label3;

	private Label label11;

	private TextBox txtMaxPerFile;

	private CheckBox cbF_9;

	private CheckBox cbF_8;

	private CheckBox cbF_7;

	private CheckBox cbF_6;

	private CheckBox cbF_5;

	private CheckBox cbF_4;

	private CheckBox cbF_3;

	private CheckBox cbF_2;

	private CheckBox cbF_1;

	private CheckBox cbF_10;

	private CheckBox cb_10;

	private Label label12;

	private Button btnSelJetExe;

	private Label label13;

	private CheckBox cbF_11;

	private CheckBox cb_11;

	private Label label9;

	private ComboBox cbJetDefaultExePath;

	private CheckBox cbF_12;

	private CheckBox cb_12;

	private Label label14;

	private CheckBox cbF_13;

	private CheckBox cb_13;

	private Label label15;

	private CheckBox cb_14;

	private CheckBox cbF_14;

	private Label label16;

	public frmJetPrintSetting()
	{
		InitializeComponent();
		SetChecked(Program.UserSettings.JetPrintSettings.NumPrintSelected);
		SetFieldChecked(Program.UserSettings.JetPrintSettings.NumFieldSelected);
		GenPrintFormatterAndNum();
		UpdateJetExeList(Program.UserSettings.JetPrintSettings.JetExecPaths);
		cbJetDefaultExePath.SelectedIndex = cbJetDefaultExePath.Items.IndexOf(Program.UserSettings.JetPrintSettings.JetExeDefaultPath);
		txtMaxPerFile.Text = Program.UserSettings.JetPrintSettings.MaxPerFile.ToString();
	}

	private void btnChangeSetting_Click(object sender, EventArgs e)
	{
		int num = 0;
		if (Program.UserSettings.JetPrintSettings.JetExeDefaultPath == string.Empty)
		{
			MessageBox.Show("請選擇預設噴墨執行檔位置。");
			return;
		}
		if (Program.UserSettings.JetPrintSettings.JetExecPaths.Count == 0)
		{
			MessageBox.Show("請新增至少一組噴墨執行檔位置。");
			return;
		}
		try
		{
			num = Convert.ToInt32(txtMaxPerFile.Text);
		}
		catch
		{
			MessageBox.Show("請輸入正確一切張數。");
			return;
		}
		if (num < 1)
		{
			MessageBox.Show("一切張數必需大於0。");
			return;
		}
		GenPrintFormatterAndNum();
		if (Program.UserSettings.JetPrintSettings.PrintFormatter == string.Empty)
		{
			MessageBox.Show("至少必需列印一種項目。");
			return;
		}
		Program.UserSettings.JetPrintSettings.MaxPerFile = num;
		base.DialogResult = DialogResult.OK;
		Close();
	}

	private void btnCancel_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void frmSetDisplayListItem_Load(object sender, EventArgs e)
	{
	}

	private void SetChecked(int n)
	{
		cb_1.Checked = ((n & 1) > 0);
		cb_2.Checked = ((n & 2) > 0);
		cb_3.Checked = ((n & 4) > 0);
		cb_4.Checked = ((n & 8) > 0);
		cb_5.Checked = ((n & 0x10) > 0);
		cb_6.Checked = ((n & 0x20) > 0);
		cb_7.Checked = ((n & 0x40) > 0);
		cb_8.Checked = ((n & 0x80) > 0);
		cb_9.Checked = ((n & 0x100) > 0);
		cb_10.Checked = ((n & 0x200) > 0);
		cb_11.Checked = ((n & 0x400) > 0);
		cb_12.Checked = ((n & 0x800) > 0);
		cb_13.Checked = ((n & 0x1000) > 0);
		cb_14.Checked = ((n & 0x2000) > 0);
	}

	private void SetFieldChecked(int n)
	{
		cbF_1.Checked = ((n & 1) > 0);
		cbF_2.Checked = ((n & 2) > 0);
		cbF_3.Checked = ((n & 4) > 0);
		cbF_4.Checked = ((n & 8) > 0);
		cbF_5.Checked = ((n & 0x10) > 0);
		cbF_6.Checked = ((n & 0x20) > 0);
		cbF_7.Checked = ((n & 0x40) > 0);
		cbF_8.Checked = ((n & 0x80) > 0);
		cbF_9.Checked = ((n & 0x100) > 0);
		cbF_10.Checked = ((n & 0x200) > 0);
		cbF_11.Checked = ((n & 0x400) > 0);
		cbF_12.Checked = ((n & 0x800) > 0);
		cbF_13.Checked = ((n & 0x1000) > 0);
		cbF_14.Checked = ((n & 0x2000) > 0);
	}

	private void UpdateJetExeList(List<string> lst)
	{
		cbJetDefaultExePath.Items.Clear();
		foreach (string item in lst)
		{
			cbJetDefaultExePath.Items.Add(item);
		}
	}

	private void GenPrintFormatterAndNum()
	{
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		int num2 = 0;
		if (cb_1.Checked)
		{
			num++;
			if (cbF_1.Checked)
			{
				num2++;
				stringBuilder.Append("{0}");
			}
			stringBuilder.Append("{1},");
		}
		if (cb_2.Checked)
		{
			num += 2;
			if (cbF_2.Checked)
			{
				num2 += 2;
				stringBuilder.Append("{2}");
			}
			stringBuilder.Append("{3},");
		}
		if (cb_3.Checked)
		{
			num += 4;
			if (cbF_3.Checked)
			{
				num2 += 4;
				stringBuilder.Append("{4}");
			}
			stringBuilder.Append("{5},");
		}
		if (cb_4.Checked)
		{
			num += 8;
			if (cbF_4.Checked)
			{
				num2 += 8;
				stringBuilder.Append("{6}");
			}
			stringBuilder.Append("{7},");
		}
		if (cb_5.Checked)
		{
			num += 16;
			if (cbF_5.Checked)
			{
				num2 += 16;
				stringBuilder.Append("{8}");
			}
			stringBuilder.Append("{9},");
		}
		if (cb_6.Checked)
		{
			num += 32;
			if (cbF_6.Checked)
			{
				num2 += 32;
				stringBuilder.Append("{10}");
			}
			stringBuilder.Append("{11},");
		}
		if (cb_7.Checked)
		{
			num += 64;
			if (cbF_7.Checked)
			{
				num2 += 64;
				stringBuilder.Append("{12}");
			}
			stringBuilder.Append("{13},");
		}
		if (cb_8.Checked)
		{
			num += 128;
			if (cbF_8.Checked)
			{
				num2 += 128;
				stringBuilder.Append("{14}");
			}
			stringBuilder.Append("{15},");
		}
		if (cb_9.Checked)
		{
			num += 256;
			if (cbF_9.Checked)
			{
				num2 += 256;
				stringBuilder.Append("{16}");
			}
			stringBuilder.Append("{17},");
		}
		if (cb_10.Checked)
		{
			num += 512;
			if (cbF_10.Checked)
			{
				num2 += 512;
				stringBuilder.Append("{18}");
			}
			stringBuilder.Append("{19},");
		}
		if (cb_11.Checked)
		{
			num += 1024;
			if (cbF_11.Checked)
			{
				num2 += 1024;
				stringBuilder.Append("{20}");
			}
			stringBuilder.Append("{21},");
		}
		if (cb_12.Checked)
		{
			num += 2048;
			if (cbF_12.Checked)
			{
				num2 += 2048;
				stringBuilder.Append("{22}");
			}
			stringBuilder.Append("{23},");
		}
		if (cb_13.Checked)
		{
			num += 4096;
			if (cbF_13.Checked)
			{
				num2 += 4096;
				stringBuilder.Append("{24}");
			}
			stringBuilder.Append("{25},");
		}
		if (cb_14.Checked)
		{
			num += 8192;
			if (cbF_14.Checked)
			{
				num2 += 8192;
				stringBuilder.Append("{26}");
			}
			stringBuilder.Append("{27},");
		}
		Program.UserSettings.JetPrintSettings.NumPrintSelected = num;
		Program.UserSettings.JetPrintSettings.NumFieldSelected = num2;
		Program.UserSettings.JetPrintSettings.PrintFormatter = ((stringBuilder.Length > 0) ? stringBuilder.ToString().TrimEnd(',') : string.Empty);
	}

	private void btnSelJetExe_Click(object sender, EventArgs e)
	{
		new frmJetExeList().ShowDialog(this);
		UpdateJetExeList(Program.UserSettings.JetPrintSettings.JetExecPaths);
		if (cbJetDefaultExePath.Items.Count == 0)
		{
			Program.UserSettings.JetPrintSettings.JetExeDefaultPath = string.Empty;
			return;
		}
		cbJetDefaultExePath.SelectedIndex = cbJetDefaultExePath.Items.IndexOf(Program.UserSettings.JetPrintSettings.JetExeDefaultPath);
		if (cbJetDefaultExePath.SelectedIndex < 0)
		{
			cbJetDefaultExePath.SelectedIndex = 0;
			Program.UserSettings.JetPrintSettings.JetExeDefaultPath = Convert.ToString(cbJetDefaultExePath.Items[cbJetDefaultExePath.SelectedIndex]);
		}
	}

	private void cbJetDefaultExePath_SelectedIndexChanged(object sender, EventArgs e)
	{
		Program.UserSettings.JetPrintSettings.JetExeDefaultPath = Convert.ToString(cbJetDefaultExePath.Items[cbJetDefaultExePath.SelectedIndex]);
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(離線列印Client程式.frmJetPrintSetting));
		panelPrint = new System.Windows.Forms.Panel();
		cbF_13 = new System.Windows.Forms.CheckBox();
		cb_13 = new System.Windows.Forms.CheckBox();
		label15 = new System.Windows.Forms.Label();
		cbF_12 = new System.Windows.Forms.CheckBox();
		cb_12 = new System.Windows.Forms.CheckBox();
		label14 = new System.Windows.Forms.Label();
		cbJetDefaultExePath = new System.Windows.Forms.ComboBox();
		cbF_11 = new System.Windows.Forms.CheckBox();
		cb_11 = new System.Windows.Forms.CheckBox();
		label9 = new System.Windows.Forms.Label();
		btnSelJetExe = new System.Windows.Forms.Button();
		label13 = new System.Windows.Forms.Label();
		cbF_10 = new System.Windows.Forms.CheckBox();
		cb_10 = new System.Windows.Forms.CheckBox();
		label12 = new System.Windows.Forms.Label();
		cbF_9 = new System.Windows.Forms.CheckBox();
		cbF_8 = new System.Windows.Forms.CheckBox();
		cbF_7 = new System.Windows.Forms.CheckBox();
		cbF_6 = new System.Windows.Forms.CheckBox();
		cbF_5 = new System.Windows.Forms.CheckBox();
		cbF_4 = new System.Windows.Forms.CheckBox();
		cbF_3 = new System.Windows.Forms.CheckBox();
		cbF_2 = new System.Windows.Forms.CheckBox();
		cbF_1 = new System.Windows.Forms.CheckBox();
		txtMaxPerFile = new System.Windows.Forms.TextBox();
		label11 = new System.Windows.Forms.Label();
		cb_4 = new System.Windows.Forms.CheckBox();
		label3 = new System.Windows.Forms.Label();
		cb_9 = new System.Windows.Forms.CheckBox();
		cb_8 = new System.Windows.Forms.CheckBox();
		cb_7 = new System.Windows.Forms.CheckBox();
		cb_6 = new System.Windows.Forms.CheckBox();
		cb_5 = new System.Windows.Forms.CheckBox();
		cb_3 = new System.Windows.Forms.CheckBox();
		cb_2 = new System.Windows.Forms.CheckBox();
		label10 = new System.Windows.Forms.Label();
		label8 = new System.Windows.Forms.Label();
		label7 = new System.Windows.Forms.Label();
		label6 = new System.Windows.Forms.Label();
		label5 = new System.Windows.Forms.Label();
		cb_1 = new System.Windows.Forms.CheckBox();
		label4 = new System.Windows.Forms.Label();
		btnCancel = new System.Windows.Forms.Button();
		btnChangeSetting = new System.Windows.Forms.Button();
		label2 = new System.Windows.Forms.Label();
		label1 = new System.Windows.Forms.Label();
		label16 = new System.Windows.Forms.Label();
		cbF_14 = new System.Windows.Forms.CheckBox();
		cb_14 = new System.Windows.Forms.CheckBox();
		panelPrint.SuspendLayout();
		SuspendLayout();
		panelPrint.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		panelPrint.Controls.Add(cb_14);
		panelPrint.Controls.Add(cbF_14);
		panelPrint.Controls.Add(label16);
		panelPrint.Controls.Add(cbF_13);
		panelPrint.Controls.Add(cb_13);
		panelPrint.Controls.Add(label15);
		panelPrint.Controls.Add(cbF_12);
		panelPrint.Controls.Add(cb_12);
		panelPrint.Controls.Add(label14);
		panelPrint.Controls.Add(cbJetDefaultExePath);
		panelPrint.Controls.Add(cbF_11);
		panelPrint.Controls.Add(cb_11);
		panelPrint.Controls.Add(label9);
		panelPrint.Controls.Add(btnSelJetExe);
		panelPrint.Controls.Add(label13);
		panelPrint.Controls.Add(cbF_10);
		panelPrint.Controls.Add(cb_10);
		panelPrint.Controls.Add(label12);
		panelPrint.Controls.Add(cbF_9);
		panelPrint.Controls.Add(cbF_8);
		panelPrint.Controls.Add(cbF_7);
		panelPrint.Controls.Add(cbF_6);
		panelPrint.Controls.Add(cbF_5);
		panelPrint.Controls.Add(cbF_4);
		panelPrint.Controls.Add(cbF_3);
		panelPrint.Controls.Add(cbF_2);
		panelPrint.Controls.Add(cbF_1);
		panelPrint.Controls.Add(txtMaxPerFile);
		panelPrint.Controls.Add(label11);
		panelPrint.Controls.Add(cb_4);
		panelPrint.Controls.Add(label3);
		panelPrint.Controls.Add(cb_9);
		panelPrint.Controls.Add(cb_8);
		panelPrint.Controls.Add(cb_7);
		panelPrint.Controls.Add(cb_6);
		panelPrint.Controls.Add(cb_5);
		panelPrint.Controls.Add(cb_3);
		panelPrint.Controls.Add(cb_2);
		panelPrint.Controls.Add(label10);
		panelPrint.Controls.Add(label8);
		panelPrint.Controls.Add(label7);
		panelPrint.Controls.Add(label6);
		panelPrint.Controls.Add(label5);
		panelPrint.Controls.Add(cb_1);
		panelPrint.Controls.Add(label4);
		panelPrint.Controls.Add(btnCancel);
		panelPrint.Controls.Add(btnChangeSetting);
		panelPrint.Controls.Add(label2);
		panelPrint.Controls.Add(label1);
		panelPrint.Location = new System.Drawing.Point(2, 3);
		panelPrint.Name = "panelPrint";
		panelPrint.Size = new System.Drawing.Size(316, 531);
		panelPrint.TabIndex = 2;
		cbF_13.AutoSize = true;
		cbF_13.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		cbF_13.Location = new System.Drawing.Point(150, 342);
		cbF_13.Name = "cbF_13";
		cbF_13.Size = new System.Drawing.Size(91, 20);
		cbF_13.TabIndex = 86;
		cbF_13.Text = "欄位名稱";
		cbF_13.UseVisualStyleBackColor = true;
		cb_13.AutoSize = true;
		cb_13.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		cb_13.Location = new System.Drawing.Point(247, 342);
		cb_13.Name = "cb_13";
		cb_13.Size = new System.Drawing.Size(59, 20);
		cb_13.TabIndex = 85;
		cb_13.Text = "列印";
		cb_13.UseVisualStyleBackColor = true;
		label15.AutoSize = true;
		label15.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label15.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label15.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label15.Location = new System.Drawing.Point(3, 338);
		label15.MaximumSize = new System.Drawing.Size(200, 0);
		label15.MinimumSize = new System.Drawing.Size(140, 0);
		label15.Name = "label15";
		label15.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label15.Size = new System.Drawing.Size(140, 26);
		label15.TabIndex = 84;
		label15.Text = "原料生產單位";
		cbF_12.AutoSize = true;
		cbF_12.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		cbF_12.Location = new System.Drawing.Point(150, 314);
		cbF_12.Name = "cbF_12";
		cbF_12.Size = new System.Drawing.Size(91, 20);
		cbF_12.TabIndex = 83;
		cbF_12.Text = "欄位名稱";
		cbF_12.UseVisualStyleBackColor = true;
		cb_12.AutoSize = true;
		cb_12.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		cb_12.Location = new System.Drawing.Point(247, 314);
		cb_12.Name = "cb_12";
		cb_12.Size = new System.Drawing.Size(59, 20);
		cb_12.TabIndex = 82;
		cb_12.Text = "列印";
		cb_12.UseVisualStyleBackColor = true;
		label14.AutoSize = true;
		label14.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label14.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label14.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label14.Location = new System.Drawing.Point(3, 310);
		label14.MaximumSize = new System.Drawing.Size(200, 0);
		label14.MinimumSize = new System.Drawing.Size(140, 0);
		label14.Name = "label14";
		label14.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label14.Size = new System.Drawing.Size(140, 26);
		label14.TabIndex = 81;
		label14.Text = "QR碼網址";
		cbJetDefaultExePath.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		cbJetDefaultExePath.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		cbJetDefaultExePath.FormattingEnabled = true;
		cbJetDefaultExePath.Location = new System.Drawing.Point(3, 423);
		cbJetDefaultExePath.Name = "cbJetDefaultExePath";
		cbJetDefaultExePath.Size = new System.Drawing.Size(301, 24);
		cbJetDefaultExePath.TabIndex = 80;
		cbJetDefaultExePath.SelectedIndexChanged += new System.EventHandler(cbJetDefaultExePath_SelectedIndexChanged);
		cbF_11.AutoSize = true;
		cbF_11.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		cbF_11.Location = new System.Drawing.Point(150, 285);
		cbF_11.Name = "cbF_11";
		cbF_11.Size = new System.Drawing.Size(91, 20);
		cbF_11.TabIndex = 79;
		cbF_11.Text = "欄位名稱";
		cbF_11.UseVisualStyleBackColor = true;
		cb_11.AutoSize = true;
		cb_11.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		cb_11.Location = new System.Drawing.Point(247, 285);
		cb_11.Name = "cb_11";
		cb_11.Size = new System.Drawing.Size(59, 20);
		cb_11.TabIndex = 78;
		cb_11.Text = "列印";
		cb_11.UseVisualStyleBackColor = true;
		label9.AutoSize = true;
		label9.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label9.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label9.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label9.Location = new System.Drawing.Point(3, 282);
		label9.MaximumSize = new System.Drawing.Size(200, 0);
		label9.MinimumSize = new System.Drawing.Size(140, 0);
		label9.Name = "label9";
		label9.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label9.Size = new System.Drawing.Size(140, 26);
		label9.TabIndex = 77;
		label9.Text = "使用者輸入";
		btnSelJetExe.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		btnSelJetExe.Location = new System.Drawing.Point(149, 394);
		btnSelJetExe.Name = "btnSelJetExe";
		btnSelJetExe.Size = new System.Drawing.Size(109, 26);
		btnSelJetExe.TabIndex = 75;
		btnSelJetExe.Text = "增修執行檔";
		btnSelJetExe.UseVisualStyleBackColor = true;
		btnSelJetExe.Click += new System.EventHandler(btnSelJetExe_Click);
		label13.AutoSize = true;
		label13.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label13.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label13.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label13.Location = new System.Drawing.Point(3, 394);
		label13.MaximumSize = new System.Drawing.Size(200, 0);
		label13.MinimumSize = new System.Drawing.Size(140, 0);
		label13.Name = "label13";
		label13.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label13.Size = new System.Drawing.Size(140, 26);
		label13.TabIndex = 74;
		label13.Text = "噴墨執行檔位置";
		cbF_10.AutoSize = true;
		cbF_10.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		cbF_10.Location = new System.Drawing.Point(150, 254);
		cbF_10.Name = "cbF_10";
		cbF_10.Size = new System.Drawing.Size(91, 20);
		cbF_10.TabIndex = 73;
		cbF_10.Text = "欄位名稱";
		cbF_10.UseVisualStyleBackColor = true;
		cb_10.AutoSize = true;
		cb_10.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		cb_10.Location = new System.Drawing.Point(247, 254);
		cb_10.Name = "cb_10";
		cb_10.Size = new System.Drawing.Size(59, 20);
		cb_10.TabIndex = 72;
		cb_10.Text = "列印";
		cb_10.UseVisualStyleBackColor = true;
		label12.AutoSize = true;
		label12.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label12.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label12.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label12.Location = new System.Drawing.Point(3, 58);
		label12.MaximumSize = new System.Drawing.Size(200, 0);
		label12.MinimumSize = new System.Drawing.Size(140, 0);
		label12.Name = "label12";
		label12.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label12.Size = new System.Drawing.Size(140, 26);
		label12.TabIndex = 71;
		label12.Text = "有效日期";
		cbF_9.AutoSize = true;
		cbF_9.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		cbF_9.Location = new System.Drawing.Point(150, 226);
		cbF_9.Name = "cbF_9";
		cbF_9.Size = new System.Drawing.Size(91, 20);
		cbF_9.TabIndex = 70;
		cbF_9.Text = "欄位名稱";
		cbF_9.UseVisualStyleBackColor = true;
		cbF_8.AutoSize = true;
		cbF_8.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		cbF_8.Location = new System.Drawing.Point(150, 199);
		cbF_8.Name = "cbF_8";
		cbF_8.Size = new System.Drawing.Size(91, 20);
		cbF_8.TabIndex = 69;
		cbF_8.Text = "欄位名稱";
		cbF_8.UseVisualStyleBackColor = true;
		cbF_7.AutoSize = true;
		cbF_7.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		cbF_7.Location = new System.Drawing.Point(150, 171);
		cbF_7.Name = "cbF_7";
		cbF_7.Size = new System.Drawing.Size(91, 20);
		cbF_7.TabIndex = 68;
		cbF_7.Text = "欄位名稱";
		cbF_7.UseVisualStyleBackColor = true;
		cbF_6.AutoSize = true;
		cbF_6.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		cbF_6.Location = new System.Drawing.Point(150, 143);
		cbF_6.Name = "cbF_6";
		cbF_6.Size = new System.Drawing.Size(91, 20);
		cbF_6.TabIndex = 67;
		cbF_6.Text = "欄位名稱";
		cbF_6.UseVisualStyleBackColor = true;
		cbF_5.AutoSize = true;
		cbF_5.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		cbF_5.Location = new System.Drawing.Point(150, 112);
		cbF_5.Name = "cbF_5";
		cbF_5.Size = new System.Drawing.Size(91, 20);
		cbF_5.TabIndex = 66;
		cbF_5.Text = "欄位名稱";
		cbF_5.UseVisualStyleBackColor = true;
		cbF_4.AutoSize = true;
		cbF_4.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		cbF_4.Location = new System.Drawing.Point(150, 84);
		cbF_4.Name = "cbF_4";
		cbF_4.Size = new System.Drawing.Size(91, 20);
		cbF_4.TabIndex = 65;
		cbF_4.Text = "欄位名稱";
		cbF_4.UseVisualStyleBackColor = true;
		cbF_3.AutoSize = true;
		cbF_3.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		cbF_3.Location = new System.Drawing.Point(150, 56);
		cbF_3.Name = "cbF_3";
		cbF_3.Size = new System.Drawing.Size(91, 20);
		cbF_3.TabIndex = 64;
		cbF_3.Text = "欄位名稱";
		cbF_3.UseVisualStyleBackColor = true;
		cbF_2.AutoSize = true;
		cbF_2.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		cbF_2.Location = new System.Drawing.Point(150, 29);
		cbF_2.Name = "cbF_2";
		cbF_2.Size = new System.Drawing.Size(91, 20);
		cbF_2.TabIndex = 63;
		cbF_2.Text = "欄位名稱";
		cbF_2.UseVisualStyleBackColor = true;
		cbF_1.AutoSize = true;
		cbF_1.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		cbF_1.Location = new System.Drawing.Point(150, 3);
		cbF_1.Name = "cbF_1";
		cbF_1.Size = new System.Drawing.Size(91, 20);
		cbF_1.TabIndex = 62;
		cbF_1.Text = "欄位名稱";
		cbF_1.UseVisualStyleBackColor = true;
		txtMaxPerFile.Font = new System.Drawing.Font("新細明體", 12f);
		txtMaxPerFile.Location = new System.Drawing.Point(150, 450);
		txtMaxPerFile.Name = "txtMaxPerFile";
		txtMaxPerFile.Size = new System.Drawing.Size(154, 27);
		txtMaxPerFile.TabIndex = 61;
		label11.AutoSize = true;
		label11.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label11.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label11.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label11.Location = new System.Drawing.Point(3, 450);
		label11.MaximumSize = new System.Drawing.Size(200, 0);
		label11.MinimumSize = new System.Drawing.Size(140, 0);
		label11.Name = "label11";
		label11.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label11.Size = new System.Drawing.Size(140, 26);
		label11.TabIndex = 60;
		label11.Text = "每檔案限制張數";
		cb_4.AutoSize = true;
		cb_4.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		cb_4.Location = new System.Drawing.Point(247, 84);
		cb_4.Name = "cb_4";
		cb_4.Size = new System.Drawing.Size(59, 20);
		cb_4.TabIndex = 57;
		cb_4.Text = "列印";
		cb_4.UseVisualStyleBackColor = true;
		label3.AutoSize = true;
		label3.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label3.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label3.Location = new System.Drawing.Point(3, 114);
		label3.MaximumSize = new System.Drawing.Size(200, 0);
		label3.MinimumSize = new System.Drawing.Size(140, 0);
		label3.Name = "label3";
		label3.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label3.Size = new System.Drawing.Size(140, 26);
		label3.TabIndex = 56;
		label3.Text = "收貨單位";
		cb_9.AutoSize = true;
		cb_9.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		cb_9.Location = new System.Drawing.Point(247, 226);
		cb_9.Name = "cb_9";
		cb_9.Size = new System.Drawing.Size(59, 20);
		cb_9.TabIndex = 52;
		cb_9.Text = "列印";
		cb_9.UseVisualStyleBackColor = true;
		cb_8.AutoSize = true;
		cb_8.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		cb_8.Location = new System.Drawing.Point(247, 199);
		cb_8.Name = "cb_8";
		cb_8.Size = new System.Drawing.Size(59, 20);
		cb_8.TabIndex = 50;
		cb_8.Text = "列印";
		cb_8.UseVisualStyleBackColor = true;
		cb_7.AutoSize = true;
		cb_7.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		cb_7.Location = new System.Drawing.Point(247, 171);
		cb_7.Name = "cb_7";
		cb_7.Size = new System.Drawing.Size(59, 20);
		cb_7.TabIndex = 49;
		cb_7.Text = "列印";
		cb_7.UseVisualStyleBackColor = true;
		cb_6.AutoSize = true;
		cb_6.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		cb_6.Location = new System.Drawing.Point(247, 143);
		cb_6.Name = "cb_6";
		cb_6.Size = new System.Drawing.Size(59, 20);
		cb_6.TabIndex = 48;
		cb_6.Text = "列印";
		cb_6.UseVisualStyleBackColor = true;
		cb_5.AutoSize = true;
		cb_5.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		cb_5.Location = new System.Drawing.Point(247, 112);
		cb_5.Name = "cb_5";
		cb_5.Size = new System.Drawing.Size(59, 20);
		cb_5.TabIndex = 47;
		cb_5.Text = "列印";
		cb_5.UseVisualStyleBackColor = true;
		cb_3.AutoSize = true;
		cb_3.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		cb_3.Location = new System.Drawing.Point(247, 56);
		cb_3.Name = "cb_3";
		cb_3.Size = new System.Drawing.Size(59, 20);
		cb_3.TabIndex = 46;
		cb_3.Text = "列印";
		cb_3.UseVisualStyleBackColor = true;
		cb_2.AutoSize = true;
		cb_2.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		cb_2.Location = new System.Drawing.Point(247, 29);
		cb_2.Name = "cb_2";
		cb_2.Size = new System.Drawing.Size(59, 20);
		cb_2.TabIndex = 45;
		cb_2.Text = "列印";
		cb_2.UseVisualStyleBackColor = true;
		label10.AutoSize = true;
		label10.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label10.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label10.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label10.Location = new System.Drawing.Point(3, 254);
		label10.MaximumSize = new System.Drawing.Size(200, 0);
		label10.MinimumSize = new System.Drawing.Size(140, 0);
		label10.Name = "label10";
		label10.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label10.Size = new System.Drawing.Size(140, 26);
		label10.TabIndex = 41;
		label10.Text = "原栽培編號";
		label8.AutoSize = true;
		label8.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label8.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label8.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label8.Location = new System.Drawing.Point(3, 226);
		label8.MaximumSize = new System.Drawing.Size(200, 0);
		label8.MinimumSize = new System.Drawing.Size(140, 0);
		label8.Name = "label8";
		label8.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label8.Size = new System.Drawing.Size(140, 26);
		label8.TabIndex = 39;
		label8.Text = "EAN";
		label7.AutoSize = true;
		label7.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label7.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label7.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label7.Location = new System.Drawing.Point(3, 198);
		label7.MaximumSize = new System.Drawing.Size(200, 0);
		label7.MinimumSize = new System.Drawing.Size(140, 0);
		label7.Name = "label7";
		label7.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label7.Size = new System.Drawing.Size(140, 26);
		label7.TabIndex = 38;
		label7.Text = "追溯號碼";
		label6.AutoSize = true;
		label6.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label6.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label6.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label6.Location = new System.Drawing.Point(3, 170);
		label6.MaximumSize = new System.Drawing.Size(200, 0);
		label6.MinimumSize = new System.Drawing.Size(140, 0);
		label6.Name = "label6";
		label6.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label6.Size = new System.Drawing.Size(140, 26);
		label6.TabIndex = 37;
		label6.Text = "規格";
		label5.AutoSize = true;
		label5.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label5.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label5.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label5.Location = new System.Drawing.Point(3, 142);
		label5.MaximumSize = new System.Drawing.Size(200, 0);
		label5.MinimumSize = new System.Drawing.Size(140, 0);
		label5.Name = "label5";
		label5.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label5.Size = new System.Drawing.Size(140, 26);
		label5.TabIndex = 36;
		label5.Text = "品項";
		cb_1.AutoSize = true;
		cb_1.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		cb_1.Location = new System.Drawing.Point(247, 3);
		cb_1.Name = "cb_1";
		cb_1.Size = new System.Drawing.Size(59, 20);
		cb_1.TabIndex = 35;
		cb_1.Text = "列印";
		cb_1.UseVisualStyleBackColor = true;
		label4.AutoSize = true;
		label4.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label4.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label4.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label4.Location = new System.Drawing.Point(3, 86);
		label4.MaximumSize = new System.Drawing.Size(200, 0);
		label4.MinimumSize = new System.Drawing.Size(140, 0);
		label4.Name = "label4";
		label4.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label4.Size = new System.Drawing.Size(140, 26);
		label4.TabIndex = 33;
		label4.Text = "農民";
		btnCancel.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		btnCancel.Location = new System.Drawing.Point(150, 491);
		btnCancel.Name = "btnCancel";
		btnCancel.Size = new System.Drawing.Size(80, 31);
		btnCancel.TabIndex = 20;
		btnCancel.Text = "取消";
		btnCancel.UseVisualStyleBackColor = true;
		btnCancel.Click += new System.EventHandler(btnCancel_Click);
		btnChangeSetting.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		btnChangeSetting.Location = new System.Drawing.Point(64, 491);
		btnChangeSetting.Name = "btnChangeSetting";
		btnChangeSetting.Size = new System.Drawing.Size(80, 31);
		btnChangeSetting.TabIndex = 19;
		btnChangeSetting.Text = "確認";
		btnChangeSetting.UseVisualStyleBackColor = true;
		btnChangeSetting.Click += new System.EventHandler(btnChangeSetting_Click);
		label2.AutoSize = true;
		label2.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label2.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label2.Location = new System.Drawing.Point(3, 30);
		label2.MaximumSize = new System.Drawing.Size(200, 0);
		label2.MinimumSize = new System.Drawing.Size(140, 0);
		label2.Name = "label2";
		label2.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label2.Size = new System.Drawing.Size(140, 26);
		label2.TabIndex = 2;
		label2.Text = "包裝日期";
		label1.AutoSize = true;
		label1.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label1.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label1.Location = new System.Drawing.Point(3, 2);
		label1.MaximumSize = new System.Drawing.Size(200, 0);
		label1.MinimumSize = new System.Drawing.Size(140, 0);
		label1.Name = "label1";
		label1.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label1.Size = new System.Drawing.Size(140, 26);
		label1.TabIndex = 0;
		label1.Text = "上傳日期";
		label16.AutoSize = true;
		label16.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label16.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label16.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label16.Location = new System.Drawing.Point(3, 366);
		label16.MaximumSize = new System.Drawing.Size(200, 0);
		label16.MinimumSize = new System.Drawing.Size(140, 0);
		label16.Name = "label16";
		label16.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label16.Size = new System.Drawing.Size(140, 26);
		label16.TabIndex = 87;
		label16.Text = "驗證單位";
		cbF_14.AutoSize = true;
		cbF_14.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		cbF_14.Location = new System.Drawing.Point(150, 368);
		cbF_14.Name = "cbF_14";
		cbF_14.Size = new System.Drawing.Size(91, 20);
		cbF_14.TabIndex = 88;
		cbF_14.Text = "欄位名稱";
		cbF_14.UseVisualStyleBackColor = true;
		cb_14.AutoSize = true;
		cb_14.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		cb_14.Location = new System.Drawing.Point(247, 368);
		cb_14.Name = "cb_14";
		cb_14.Size = new System.Drawing.Size(59, 20);
		cb_14.TabIndex = 89;
		cb_14.Text = "列印";
		cb_14.UseVisualStyleBackColor = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(322, 539);
		base.Controls.Add(panelPrint);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "frmJetPrintSetting";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		Text = "項目列印設定";
		base.Load += new System.EventHandler(frmSetDisplayListItem_Load);
		panelPrint.ResumeLayout(false);
		panelPrint.PerformLayout();
		ResumeLayout(false);
	}
}
