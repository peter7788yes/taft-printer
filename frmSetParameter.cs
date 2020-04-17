using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using T00SharedLibraryDotNet20;
using 離線列印Client程式;

public class frmSetParameter : Form
{
	private PrintFormat _printFormatSelected;

	private IContainer components;

	private Panel panelPrint;

	private ComboBox cmbDaysFlush;

	private Button btnCancel;

	private Button btnChangeSetting;

	private Label label2;

	private Label label1;

	private ComboBox cmbShowHistory;

	private Label label3;

	private Label lblPrintFormatSelected;

	private Button btnSelectPrintFormat;

	private ComboBox cmbDefaultPrinter;

	private Label label4;

	private Button btnSelectDisplayListItem;

	private Label label5;

	private CheckBox cbExpDate;

	private Label label6;

	private TextBox txtExpDateAccum;

	private Label label7;

	private CheckBox cbUserInput;

	private Label label8;

	private Label label9;

	private CheckBox cbIgnoreUserInputDesc;

	private TextBox txtUserInputDesc;

	private TextBox txtExpDateDesc;

	private Label label10;

	private ComboBox cmbShowNDays;

	private Label label11;

	private TextBox txtUserInputDefault;

	private Label label12;

	private Button btnTestPrint;

	private ComboBox cmbDLAll;

	private Label label13;

	private TextBox txtTestPrintNum;

	private Label label14;

	private Panel pHywebOnly;

	private Label label15;

	private TextBox txtQX;

	private Button btnHywebOnlyDesc;

	private Label label17;

	private TextBox txtDY;

	private Label label18;

	private TextBox txtDX;

	private Label label16;

	private TextBox txtQY;

	private Label label20;

	private CheckBox cbShowPackDate;

	private CheckBox cbRotate;

	private ComboBox cmbUserInputOption;

	private Label label21;

	private Label label19;

	private TextBox txtDaysFlush;

	public frmSetParameter()
	{
		InitializeComponent();
		Program.ReloadCRC();
	}

