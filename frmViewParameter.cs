using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using 離線列印Client程式;

public class frmViewParameter : Form
{
	private IContainer components;

	private PictureBox pictureBox1;

	private Button btnChangeSetting;

	public frmViewParameter()
	{
		InitializeComponent();
	}

	private void btnChangeSetting_Click(object sender, EventArgs e)
	{
		Close();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(離線列印Client程式.frmViewParameter));
		btnChangeSetting = new System.Windows.Forms.Button();
		pictureBox1 = new System.Windows.Forms.PictureBox();
		((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
		SuspendLayout();
		btnChangeSetting.Font = new System.Drawing.Font("新細明體", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 136);
		btnChangeSetting.Location = new System.Drawing.Point(565, 1233);
		btnChangeSetting.Name = "btnChangeSetting";
		btnChangeSetting.Size = new System.Drawing.Size(80, 31);
		btnChangeSetting.TabIndex = 24;
		btnChangeSetting.Text = "確認";
		btnChangeSetting.UseVisualStyleBackColor = true;
		btnChangeSetting.Click += new System.EventHandler(btnChangeSetting_Click);
		pictureBox1.Image = (System.Drawing.Image)resources.GetObject("pictureBox1.Image");
		pictureBox1.Location = new System.Drawing.Point(2, 2);
		pictureBox1.Name = "pictureBox1";
		pictureBox1.Size = new System.Drawing.Size(1330, 1234);
		pictureBox1.TabIndex = 0;
		pictureBox1.TabStop = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		AutoScroll = true;
		base.ClientSize = new System.Drawing.Size(980, 665);
		base.Controls.Add(btnChangeSetting);
		base.Controls.Add(pictureBox1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "frmViewParameter";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		Text = "樣式預覽";
		((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
		ResumeLayout(false);
	}
}
