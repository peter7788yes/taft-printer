using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using T00SharedLibraryDotNet20;
using 離線列印Client程式;

public class frmClearHistory : Form
{
	private IContainer components;

	private Panel panelPrint;

	private Button btnCancel;

	private Button btnChangeSetting;

	private DateTimePicker dtToClear;

	private Label label1;

	private Label label2;

	private StatusStrip ssDesc;

	private ToolStripStatusLabel ssLblDesc;

	public frmClearHistory()
	{
		InitializeComponent();
	}

	private void btnChangeSetting_Click(object sender, EventArgs e)
	{
		try
		{
			ssLblDesc.Text = "準備刪除歷史資料";
			RemoveHistory(dtToClear.Value);
			base.DialogResult = DialogResult.OK;
		}
		catch (Exception)
		{
			MessageBox.Show("清除歷史紀錄失敗。");
		}
		finally
		{
			Close();
		}
	}

	private void btnCancel_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void frmClearHistory_Load(object sender, EventArgs e)
	{
		ssLblDesc.Text = string.Empty;
		dtToClear.Value = DateTime.Now.AddMonths(-3);
	}

	private void RemoveHistory(DateTime endDate)
	{
		string arg = endDate.ToString("yyyy/MM/dd");
		int num;
		do
		{
			num = Convert.ToInt32(DataBaseUtilities.DBOperation(Program.ConnectionString, string.Format("select count(*) from ResumeHistory where PackDate <= #{0}#", arg), null, CommandOperationType.ExecuteScalar));
			if (num > 0)
			{
				ssLblDesc.Text = string.Format("目前尚有{0}筆資料準備刪除，請稍候", num);
			}
			else
			{
				ssLblDesc.Text = "刪除成功!";
			}
			DataBaseUtilities.DBOperation(Program.ConnectionString, string.Format("delete * from ResumeHistory where initPrintCodeSrt IN ( SELECT TOP 500 initPrintCodeSrt FROM ResumeHistory WHERE PackDate <= #{0}# ORDER BY PackDate ASC ) ", arg), null, CommandOperationType.ExecuteNonQuery);
			Application.DoEvents();
		}
		while (num > 0);
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(離線列印Client程式.frmClearHistory));
		panelPrint = new System.Windows.Forms.Panel();
		label2 = new System.Windows.Forms.Label();
		label1 = new System.Windows.Forms.Label();
		dtToClear = new System.Windows.Forms.DateTimePicker();
		btnCancel = new System.Windows.Forms.Button();
		btnChangeSetting = new System.Windows.Forms.Button();
		ssDesc = new System.Windows.Forms.StatusStrip();
		ssLblDesc = new System.Windows.Forms.ToolStripStatusLabel();
		panelPrint.SuspendLayout();
		ssDesc.SuspendLayout();
		SuspendLayout();
		panelPrint.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		panelPrint.Controls.Add(label2);
		panelPrint.Controls.Add(label1);
		panelPrint.Controls.Add(dtToClear);
		panelPrint.Controls.Add(btnCancel);
		panelPrint.Controls.Add(btnChangeSetting);
		panelPrint.Location = new System.Drawing.Point(2, 3);
		panelPrint.Name = "panelPrint";
		panelPrint.Size = new System.Drawing.Size(344, 92);
		panelPrint.TabIndex = 2;
		label2.AutoSize = true;
		label2.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label2.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label2.Location = new System.Drawing.Point(225, 9);
		label2.MaximumSize = new System.Drawing.Size(200, 0);
		label2.MinimumSize = new System.Drawing.Size(80, 0);
		label2.Name = "label2";
		label2.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label2.Size = new System.Drawing.Size(103, 26);
		label2.TabIndex = 23;
		label2.Text = "之前的資料";
		label1.AutoSize = true;
		label1.BackColor = System.Drawing.SystemColors.AppWorkspace;
		label1.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		label1.Location = new System.Drawing.Point(8, 9);
		label1.MaximumSize = new System.Drawing.Size(200, 0);
		label1.MinimumSize = new System.Drawing.Size(80, 0);
		label1.Name = "label1";
		label1.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
		label1.Size = new System.Drawing.Size(80, 26);
		label1.TabIndex = 22;
		label1.Text = "清空";
		dtToClear.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		dtToClear.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		dtToClear.Location = new System.Drawing.Point(94, 9);
		dtToClear.Name = "dtToClear";
		dtToClear.Size = new System.Drawing.Size(125, 27);
		dtToClear.TabIndex = 21;
		btnCancel.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		btnCancel.Location = new System.Drawing.Point(168, 47);
		btnCancel.Name = "btnCancel";
		btnCancel.Size = new System.Drawing.Size(80, 31);
		btnCancel.TabIndex = 20;
		btnCancel.Text = "取消";
		btnCancel.UseVisualStyleBackColor = true;
		btnCancel.Click += new System.EventHandler(btnCancel_Click);
		btnChangeSetting.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		btnChangeSetting.Location = new System.Drawing.Point(82, 47);
		btnChangeSetting.Name = "btnChangeSetting";
		btnChangeSetting.Size = new System.Drawing.Size(80, 31);
		btnChangeSetting.TabIndex = 19;
		btnChangeSetting.Text = "確認";
		btnChangeSetting.UseVisualStyleBackColor = true;
		btnChangeSetting.Click += new System.EventHandler(btnChangeSetting_Click);
		ssDesc.Items.AddRange(new System.Windows.Forms.ToolStripItem[1]
		{
			ssLblDesc
		});
		ssDesc.Location = new System.Drawing.Point(0, 98);
		ssDesc.Name = "ssDesc";
		ssDesc.Size = new System.Drawing.Size(351, 22);
		ssDesc.TabIndex = 3;
		ssDesc.Text = "statusStrip1";
		ssLblDesc.Name = "ssLblDesc";
		ssLblDesc.Size = new System.Drawing.Size(0, 17);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(351, 120);
		base.Controls.Add(ssDesc);
		base.Controls.Add(panelPrint);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "frmClearHistory";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		Text = "清除歷史資料";
		base.Load += new System.EventHandler(frmClearHistory_Load);
		panelPrint.ResumeLayout(false);
		panelPrint.PerformLayout();
		ssDesc.ResumeLayout(false);
		ssDesc.PerformLayout();
		ResumeLayout(false);
		PerformLayout();
	}
}