	private bool SaveChanges(bool showPrompt)
	{
		bool flag = true;
		try
		{
			if (txtExpDateDesc.Text.Length < 2 || txtExpDateDesc.Text.Length > 4)
			{
				MessageBox.Show("保存日期顯示名稱必需為2~4個字元。");
				txtExpDateDesc.Focus();
				return false;
			}
			if (cbExpDate.Checked)
			{
				try
				{
					int num = Convert.ToInt32(txtExpDateAccum.Text);
					if (num > 0)
					{
						flag = true;
						Program.ExpireDateAccum = num;
					}
					else
					{
						flag = false;
					}
				}
				catch
				{
					flag = false;
				}
			}
			else
			{
				Program.ExpireDateAccum = 0;
			}
			if (!flag)
			{
				MessageBox.Show("保存期限天數必需為大於零的數字，請重新輸入。");
				return false;
			}
			Program.UserSettings.ExpDateDesc = txtExpDateDesc.Text;
			Program.UserSettings.EnableUserInput = cbUserInput.Checked;
			Program.UserSettings.UserInputOption = cmbUserInputOption.SelectedIndex;
			Program.UserSettings.UserInputDesc = txtUserInputDesc.Text;
			Program.UserSettings.UserInputDfut = txtUserInputDefault.Text;
			Program.UserSettings.IsRotate = cbRotate.Checked;
			Program.UserSettings.ShowNextNDays = ((Convert.ToString(cmbShowNDays.SelectedValue) == "不限") ? (-1) : cmbShowNDays.SelectedIndex);
			Program.UserSettings.DontDownloadAll = (cmbDLAll.SelectedIndex == 1);
			Program.UserSettings.ShowPackDate = cbShowPackDate.Checked;
			TaftOLP.Default.UserSetting = Program.UserSettings.Serialize(false);
			TaftOLP.Default.Save();
			Program.ShowHistory = ((cmbShowHistory.SelectedIndex == 0) ? true : false);
			Program.DayToFlush = -(Convert.ToInt32(txtDaysFlush.Text) - 1);
			Program.ThisPrintFormat = _printFormatSelected;
			Program.PrinterName = ((cmbDefaultPrinter.Text == "使用系統預設印表機" && cmbDefaultPrinter.SelectedIndex == 0) ? "" : cmbDefaultPrinter.Text);
			string[,] strFieldArray = new string[6, 2]
			{
				{
					"ShowHistory",
					Program.ShowHistory ? "Y" : "N"
				},
				{
					"DayToFlush",
					(-Program.DayToFlush + 1).ToString()
				},
				{
					"PrintFormat",
					Convert.ToInt32(Program.ThisPrintFormat).ToString()
				},
				{
					"DefaultPrinterName",
					Program.PrinterName
				},
				{
					"DisplayList",
					Program.DisplayListItems.ToString()
				},
				{
					"ExpireDateAccum",
					Program.ExpireDateAccum.ToString()
				}
			};
			if (Convert.ToInt32(DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Update, "", "SysParam", "1=1", "", strFieldArray, null, CommandOperationType.ExecuteNonQuery)) == 0)
			{
				DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Insert, "", "SysParam", "", "", strFieldArray, null, CommandOperationType.ExecuteNonQuery);
			}
			if (showPrompt)
			{
				MessageBox.Show("更新參數完成");
			}
			Program.WriteCRC();
			return true;
		}
		catch (Exception)
		{
			if (showPrompt)
			{
				MessageBox.Show("更新參數失敗");
			}
			return false;
		}
	}

	private void btnChangeSetting_Click(object sender, EventArgs e)
	{
		SaveChanges(true);
		Close();
	}

	private void frmSetParameter_Load(object sender, EventArgs e)
	{
		try
		{
			if (Program.IsHyweb)
			{
				pHywebOnly.Visible = true;
			}
			if (Program.ShowHistory)
			{
				cmbShowHistory.SelectedIndex = 0;
			}
			else
			{
				cmbShowHistory.SelectedIndex = 1;
			}
			cmbDLAll.SelectedIndex = (Program.UserSettings.DontDownloadAll ? 1 : 0);
			txtExpDateDesc.Text = Program.UserSettings.ExpDateDesc;
			txtUserInputDesc.Text = Program.UserSettings.UserInputDesc;
			txtUserInputDefault.Text = Program.UserSettings.UserInputDfut;
			cbUserInput.Checked = Program.UserSettings.EnableUserInput;
			cbShowPackDate.Checked = Program.UserSettings.ShowPackDate;
			cbRotate.Checked = Program.UserSettings.IsRotate;
			if (cbUserInput.Checked)
			{
				txtUserInputDesc.Enabled = (txtUserInputDesc.Text.Length > 0);
				cbIgnoreUserInputDesc.Enabled = true;
				cbIgnoreUserInputDesc.Checked = !txtUserInputDesc.Enabled;
				cmbUserInputOption.SelectedIndex = Program.UserSettings.UserInputOption;
			}
			else
			{
				cbIgnoreUserInputDesc.Checked = false;
				cbIgnoreUserInputDesc.Enabled = false;
				txtUserInputDesc.Enabled = false;
				txtUserInputDesc.Text = string.Empty;
				txtUserInputDefault.Enabled = false;
				txtUserInputDefault.Text = string.Empty;
				cmbUserInputOption.Enabled = false;
			}
			txtExpDateAccum.Enabled = (Program.ExpireDateAccum > 0);
			cbExpDate.Checked = (Program.ExpireDateAccum > 0);
			txtExpDateAccum.Text = ((Program.ExpireDateAccum > 0) ? Program.ExpireDateAccum.ToString() : string.Empty);
			txtDaysFlush.Text = (-Program.DayToFlush + 1).ToString();
			cmbShowNDays.SelectedIndex = ((Program.UserSettings.ShowNextNDays >= 0) ? Program.UserSettings.ShowNextNDays : 0);
			lblPrintFormatSelected.Text = PrintCodeUtilties.PrtFmtShowName(Program.ThisPrintFormat);
			_printFormatSelected = Program.ThisPrintFormat;
			cbRotate.Visible = (_printFormatSelected == PrintFormat.標籤45X52 || _printFormatSelected == PrintFormat.標籤45X52顯示保存日期 || _printFormatSelected == PrintFormat.標籤75X42 || _printFormatSelected == PrintFormat.標籤75X42含生產者資訊 || _printFormatSelected == PrintFormat.標籤1品項 || _printFormatSelected == PrintFormat.標籤2品項 || _printFormatSelected == PrintFormat.標籤6品項 || _printFormatSelected == PrintFormat.標籤10品項);
			ReloadPrinters();
		}
		catch (Exception)
		{
		}
	}

	private void ReloadPrinters()
	{
		try
		{
			cmbDefaultPrinter.Items.Clear();
			cmbDefaultPrinter.Items.Add("使用系統預設印表機");
			foreach (string installedPrinter in PrinterSettings.InstalledPrinters)
			{
				cmbDefaultPrinter.Items.Add(installedPrinter);
			}
			cmbDefaultPrinter.Text = ((Program.PrinterName == string.Empty) ? "使用系統預設印表機" : Program.PrinterName);
			if (cmbDefaultPrinter.SelectedIndex == -1)
			{
				cmbDefaultPrinter.SelectedIndex = 0;
			}
		}
		catch (Exception)
		{
			cmbDefaultPrinter.Items.Clear();
			cmbDefaultPrinter.Items.Add("使用系統預設印表機");
			cmbDefaultPrinter.SelectedIndex = 0;
		}
	}

	private void cmbShowHistory_SelectedIndexChanged(object sender, EventArgs e)
	{
	}

	private void cmbDaysFlush_SelectedIndexChanged(object sender, EventArgs e)
	{
	}

	private void btnCancel_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btnSelectPrintFormat_Click(object sender, EventArgs e)
	{
		frmSelectPrintFormat frmSelectPrintFormat = new frmSelectPrintFormat(_printFormatSelected);
		if (frmSelectPrintFormat.ShowDialog(this) == DialogResult.OK)
		{
			lblPrintFormatSelected.Text = PrintCodeUtilties.PrtFmtShowName(frmSelectPrintFormat.PrintFormatSelected);
			_printFormatSelected = frmSelectPrintFormat.PrintFormatSelected;
			cbRotate.Visible = (_printFormatSelected == PrintFormat.標籤45X52 || _printFormatSelected == PrintFormat.標籤45X52顯示保存日期 || _printFormatSelected == PrintFormat.標籤75X42 || _printFormatSelected == PrintFormat.標籤75X42含生產者資訊 || _printFormatSelected == PrintFormat.標籤1品項 || _printFormatSelected == PrintFormat.標籤2品項 || _printFormatSelected == PrintFormat.標籤6品項 || _printFormatSelected == PrintFormat.標籤10品項);
		}
		frmSelectPrintFormat.Dispose();
	}

	private void btnSelectDisplayListItem_Click(object sender, EventArgs e)
	{
		frmSetDisplayListItem frmSetDisplayListItem = new frmSetDisplayListItem(Program.DisplayListItems);
		if (frmSetDisplayListItem.ShowDialog(this) == DialogResult.OK)
		{
			Program.DisplayListItems = frmSetDisplayListItem.DisplayListItem;
		}
		frmSetDisplayListItem.Dispose();
	}

	private void cbExpDate_CheckedChanged(object sender, EventArgs e)
	{
		txtExpDateAccum.Enabled = cbExpDate.Checked;
	}

	private void cbUserInput_CheckedChanged(object sender, EventArgs e)
	{
		if (cbUserInput.Checked)
		{
			cmbUserInputOption.SelectedIndex = 0;
			txtUserInputDesc.Enabled = true;
			txtUserInputDefault.Enabled = true;
			cbIgnoreUserInputDesc.Enabled = true;
			cbIgnoreUserInputDesc.Checked = !txtUserInputDesc.Enabled;
			cmbUserInputOption.Enabled = true;
		}
		else
		{
			cbIgnoreUserInputDesc.Checked = false;
			cbIgnoreUserInputDesc.Enabled = false;
			txtUserInputDesc.Enabled = false;
			txtUserInputDesc.Text = string.Empty;
			txtUserInputDefault.Enabled = false;
			txtUserInputDefault.Text = string.Empty;
			cmbUserInputOption.Enabled = false;
		}
	}

	private void cbIgnoreUserInputDesc_CheckedChanged(object sender, EventArgs e)
	{
		txtUserInputDesc.Enabled = !cbIgnoreUserInputDesc.Checked;
		if (!txtUserInputDesc.Enabled)
		{
			txtUserInputDesc.Text = string.Empty;
		}
	}

	private void btnTestPrint_Click(object sender, EventArgs e)
	{
		SaveChanges(false);
		if (Program.ThisPrintFormat == PrintFormat.噴墨列印)
		{
			MessageBox.Show("目前噴墨列印不提供測試列印");
			return;
		}
		short num = 0;
		try
		{
			num = Convert.ToInt16(txtTestPrintNum.Text);
			if (num <= 0)
			{
				num = -1;
			}
		}
		catch
		{
			num = -2;
		}
		switch (num)
		{
		case -1:
			MessageBox.Show("測試列印張數必需大於零。");
			txtTestPrintNum.Focus();
			break;
		case -2:
			MessageBox.Show("測試列印張數必需為數字。");
			txtTestPrintNum.Focus();
			break;
		default:
			try
			{
				new QRCode("http://tqr.tw/?q=sCb7ANiv5gmk2u", "測試品項名稱測試品項名稱測試品項名稱測試品項名稱測試品項名稱測試品項名稱", "測試驗證機構", "2000/01/01", "2000/01/02", "09909876543210", 1, "4711240010228", Program.ThisPrintFormat, Program.PrinterName, num, Program.ProducerName, "測試1", Program.UserSettings.ExpDateDesc, "測試2", true, Convert.ToInt32(txtDX.Text), Convert.ToInt32(txtDY.Text), Convert.ToInt32(txtQX.Text), Convert.ToInt32(txtQY.Text), Program.UserSettings.IsRotate).Print();
			}
			catch (Exception)
			{
			}
			break;
		}
	}

	private void btnHywebOnlyDesc_Click(object sender, EventArgs e)
	{
		MessageBox.Show("qx: QRCode部份，左右，愈大愈右邊\r\nqy: QRCode部份，上下，愈大愈下面\r\ndx: 說明部份，左右，愈大愈右邊\r\ndy: 說明部份，上下，愈大愈下面");
	}

	private void cmbUserInputOption_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (cmbUserInputOption.SelectedIndex == 0)
		{
			txtUserInputDesc.Enabled = true;
			txtUserInputDefault.Enabled = true;
			cbIgnoreUserInputDesc.Enabled = true;
			cbIgnoreUserInputDesc.Checked = !txtUserInputDesc.Enabled;
		}
		else
		{
			cbIgnoreUserInputDesc.Checked = false;
			cbIgnoreUserInputDesc.Enabled = false;
			txtUserInputDesc.Enabled = false;
			txtUserInputDesc.Text = string.Empty;
			txtUserInputDefault.Enabled = false;
			txtUserInputDefault.Text = string.Empty;
		}
	}

	private void txtDaysFlush_TextChanged(object sender, EventArgs e)
	{
		if (!(txtDaysFlush.Text != ""))
		{
			return;
		}
		int result;
		if (int.TryParse(txtDaysFlush.Text, out result))
		{
			if (result > 90 || result < 0)
			{
				MessageBox.Show("註銷天數數字範圍大於0小於90。");
				txtDaysFlush.Text = "";
			}
		}
		else
		{
			MessageBox.Show("註銷天數數字範圍大於0小於90。");
			txtDaysFlush.Text = "";
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(離線列印Client程式.frmSetParameter));
		panelPrint = new System.Windows.Forms.Panel();
		label21 = new System.Windows.Forms.Label();
		label19 = new System.Windows.Forms.Label();
		txtDaysFlush = new System.Windows.Forms.TextBox();
		cmbUserInputOption = new System.Windows.Forms.ComboBox();
		cbRotate = new System.Windows.Forms.CheckBox();
		pHywebOnly = new System.Windows.Forms.Panel();
		btnHywebOnlyDesc = new System.Windows.Forms.Button();
		label17 = new System.Windows.Forms.Label();
		txtDY = new System.Windows.Forms.TextBox();
		label18 = new System.Windows.Forms.Label();
		txtDX = new System.Windows.Forms.TextBox();
		label16 = new System.Windows.Forms.Label();
		txtQY = new System.Windows.Forms.TextBox();
		label15 = new System.Windows.Forms.Label();
		txtQX = new System.Windows.Forms.TextBox();
		txtTestPrintNum = new System.Windows.Forms.TextBox();
		label14 = new System.Windows.Forms.Label();
		btnTestPrint = new System.Windows.Forms.Button();
		cmbDLAll = new System.Windows.Forms.ComboBox();
		label13 = new System.Windows.Forms.Label();
		txtUserInputDefault = new System.Windows.Forms.TextBox();
		label12 = new System.Windows.Forms.Label();
		cmbShowNDays = new System.Windows.Forms.ComboBox();
		label11 = new System.Windows.Forms.Label();
		txtExpDateDesc = new System.Windows.Forms.TextBox();
		label20 = new System.Windows.Forms.Label();
		label10 = new System.Windows.Forms.Label();
		cbIgnoreUserInputDesc = new System.Windows.Forms.CheckBox();
		txtUserInputDesc = new System.Windows.Forms.TextBox();
		label9 = new System.Windows.Forms.Label();
		cbUserInput = new System.Windows.Forms.CheckBox();
		label8 = new System.Windows.Forms.Label();
		txtExpDateAccum = new System.Windows.Forms.TextBox();
		label7 = new System.Windows.Forms.Label();
		cbShowPackDate = new System.Windows.Forms.CheckBox();
		cbExpDate = new System.Windows.Forms.CheckBox();
		label6 = new System.Windows.Forms.Label();
		btnSelectDisplayListItem = new System.Windows.Forms.Button();
		label5 = new System.Windows.Forms.Label();
		cmbDefaultPrinter = new System.Windows.Forms.ComboBox();
		label4 = new System.Windows.Forms.Label();
		btnSelectPrintFormat = new System.Windows.Forms.Button();
		lblPrintFormatSelected = new System.Windows.Forms.Label();
		label3 = new System.Windows.Forms.Label();
		cmbShowHistory = new System.Windows.Forms.ComboBox();
		cmbDaysFlush = new System.Windows.Forms.ComboBox();
		btnCancel = new System.Windows.Forms.Button();
		btnChangeSetting = new System.Windows.Forms.Button();
		label2 = new System.Windows.Forms.Label();
		label1 = new System.Windows.Forms.Label();
		panelPrint.SuspendLayout();
		pHywebOnly.SuspendLayout();
		SuspendLayout();
		panelPrint.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		panelPrint.Controls.Add(label21);
		panelPrint.Controls.Add(label19);
		panelPrint.Controls.Add(txtDaysFlush);
		panelPrint.Controls.Add(cmbUserInputOption);
		panelPrint.Controls.Add(cbRotate);
		panelPrint.Controls.Add(pHywebOnly);
		panelPrint.Controls.Add(txtTestPrintNum);
		panelPrint.Controls.Add(label14);
		panelPrint.Controls.Add(btnTestPrint);
		panelPrint.Controls.Add(cmbDLAll);
		panelPrint.Controls.Add(label13);
		panelPrint.Controls.Add(txtUserInputDefault);
		panelPrint.Controls.Add(label12);
		panelPrint.Controls.Add(cmbShowNDays);
		panelPrint.Controls.Add(label11);
		panelPrint.Controls.Add(txtExpDateDesc);
		panelPrint.Controls.Add(label20);
		panelPrint.Controls.Add(label10);
		panelPrint.Controls.Add(cbIgnoreUserInputDesc);
		panelPrint.Controls.Add(txtUserInputDesc);
		panelPrint.Controls.Add(label9);
		panelPrint.Controls.Add(cbUserInput);
		panelPrint.Controls.Add(label8);
		panelPrint.Controls.Add(txtExpDateAccum);
		panelPrint.Controls.Add(label7);
		panelPrint.Controls.Add(cbShowPackDate);
		panelPrint.Controls.Add(cbExpDate);
		panelPrint.Controls.Add(label6);
		panelPrint.Controls.Add(btnSelectDisplayListItem);
		panelPrint.Controls.Add(label5);
		panelPrint.Controls.Add(cmbDefaultPrinter);
		panelPrint.Controls.Add(label4);
		panelPrint.Controls.Add(btnSelectPrintFormat);
		panelPrint.Controls.Add(lblPrintFormatSelected);
		panelPrint.Controls.Add(label3);
		panelPrint.Controls.Add(cmbShowHistory);
		panelPrint.Controls.Add(cmbDaysFlush);
		panelPrint.Controls.Add(btnCancel);
		panelPrint.Controls.Add(btnChangeSetting);
		panelPrint.Controls.Add(label2);
		panelPrint.Controls.Add(label1);
		panelPrint.Location = new System.Drawing.Point(2, 3);
		panelPrint.Name = "panelPrint";
		panelPrint.Size = new System.Drawing.Size(514, 471);
		panelPrint.TabIndex = 2;
		label21.AutoSize = true;
		label21.Font = new System.Drawing.Font("新細明體", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label21.ForeColor = System.Drawing.Color.Red;
		label21.Location = new System.Drawing.Point(328, 7);
		label21.Name = "label21";
		label21.Size = new System.Drawing.Size(170, 13);
		label21.TabIndex = 62;
		label21.Text = "(註銷天數允許最大值為90天)";
		label19.AutoSize = true;
		label19.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label19.Location = new System.Drawing.Point(299, 7);
		label19.Name = "label19";
		label19.Size = new System.Drawing.Size(24, 16);
		label19.TabIndex = 61;
		label19.Text = "天";
		txtDaysFlush.Location = new System.Drawing.Point(193, 4);
		txtDaysFlush.Name = "txtDaysFlush";
		txtDaysFlush.Size = new System.Drawing.Size(100, 22);
		txtDaysFlush.TabIndex = 60;
		txtDaysFlush.TextChanged += new System.EventHandler(txtDaysFlush_TextChanged);
		cmbUserInputOption.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		cmbUserInputOption.FormattingEnabled = true;
		cmbUserInputOption.Items.AddRange(new object[2]
		{
			"自訂欄位",
			"包裝規格"
		});
		cmbUserInputOption.Location = new System.Drawing.Point(257, 314);
		cmbUserInputOption.Name = "cmbUserInputOption";
		cmbUserInputOption.Size = new System.Drawing.Size(106, 20);
		cmbUserInputOption.TabIndex = 59;
		cmbUserInputOption.SelectedIndexChanged += new System.EventHandler(cmbUserInputOption_SelectedIndexChanged);
		cbRotate.AutoSize = true;
		cbRotate.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		cbRotate.Location = new System.Drawing.Point(384, 258);
		cbRotate.Name = "cbRotate";
		cbRotate.Size = new System.Drawing.Size(123, 20);
		cbRotate.TabIndex = 58;
		cbRotate.Text = "90度轉向列印";
		cbRotate.UseVisualStyleBackColor = true;
		cbRotate.Visible = false;
		pHywebOnly.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		pHywebOnly.Controls.Add(btnHywebOnlyDesc);
		pHywebOnly.Controls.Add(label17);
		pHywebOnly.Controls.Add(txtDY);
		pHywebOnly.Controls.Add(label18);
		pHywebOnly.Controls.Add(txtDX);
		pHywebOnly.Controls.Add(label16);
		pHywebOnly.Controls.Add(txtQY);
		pHywebOnly.Controls.Add(label15);
		pHywebOnly.Controls.Add(txtQX);
		pHywebOnly.Location = new System.Drawing.Point(364, 284);
		pHywebOnly.Name = "pHywebOnly";
		pHywebOnly.Size = new System.Drawing.Size(133, 82);
		pHywebOnly.TabIndex = 57;
		pHywebOnly.Visible = false;
		btnHywebOnlyDesc.Font = new System.Drawing.Font("新細明體", 8f);
		btnHywebOnlyDesc.Location = new System.Drawing.Point(3, 55);
		btnHywebOnlyDesc.Name = "btnHywebOnlyDesc";
		btnHywebOnlyDesc.Size = new System.Drawing.Size(120, 20);
		btnHywebOnlyDesc.TabIndex = 55;
		btnHywebOnlyDesc.Text = "說明";
		btnHywebOnlyDesc.UseVisualStyleBackColor = true;
		btnHywebOnlyDesc.Click += new System.EventHandler(btnHywebOnlyDesc_Click);
		label17.AutoSize = true;
		label17.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label17.Font = new System.Drawing.Font("新細明體", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label17.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label17.Location = new System.Drawing.Point(66, 29);
		label17.MaximumSize = new System.Drawing.Size(25, 0);
		label17.MinimumSize = new System.Drawing.Size(25, 0);
		label17.Name = "label17";
		label17.Padding = new System.Windows.Forms.Padding(2, 5, 2, 5);
		label17.Size = new System.Drawing.Size(25, 22);
		label17.TabIndex = 42;
		label17.Text = "dy";
		txtDY.Location = new System.Drawing.Point(93, 29);
		txtDY.Name = "txtDY";
		txtDY.Size = new System.Drawing.Size(30, 22);
		txtDY.TabIndex = 41;
		txtDY.Text = "0";
		label18.AutoSize = true;
		label18.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label18.Font = new System.Drawing.Font("新細明體", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label18.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label18.Location = new System.Drawing.Point(3, 29);
		label18.MaximumSize = new System.Drawing.Size(25, 0);
		label18.MinimumSize = new System.Drawing.Size(25, 0);
		label18.Name = "label18";
		label18.Padding = new System.Windows.Forms.Padding(2, 5, 2, 5);
		label18.Size = new System.Drawing.Size(25, 22);
		label18.TabIndex = 40;
		label18.Text = "dx";
		txtDX.Location = new System.Drawing.Point(30, 29);
		txtDX.Name = "txtDX";
		txtDX.Size = new System.Drawing.Size(30, 22);
		txtDX.TabIndex = 39;
		txtDX.Text = "0";
		label16.AutoSize = true;
		label16.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label16.Font = new System.Drawing.Font("新細明體", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label16.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label16.Location = new System.Drawing.Point(66, 3);
		label16.MaximumSize = new System.Drawing.Size(25, 0);
		label16.MinimumSize = new System.Drawing.Size(25, 0);
		label16.Name = "label16";
		label16.Padding = new System.Windows.Forms.Padding(2, 5, 2, 5);
		label16.Size = new System.Drawing.Size(25, 22);
		label16.TabIndex = 38;
		label16.Text = "qy";
		txtQY.Location = new System.Drawing.Point(93, 3);
		txtQY.Name = "txtQY";
		txtQY.Size = new System.Drawing.Size(30, 22);
		txtQY.TabIndex = 37;
		txtQY.Text = "0";
		label15.AutoSize = true;
		label15.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label15.Font = new System.Drawing.Font("新細明體", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label15.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label15.Location = new System.Drawing.Point(3, 3);
		label15.MaximumSize = new System.Drawing.Size(25, 0);
		label15.MinimumSize = new System.Drawing.Size(25, 0);
		label15.Name = "label15";
		label15.Padding = new System.Windows.Forms.Padding(2, 5, 2, 5);
		label15.Size = new System.Drawing.Size(25, 22);
		label15.TabIndex = 36;
		label15.Text = "qx";
		txtQX.Location = new System.Drawing.Point(30, 3);
		txtQX.Name = "txtQX";
		txtQX.Size = new System.Drawing.Size(30, 22);
		txtQX.TabIndex = 0;
		txtQX.Text = "0";
		txtTestPrintNum.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		txtTestPrintNum.Location = new System.Drawing.Point(191, 394);
		txtTestPrintNum.Name = "txtTestPrintNum";
		txtTestPrintNum.Size = new System.Drawing.Size(167, 27);
		txtTestPrintNum.TabIndex = 56;
		txtTestPrintNum.Text = "1";
		txtTestPrintNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		label14.AutoSize = true;
		label14.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label14.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label14.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label14.Location = new System.Drawing.Point(3, 394);
		label14.MaximumSize = new System.Drawing.Size(200, 0);
		label14.MinimumSize = new System.Drawing.Size(183, 0);
		label14.Name = "label14";
		label14.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label14.Size = new System.Drawing.Size(183, 26);
		label14.TabIndex = 55;
		label14.Text = "測試列印張數";
		btnTestPrint.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		btnTestPrint.Location = new System.Drawing.Point(364, 390);
		btnTestPrint.Name = "btnTestPrint";
		btnTestPrint.Size = new System.Drawing.Size(99, 31);
		btnTestPrint.TabIndex = 54;
		btnTestPrint.Text = "列印測試";
		btnTestPrint.UseVisualStyleBackColor = true;
		btnTestPrint.Click += new System.EventHandler(btnTestPrint_Click);
		cmbDLAll.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		cmbDLAll.Font = new System.Drawing.Font("新細明體", 12f);
		cmbDLAll.FormattingEnabled = true;
		cmbDLAll.Items.AddRange(new object[2]
		{
			"是",
			"否"
		});
		cmbDLAll.Location = new System.Drawing.Point(192, 87);
		cmbDLAll.Name = "cmbDLAll";
		cmbDLAll.Size = new System.Drawing.Size(314, 24);
		cmbDLAll.TabIndex = 53;
		label13.AutoSize = true;
		label13.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label13.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label13.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label13.Location = new System.Drawing.Point(3, 86);
		label13.MaximumSize = new System.Drawing.Size(200, 0);
		label13.MinimumSize = new System.Drawing.Size(183, 0);
		label13.Name = "label13";
		label13.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label13.Size = new System.Drawing.Size(183, 26);
		label13.TabIndex = 52;
		label13.Text = "預設標籤全部下載";
		txtUserInputDefault.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		txtUserInputDefault.Location = new System.Drawing.Point(191, 337);
		txtUserInputDefault.Name = "txtUserInputDefault";
		txtUserInputDefault.Size = new System.Drawing.Size(167, 27);
		txtUserInputDefault.TabIndex = 51;
		label12.AutoSize = true;
		label12.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label12.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label12.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label12.Location = new System.Drawing.Point(3, 338);
		label12.MaximumSize = new System.Drawing.Size(200, 0);
		label12.MinimumSize = new System.Drawing.Size(183, 0);
		label12.Name = "label12";
		label12.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label12.Size = new System.Drawing.Size(183, 26);
		label12.TabIndex = 50;
		label12.Text = "標籤自訂欄位預設值";
		cmbShowNDays.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		cmbShowNDays.Font = new System.Drawing.Font("新細明體", 12f);
		cmbShowNDays.FormattingEnabled = true;
		cmbShowNDays.Items.AddRange(new object[4]
		{
			"不限",
			"1",
			"2",
			"3"
		});
		cmbShowNDays.Location = new System.Drawing.Point(192, 31);
		cmbShowNDays.Name = "cmbShowNDays";
		cmbShowNDays.Size = new System.Drawing.Size(314, 24);
		cmbShowNDays.TabIndex = 49;
		label11.AutoSize = true;
		label11.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label11.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label11.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label11.Location = new System.Drawing.Point(3, 30);
		label11.MaximumSize = new System.Drawing.Size(200, 0);
		label11.MinimumSize = new System.Drawing.Size(183, 0);
		label11.Name = "label11";
		label11.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label11.Size = new System.Drawing.Size(183, 26);
		label11.TabIndex = 48;
		label11.Text = "顯示未來天數";
		txtExpDateDesc.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		txtExpDateDesc.Location = new System.Drawing.Point(191, 170);
		txtExpDateDesc.Name = "txtExpDateDesc";
		txtExpDateDesc.Size = new System.Drawing.Size(315, 27);
		txtExpDateDesc.TabIndex = 47;
		label20.AutoSize = true;
		label20.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label20.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label20.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label20.Location = new System.Drawing.Point(3, 142);
		label20.MaximumSize = new System.Drawing.Size(200, 0);
		label20.MinimumSize = new System.Drawing.Size(183, 0);
		label20.Name = "label20";
		label20.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label20.Size = new System.Drawing.Size(183, 26);
		label20.TabIndex = 46;
		label20.Text = "包裝日期顯示設定";
		label10.AutoSize = true;
		label10.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label10.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label10.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label10.Location = new System.Drawing.Point(3, 170);
		label10.MaximumSize = new System.Drawing.Size(200, 0);
		label10.MinimumSize = new System.Drawing.Size(183, 0);
		label10.Name = "label10";
		label10.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label10.Size = new System.Drawing.Size(183, 26);
		label10.TabIndex = 46;
		label10.Text = "保存日期顯示名稱";
		cbIgnoreUserInputDesc.AutoSize = true;
		cbIgnoreUserInputDesc.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		cbIgnoreUserInputDesc.Location = new System.Drawing.Point(364, 368);
		cbIgnoreUserInputDesc.Name = "cbIgnoreUserInputDesc";
		cbIgnoreUserInputDesc.Size = new System.Drawing.Size(107, 20);
		cbIgnoreUserInputDesc.TabIndex = 45;
		cbIgnoreUserInputDesc.Text = "不顯示名稱";
		cbIgnoreUserInputDesc.UseVisualStyleBackColor = true;
		cbIgnoreUserInputDesc.CheckedChanged += new System.EventHandler(cbIgnoreUserInputDesc_CheckedChanged);
		txtUserInputDesc.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		txtUserInputDesc.Location = new System.Drawing.Point(191, 366);
		txtUserInputDesc.Name = "txtUserInputDesc";
		txtUserInputDesc.Size = new System.Drawing.Size(167, 27);
		txtUserInputDesc.TabIndex = 44;
		label9.AutoSize = true;
		label9.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label9.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label9.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label9.Location = new System.Drawing.Point(3, 366);
		label9.MaximumSize = new System.Drawing.Size(200, 0);
		label9.MinimumSize = new System.Drawing.Size(183, 0);
		label9.Name = "label9";
		label9.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label9.Size = new System.Drawing.Size(183, 26);
		label9.TabIndex = 43;
		label9.Text = "標籤自訂欄位名稱";
		cbUserInput.AutoSize = true;
		cbUserInput.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		cbUserInput.Location = new System.Drawing.Point(192, 315);
		cbUserInput.Name = "cbUserInput";
		cbUserInput.Size = new System.Drawing.Size(59, 20);
		cbUserInput.TabIndex = 42;
		cbUserInput.Text = "啟用";
		cbUserInput.UseVisualStyleBackColor = true;
		cbUserInput.CheckedChanged += new System.EventHandler(cbUserInput_CheckedChanged);
		label8.AutoSize = true;
		label8.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label8.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label8.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label8.Location = new System.Drawing.Point(3, 310);
		label8.MaximumSize = new System.Drawing.Size(200, 0);
		label8.MinimumSize = new System.Drawing.Size(183, 0);
		label8.Name = "label8";
		label8.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label8.Size = new System.Drawing.Size(183, 26);
		label8.TabIndex = 41;
		label8.Text = "標籤自訂欄位包裝規格";
		txtExpDateAccum.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		txtExpDateAccum.Location = new System.Drawing.Point(192, 225);
		txtExpDateAccum.Name = "txtExpDateAccum";
		txtExpDateAccum.Size = new System.Drawing.Size(314, 27);
		txtExpDateAccum.TabIndex = 40;
		label7.AutoSize = true;
		label7.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label7.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label7.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label7.Location = new System.Drawing.Point(3, 226);
		label7.MaximumSize = new System.Drawing.Size(200, 0);
		label7.MinimumSize = new System.Drawing.Size(183, 0);
		label7.Name = "label7";
		label7.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label7.Size = new System.Drawing.Size(183, 26);
		label7.TabIndex = 39;
		label7.Text = "保存期限預設天數";
		cbShowPackDate.AutoSize = true;
		cbShowPackDate.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		cbShowPackDate.Location = new System.Drawing.Point(192, 148);
		cbShowPackDate.Name = "cbShowPackDate";
		cbShowPackDate.Size = new System.Drawing.Size(59, 20);
		cbShowPackDate.TabIndex = 38;
		cbShowPackDate.Text = "顯示";
		cbShowPackDate.UseVisualStyleBackColor = true;
		cbShowPackDate.CheckedChanged += new System.EventHandler(cbExpDate_CheckedChanged);
		cbExpDate.AutoSize = true;
		cbExpDate.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		cbExpDate.Location = new System.Drawing.Point(192, 201);
		cbExpDate.Name = "cbExpDate";
		cbExpDate.Size = new System.Drawing.Size(59, 20);
		cbExpDate.TabIndex = 38;
		cbExpDate.Text = "啟用";
		cbExpDate.UseVisualStyleBackColor = true;
		cbExpDate.CheckedChanged += new System.EventHandler(cbExpDate_CheckedChanged);
		label6.AutoSize = true;
		label6.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label6.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label6.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label6.Location = new System.Drawing.Point(3, 198);
		label6.MaximumSize = new System.Drawing.Size(200, 0);
		label6.MinimumSize = new System.Drawing.Size(183, 0);
		label6.Name = "label6";
		label6.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label6.Size = new System.Drawing.Size(183, 26);
		label6.TabIndex = 37;
		label6.Text = "保存期限設定";
		btnSelectDisplayListItem.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		btnSelectDisplayListItem.Location = new System.Drawing.Point(192, 283);
		btnSelectDisplayListItem.Name = "btnSelectDisplayListItem";
		btnSelectDisplayListItem.Size = new System.Drawing.Size(66, 26);
		btnSelectDisplayListItem.TabIndex = 36;
		btnSelectDisplayListItem.Text = "選擇";
		btnSelectDisplayListItem.UseVisualStyleBackColor = true;
		btnSelectDisplayListItem.Click += new System.EventHandler(btnSelectDisplayListItem_Click);
		label5.AutoSize = true;
		label5.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label5.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label5.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label5.Location = new System.Drawing.Point(3, 282);
		label5.MaximumSize = new System.Drawing.Size(200, 0);
		label5.MinimumSize = new System.Drawing.Size(183, 0);
		label5.Name = "label5";
		label5.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label5.Size = new System.Drawing.Size(183, 26);
		label5.TabIndex = 35;
		label5.Text = "標籤清單顯示欄位";
		cmbDefaultPrinter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		cmbDefaultPrinter.Font = new System.Drawing.Font("新細明體", 12f);
		cmbDefaultPrinter.FormattingEnabled = true;
		cmbDefaultPrinter.Location = new System.Drawing.Point(192, 115);
		cmbDefaultPrinter.Name = "cmbDefaultPrinter";
		cmbDefaultPrinter.Size = new System.Drawing.Size(314, 24);
		cmbDefaultPrinter.TabIndex = 34;
		label4.AutoSize = true;
		label4.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label4.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label4.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label4.Location = new System.Drawing.Point(3, 114);
		label4.MaximumSize = new System.Drawing.Size(200, 0);
		label4.MinimumSize = new System.Drawing.Size(183, 0);
		label4.Name = "label4";
		label4.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label4.Size = new System.Drawing.Size(183, 26);
		label4.TabIndex = 33;
		label4.Text = "標籤列印預設印表機";
		btnSelectPrintFormat.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		btnSelectPrintFormat.Location = new System.Drawing.Point(192, 254);
		btnSelectPrintFormat.Name = "btnSelectPrintFormat";
		btnSelectPrintFormat.Size = new System.Drawing.Size(66, 26);
		btnSelectPrintFormat.TabIndex = 32;
		btnSelectPrintFormat.Text = "選擇";
		btnSelectPrintFormat.UseVisualStyleBackColor = true;
		btnSelectPrintFormat.Click += new System.EventHandler(btnSelectPrintFormat_Click);
		lblPrintFormatSelected.AutoSize = true;
		lblPrintFormatSelected.BackColor = System.Drawing.SystemColors.ControlLight;
		lblPrintFormatSelected.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		lblPrintFormatSelected.ForeColor = System.Drawing.SystemColors.ControlText;
		lblPrintFormatSelected.Location = new System.Drawing.Point(264, 255);
		lblPrintFormatSelected.MaximumSize = new System.Drawing.Size(240, 0);
		lblPrintFormatSelected.MinimumSize = new System.Drawing.Size(240, 0);
		lblPrintFormatSelected.Name = "lblPrintFormatSelected";
		lblPrintFormatSelected.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		lblPrintFormatSelected.Size = new System.Drawing.Size(240, 26);
		lblPrintFormatSelected.TabIndex = 31;
		lblPrintFormatSelected.Text = "標籤列印樣式";
		label3.AutoSize = true;
		label3.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label3.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label3.Location = new System.Drawing.Point(3, 254);
		label3.MaximumSize = new System.Drawing.Size(200, 0);
		label3.MinimumSize = new System.Drawing.Size(183, 0);
		label3.Name = "label3";
		label3.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label3.Size = new System.Drawing.Size(183, 26);
		label3.TabIndex = 30;
		label3.Text = "標籤列印樣式";
		cmbShowHistory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		cmbShowHistory.Font = new System.Drawing.Font("新細明體", 12f);
		cmbShowHistory.FormattingEnabled = true;
		cmbShowHistory.Items.AddRange(new object[2]
		{
			"顯示",
			"不顯示"
		});
		cmbShowHistory.Location = new System.Drawing.Point(192, 59);
		cmbShowHistory.Name = "cmbShowHistory";
		cmbShowHistory.Size = new System.Drawing.Size(314, 24);
		cmbShowHistory.TabIndex = 29;
		cmbShowHistory.SelectedIndexChanged += new System.EventHandler(cmbShowHistory_SelectedIndexChanged);
		cmbDaysFlush.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		cmbDaysFlush.Font = new System.Drawing.Font("新細明體", 12f);
		cmbDaysFlush.FormattingEnabled = true;
		cmbDaysFlush.Items.AddRange(new object[7]
		{
			"1",
			"2",
			"3",
			"4",
			"5",
			"6",
			"7"
		});
		cmbDaysFlush.Location = new System.Drawing.Point(364, 437);
		cmbDaysFlush.Name = "cmbDaysFlush";
		cmbDaysFlush.Size = new System.Drawing.Size(117, 24);
		cmbDaysFlush.TabIndex = 28;
		cmbDaysFlush.Visible = false;
		cmbDaysFlush.SelectedIndexChanged += new System.EventHandler(cmbDaysFlush_SelectedIndexChanged);
		btnCancel.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		btnCancel.Location = new System.Drawing.Point(277, 433);
		btnCancel.Name = "btnCancel";
		btnCancel.Size = new System.Drawing.Size(80, 31);
		btnCancel.TabIndex = 20;
		btnCancel.Text = "取消";
		btnCancel.UseVisualStyleBackColor = true;
		btnCancel.Click += new System.EventHandler(btnCancel_Click);
		btnChangeSetting.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		btnChangeSetting.Location = new System.Drawing.Point(191, 433);
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
		label2.Location = new System.Drawing.Point(3, 58);
		label2.MaximumSize = new System.Drawing.Size(200, 0);
		label2.MinimumSize = new System.Drawing.Size(183, 0);
		label2.Name = "label2";
		label2.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label2.Size = new System.Drawing.Size(183, 26);
		label2.TabIndex = 2;
		label2.Text = "預設顯示逾期申請資訊";
		label1.AutoSize = true;
		label1.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label1.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label1.Location = new System.Drawing.Point(3, 2);
		label1.MaximumSize = new System.Drawing.Size(200, 0);
		label1.MinimumSize = new System.Drawing.Size(183, 0);
		label1.Name = "label1";
		label1.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label1.Size = new System.Drawing.Size(183, 26);
		label1.TabIndex = 0;
		label1.Text = "註銷天數";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(522, 477);
		base.Controls.Add(panelPrint);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "frmSetParameter";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		Text = "參數設定";
		base.Load += new System.EventHandler(frmSetParameter_Load);
		panelPrint.ResumeLayout(false);
		panelPrint.PerformLayout();
		pHywebOnly.ResumeLayout(false);
		pHywebOnly.PerformLayout();
		ResumeLayout(false);
	}
}
