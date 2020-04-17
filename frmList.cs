using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using T00SharedLibraryDotNet20;
using 離線列印Client程式;
using 離線列印Client程式.L1_1O;
using 離線列印Client程式.OfflinePrintWebService;

public class frmList : Form
{
	public class DataGridViewDisableButtonColumn : DataGridViewButtonColumn
	{
		public DataGridViewDisableButtonColumn()
		{
			CellTemplate = new DataGridViewDisableButtonCell();
		}
	}

	public class DataGridViewDisableButtonCell : DataGridViewButtonCell
	{
		private bool enabledValue;

		public bool Enabled
		{
			get
			{
				return enabledValue;
			}
			set
			{
				enabledValue = value;
			}
		}

		public override object Clone()
		{
			DataGridViewDisableButtonCell obj = (DataGridViewDisableButtonCell)base.Clone();
			obj.Enabled = Enabled;
			return obj;
		}

		public DataGridViewDisableButtonCell()
		{
			enabledValue = true;
		}

		protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates elementState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
		{
			if (!enabledValue)
			{
				if ((paintParts & DataGridViewPaintParts.Background) == DataGridViewPaintParts.Background)
				{
					SolidBrush solidBrush = new SolidBrush(cellStyle.BackColor);
					graphics.FillRectangle(solidBrush, cellBounds);
					solidBrush.Dispose();
				}
				if ((paintParts & DataGridViewPaintParts.Border) == DataGridViewPaintParts.Border)
				{
					PaintBorder(graphics, clipBounds, cellBounds, cellStyle, advancedBorderStyle);
				}
				Rectangle bounds = cellBounds;
				Rectangle rectangle = BorderWidths(advancedBorderStyle);
				bounds.X += rectangle.X;
				bounds.Y += rectangle.Y;
				bounds.Height -= rectangle.Height;
				bounds.Width -= rectangle.Width;
				ButtonRenderer.DrawButton(graphics, bounds, PushButtonState.Disabled);
				if (base.FormattedValue is string)
				{
					TextRenderer.DrawText(graphics, (string)base.FormattedValue, base.DataGridView.Font, bounds, SystemColors.GrayText);
				}
			}
			else
			{
				base.Paint(graphics, clipBounds, cellBounds, rowIndex, elementState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
			}
		}
	}

	private int _SortingColIndex = -1;

	private ListSortDirection _SortOrder;

	private string _SqlTimeCond = string.Empty;

	private DateTime _SrtTimeRange = DateTime.Now.AddDays(-5.0);

	private DateTime _EndTimeRange = DateTime.Now;

	private IContainer components;

	private DataGridView dgvPrint;

	private BindingSource bs;

	private BindingNavigator bn;

	private ToolStripLabel bindingNavigatorCountItem;

	private ToolStripButton bindingNavigatorMoveFirstItem;

	private ToolStripButton bindingNavigatorMovePreviousItem;

	private ToolStripSeparator bindingNavigatorSeparator;

	private ToolStripSeparator bindingNavigatorSeparator1;

	private ToolStripButton bindingNavigatorMoveNextItem;

	private ToolStripButton bindingNavigatorMoveLastItem;

	private ToolStripSeparator bindingNavigatorSeparator2;

	private ToolStripTextBox bindingNavigatorPositionItem;

	private ToolStripButton btnShowHistory;

	private ToolStripLabel lblTimeRange;

	private ToolStripButton btnSetTimeRange;

	public frmList()
	{
		InitializeComponent();
	}

	private void DisEnableBtns()
	{
		foreach (DataGridViewRow item in (IEnumerable)dgvPrint.Rows)
		{
			if (item.Cells["AvailableForPrinting"].Value.ToString() == "0")
			{
				((DataGridViewDisableButtonCell)item.Cells["列印"]).Enabled = false;
			}
		}
	}

	private void InitdgvPrint()
	{
		dgvPrint.DataSource = LoadDataFromDB();
		dgvPrint.Columns[0].HeaderText = "上傳日期";
		dgvPrint.Columns[0].Visible = ((Program.DisplayListItems & 1) > 0);
		dgvPrint.Columns[1].HeaderText = "包裝日期";
		dgvPrint.Columns[1].Visible = ((Program.DisplayListItems & 2) > 0);
		dgvPrint.Columns[2].HeaderText = "農民";
		dgvPrint.Columns[2].Visible = ((Program.DisplayListItems & 4) > 0);
		dgvPrint.Columns[3].HeaderText = "收貨單位";
		dgvPrint.Columns[3].Visible = ((Program.DisplayListItems & 0x1000) > 0);
		dgvPrint.Columns[4].HeaderText = "品項";
		dgvPrint.Columns[4].Visible = ((Program.DisplayListItems & 8) > 0);
		dgvPrint.Columns[5].HeaderText = "規格";
		dgvPrint.Columns[5].Visible = ((Program.DisplayListItems & 0x10) > 0);
		dgvPrint.Columns[6].HeaderText = "追溯號碼";
		dgvPrint.Columns[6].Visible = ((Program.DisplayListItems & 0x20) > 0);
		dgvPrint.Columns[7].HeaderText = "EAN";
		dgvPrint.Columns[7].Visible = ((Program.DisplayListItems & 0x40) > 0);
		dgvPrint.Columns[8].HeaderText = "二維條碼";
		dgvPrint.Columns[8].Visible = false;
		dgvPrint.Columns[9].HeaderText = "起始號碼";
		dgvPrint.Columns[9].Visible = ((Program.DisplayListItems & 0x80) > 0);
		dgvPrint.Columns[10].HeaderText = "結束號碼";
		dgvPrint.Columns[10].Visible = false;
		dgvPrint.Columns[11].HeaderText = "原栽培編碼";
		dgvPrint.Columns[11].Visible = ((Program.DisplayListItems & 0x100) > 0);
		dgvPrint.Columns[12].HeaderText = "申請數量";
		dgvPrint.Columns[12].Visible = ((Program.DisplayListItems & 0x200) > 0);
		dgvPrint.Columns[13].HeaderText = "保存日期";
		dgvPrint.Columns[13].Visible = false;
		dgvPrint.Columns[14].HeaderText = "啟始列印標籤";
		dgvPrint.Columns[14].Visible = false;
		dgvPrint.Columns[15].HeaderText = "目前或歷史紀錄";
		dgvPrint.Columns[15].Visible = false;
		dgvPrint.Columns[16].HeaderText = "本機履歷ID";
		dgvPrint.Columns[16].Visible = false;
		dgvPrint.Columns[17].HeaderText = "生產者";
		dgvPrint.Columns[17].Visible = false;
		dgvPrint.Columns[18].HeaderText = "原料生產單位";
		dgvPrint.Columns[18].Visible = true;
		dgvPrint.Columns[19].HeaderText = "可列印數";
		dgvPrint.Columns[19].Visible = ((Program.DisplayListItems & 0x400) > 0);
		dgvPrint.Columns[20].HeaderText = "已列印數";
		dgvPrint.Columns[20].Visible = ((Program.DisplayListItems & 0x800) > 0);
		dgvPrint.Width = 988;
		dgvPrint.Columns[0].Width = 76;
		dgvPrint.Columns[1].Width = 78;
		dgvPrint.Columns[2].Width = 60;
		dgvPrint.AutoResizeColumn(3);
		dgvPrint.Columns[4].Width = 92;
		dgvPrint.AutoResizeColumn(5);
		dgvPrint.AutoResizeColumn(6);
		dgvPrint.AutoResizeColumn(7);
		dgvPrint.Columns[8].Width = 0;
		dgvPrint.AutoResizeColumn(9);
		dgvPrint.Columns[10].Width = 0;
		dgvPrint.AutoResizeColumn(11);
		dgvPrint.Columns[12].Width = 57;
		dgvPrint.Columns[13].Width = 0;
		dgvPrint.Columns[14].Width = 0;
		dgvPrint.Columns[15].Width = 0;
		dgvPrint.Columns[16].Width = 0;
		dgvPrint.Columns[17].Width = 0;
		dgvPrint.Columns[18].Width = 57;
		dgvPrint.Columns[19].Width = 57;
		DataGridViewDisableButtonColumn dataGridViewDisableButtonColumn = new DataGridViewDisableButtonColumn();
		dataGridViewDisableButtonColumn.HeaderText = "列印";
		dataGridViewDisableButtonColumn.Name = "列印";
		dataGridViewDisableButtonColumn.Text = "列印";
		dataGridViewDisableButtonColumn.Width = 43;
		dataGridViewDisableButtonColumn.UseColumnTextForButtonValue = true;
		dgvPrint.Columns.Add(dataGridViewDisableButtonColumn);
		DataGridViewDisableButtonColumn dataGridViewDisableButtonColumn2 = new DataGridViewDisableButtonColumn();
		dataGridViewDisableButtonColumn2.HeaderText = "撤銷";
		dataGridViewDisableButtonColumn2.Name = "撤銷";
		dataGridViewDisableButtonColumn2.Text = "撤銷";
		dataGridViewDisableButtonColumn2.Width = 43;
		dataGridViewDisableButtonColumn2.UseColumnTextForButtonValue = true;
		dgvPrint.Columns.Add(dataGridViewDisableButtonColumn2);
		DataGridViewDisableButtonColumn dataGridViewDisableButtonColumn3 = new DataGridViewDisableButtonColumn();
		dataGridViewDisableButtonColumn3.HeaderText = "剩餘張數回沖";
		dataGridViewDisableButtonColumn3.Name = "立即回沖";
		dataGridViewDisableButtonColumn3.Text = "立即回沖";
		dataGridViewDisableButtonColumn3.Width = 63;
		dataGridViewDisableButtonColumn3.UseColumnTextForButtonValue = true;
		dgvPrint.Columns.Add(dataGridViewDisableButtonColumn3);
		foreach (DataGridViewColumn column in dgvPrint.Columns)
		{
			column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			if (column.HeaderText == "申請數量" || column.HeaderText == "可列印數" || column.HeaderText == "已列印數")
			{
				column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopRight;
			}
			else
			{
				column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
			}
		}
		DisEnableBtns();
		DoResizeDGV();
		bs.DataSource = dgvPrint.DataSource;
		bn.BindingSource = bs;
		dgvPrint.EnableHeadersVisualStyles = true;
		bindingNavigatorPositionItem.Text = Convert.ToString(bs.Position + 1);
		dgvPrint.ReadOnly = true;
		dgvPrint.AllowUserToDeleteRows = false;
	}

	private DataTable LoadDataFromDB()
	{
		DataTable dataTable = LoadDataFromResume("c");
		if (Program.ShowHistory)
		{
			dataTable.Merge(LoadDataFromResume("h"), true, MissingSchemaAction.Ignore);
		}
		return dataTable;
	}

	private DataTable LoadDataFromResume(string whichT)
	{
		StringBuilder stringBuilder = new StringBuilder("");
		if (whichT == "c")
		{
			stringBuilder.AppendFormat("SELECT        CreateDate, PackDate,  FarmerName, ReceiveOrg, ProductName, Unit, TraceCode, EanCode, QRCodeUrl,  initPrintCodeSrt, initPrintCodeEnd, PreTraceCode, initAvailablePrint, ExpDate, PrintCodeSrt, 'c' as curOrHis, ResumeID, Producer,SourceProducer ");
			stringBuilder.AppendFormat("FROM\t\t    ResumeCurrent ");
			if (Program.UserSettings.ShowNextNDays > 0)
			{
				stringBuilder.AppendFormat("WHERE\t\t    PackDate <=#{0}# ", DateTime.Now.AddDays(Program.UserSettings.ShowNextNDays).ToString("yyyy/MM/dd"));
			}
			stringBuilder.AppendFormat("GROUP BY      CreateDate, PackDate, FarmerName, ReceiveOrg, ProductName, Unit, TraceCode, EanCode , QRCodeUrl, initPrintCodeSrt, initPrintCodeEnd, PreTraceCode, initAvailablePrint, ExpDate, PrintCodeSrt, ResumeID, Producer,SourceProducer ");
			stringBuilder.AppendFormat("ORDER BY      PackDate Desc, TraceCode ");
			return PutAvailableAndPrintedStatsIntoDT((DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, stringBuilder.ToString(), null, CommandOperationType.ExecuteReaderReturnDataTable));
		}
		stringBuilder.AppendFormat("SELECT \t\tCreateDate, PackDate,  FarmerName, ReceiveOrg, ProductName, Unit, TraceCode, EanCode, QRCodeUrl, initPrintCodeSrt, initPrintCodeEnd, PreTraceCode, initAvailablePrint, ExpDate, ' ' as PrintCodeSrt, 'h' as curOrHis, ResumeID, Producer, '0' as AvailableForPrinting, Sum(TotalPrinted) as AlreadyPrinted ,SourceProducer ");
		stringBuilder.AppendFormat("FROM\t\t    ResumeHistory ");
		stringBuilder.AppendFormat("WHERE         {0} ", _SqlTimeCond);
		stringBuilder.AppendFormat("GROUP BY      CreateDate, PackDate, FarmerName, ReceiveOrg, ProductName, Unit, TraceCode, EanCode , QRCodeUrl, initPrintCodeSrt, initPrintCodeEnd, PreTraceCode, initAvailablePrint, ExpDate, ResumeID, Producer,SourceProducer ");
		stringBuilder.AppendFormat("ORDER BY      PackDate Desc, TraceCode ");
		return (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, stringBuilder.ToString(), null, CommandOperationType.ExecuteReaderReturnDataTable);
	}

	private DataTable PutAvailableAndPrintedStatsIntoDT(DataTable dt)
	{
		if (dt == null)
		{
			return null;
		}
		dt.Columns.Add("AvailableForPrinting");
		dt.Columns.Add("AlreadyPrinted", Type.GetType("System.Int32"));
		for (int i = 0; i < dt.Rows.Count; i++)
		{
			string[] strWhereParameterArray = new string[5]
			{
				dt.Rows[i]["PackDate"].ToString(),
				dt.Rows[i]["FarmerName"].ToString(),
				dt.Rows[i]["ProductName"].ToString(),
				dt.Rows[i]["TraceCode"].ToString(),
				dt.Rows[i]["QRCodeUrl"].ToString()
			};
			dt.Rows[i]["AvailableForPrinting"] = GetAvailablePrintSum(dt.Rows[i]["PackDate"].ToString(), dt.Rows[i]["FarmerName"].ToString(), dt.Rows[i]["ProductName"].ToString(), dt.Rows[i]["TraceCode"].ToString(), dt.Rows[i]["QRCodeUrl"].ToString());
			dt.Rows[i]["AlreadyPrinted"] = DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Select, "SUM(TotalPrinted)", "ResumeCurrent", "PackDate = {0} and FarmerName = {1} and ProductName = {2} and TraceCode = {3} and QRCodeUrl = {4}", "", null, strWhereParameterArray, CommandOperationType.ExecuteScalar).ToString();
		}
		return dt;
	}

	private int GetAvailablePrintSum(string PackDate, string FarmerName, string ProductName, string TraceCode, string QRCodeUrl)
	{
		string[] strWhereParameterArray = new string[5]
		{
			PackDate,
			FarmerName,
			ProductName,
			TraceCode,
			QRCodeUrl
		};
		DataTable dataTable = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Select, "PrintCodeSrt, PrintCodeEnd", "ResumeCurrent", "PackDate = {0} and FarmerName = {1} and ProductName = {2} and TraceCode = {3} and QRCodeUrl = {4}", "", null, strWhereParameterArray, CommandOperationType.ExecuteReaderReturnDataTable);
		if (dataTable == null || dataTable.Rows.Count == 0)
		{
			return 0;
		}
		int num = 0;
		for (int i = 0; i < dataTable.Rows.Count; i++)
		{
			num += Convert.ToInt32(Convert.ToInt64(dataTable.Rows[i][1]) - Convert.ToInt64(dataTable.Rows[i][0]) + 1);
		}
		if (num < 0)
		{
			num = 0;
		}
		return num;
	}

	private void dgvPrint_CellContentClick(object sender, DataGridViewCellEventArgs e)
	{
		if (IsANonHeaderLinkCell(e) || !IsANonHeaderButtonCell(e))
		{
			return;
		}
		if (dgvPrint.Columns[e.ColumnIndex].HeaderText == "撤銷")
		{
			if (dgvPrint.Rows[e.RowIndex].Cells["curOrHis"].Value.ToString() == "c" && Convert.ToInt32(dgvPrint.Rows[e.RowIndex].Cells["AlreadyPrinted"].Value) > 0)
			{
				GoDisposePrint(dgvPrint.Rows[e.RowIndex].Cells["CreateDate"].Value.ToString(), Convert.ToDateTime(dgvPrint.Rows[e.RowIndex].Cells["PackDate"].Value).ToString("yyyy/MM/dd"), dgvPrint.Rows[e.RowIndex].Cells["FarmerName"].Value.ToString(), dgvPrint.Rows[e.RowIndex].Cells["ProductName"].Value.ToString(), dgvPrint.Rows[e.RowIndex].Cells["TraceCode"].Value.ToString(), dgvPrint.Rows[e.RowIndex].Cells["initPrintCodeSrt"].Value.ToString(), dgvPrint.Rows[e.RowIndex].Cells["initPrintCodeEnd"].Value.ToString(), dgvPrint.Rows[e.RowIndex].Cells["AlreadyPrinted"].Value.ToString(), Convert.ToString(dgvPrint.Rows[e.RowIndex].Cells["ResumeID"].Value), false);
			}
			else if (dgvPrint.Rows[e.RowIndex].Cells["curOrHis"].Value.ToString() == "c")
			{
				MessageBox.Show("該筆資料無列印標籤可供撤銷。");
			}
			else if (dgvPrint.Rows[e.RowIndex].Cells["curOrHis"].Value.ToString() == "h")
			{
				GoDisposePrint(dgvPrint.Rows[e.RowIndex].Cells["CreateDate"].Value.ToString(), Convert.ToDateTime(dgvPrint.Rows[e.RowIndex].Cells["PackDate"].Value).ToString("yyyy/MM/dd"), dgvPrint.Rows[e.RowIndex].Cells["FarmerName"].Value.ToString(), dgvPrint.Rows[e.RowIndex].Cells["ProductName"].Value.ToString(), dgvPrint.Rows[e.RowIndex].Cells["TraceCode"].Value.ToString(), dgvPrint.Rows[e.RowIndex].Cells["initPrintCodeSrt"].Value.ToString(), dgvPrint.Rows[e.RowIndex].Cells["initPrintCodeEnd"].Value.ToString(), dgvPrint.Rows[e.RowIndex].Cells["AlreadyPrinted"].Value.ToString(), Convert.ToString(dgvPrint.Rows[e.RowIndex].Cells["ResumeID"].Value), true);
			}
		}
		else if (dgvPrint.Columns[e.ColumnIndex].HeaderText == "列印")
		{
			if (Convert.ToInt32(dgvPrint.Rows[e.RowIndex].Cells["AvailableForPrinting"].Value) != 0)
			{
				GoPrint(dgvPrint.Rows[e.RowIndex].Cells["CreateDate"].Value.ToString(), Convert.ToDateTime(dgvPrint.Rows[e.RowIndex].Cells["PackDate"].Value).ToString("yyyy/MM/dd"), dgvPrint.Rows[e.RowIndex].Cells["FarmerName"].Value.ToString(), dgvPrint.Rows[e.RowIndex].Cells["ProductName"].Value.ToString(), dgvPrint.Rows[e.RowIndex].Cells["TraceCode"].Value.ToString(), dgvPrint.Rows[e.RowIndex].Cells["EanCode"].Value.ToString(), dgvPrint.Rows[e.RowIndex].Cells["QRCodeUrl"].Value.ToString(), dgvPrint.Rows[e.RowIndex].Cells["AvailableForPrinting"].Value.ToString(), dgvPrint.Rows[e.RowIndex].Cells["AlreadyPrinted"].Value.ToString(), dgvPrint.Rows[e.RowIndex].Cells["PrintCodeSrt"].Value.ToString(), DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Select, "top 1 AuthName", "ResumeCurrent", "QRCodeUrl = '" + dgvPrint.Rows[e.RowIndex].Cells["QRCodeUrl"].Value.ToString() + "'", "", null, null, CommandOperationType.ExecuteScalar).ToString(), dgvPrint.Rows[e.RowIndex].Cells["Unit"].Value.ToString(), dgvPrint.Rows[e.RowIndex].Cells["ExpDate"].Value.ToString(), dgvPrint.Rows[e.RowIndex].Cells["ReceiveOrg"].Value.ToString(), dgvPrint.Rows[e.RowIndex].Cells["PreTraceCode"].Value.ToString(), dgvPrint.Rows[e.RowIndex].Cells["Producer"].Value.ToString(), dgvPrint.Rows[e.RowIndex].Cells["SourceProducer"].Value.ToString());
			}
		}
		else if (dgvPrint.Columns[e.ColumnIndex].HeaderText == "剩餘張數回沖" && MessageBox.Show("您確定要回沖可列印的標籤數量，回沖後無法再進行撤銷", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
		{
			dgvPrint.Enabled = false;
			MoveDataToHistory(dgvPrint.Rows[e.RowIndex].Cells["TraceCode"].Value.ToString());
			flushPrintedBackTo1O();
			MessageBox.Show("回沖完成");
			dgvPrint.Enabled = true;
			dgvPrint.DataSource = null;
			dgvPrint.Columns.Remove("列印");
			dgvPrint.Columns.Remove("撤銷");
			dgvPrint.Columns.Remove("立即回沖");
			InitdgvPrint();
			if (_SortingColIndex >= 0)
			{
				dgvPrint.Sort(dgvPrint.Columns[_SortingColIndex], _SortOrder);
				DisEnableBtns();
			}
		}
	}

	private void GoPrint(string CreateDate, string PackDate, string FarmerName, string ProductName, string TraceCode, string EanCode, string QRCodeUrl, string AvailableForPrinting, string AlreadyPrinted, string PrintCodeSrt, string AuthName, string Unit, string ExpDate, string ReceiveOrg, string PreTraceCode, string Producer, string SourceProducer)
	{
		frmPrint frmPrint = new frmPrint(CreateDate, PackDate, FarmerName, ProductName, TraceCode, EanCode, QRCodeUrl, AvailableForPrinting, AlreadyPrinted, PrintCodeSrt, AuthName, Unit, ExpDate, ReceiveOrg, PreTraceCode, Producer, SourceProducer);
		frmPrint.ShowDialog(this);
		int num = 1;
		frmPrint.Dispose();
		dgvPrint.DataSource = null;
		dgvPrint.Columns.Remove("列印");
		dgvPrint.Columns.Remove("撤銷");
		dgvPrint.Columns.Remove("立即回沖");
		InitdgvPrint();
		if (_SortingColIndex >= 0)
		{
			dgvPrint.Sort(dgvPrint.Columns[_SortingColIndex], _SortOrder);
			DisEnableBtns();
		}
	}

	private void GoDisposePrint(string CreateDate, string PackDate, string FarmerName, string ProductName, string TraceCode, string initPrintCodeSrt, string initPrintCodeEnd, string AlreadyPrinted, string ResumeID, bool showOnly)
	{
		frmDisposePrint frmDisposePrint = new frmDisposePrint(CreateDate, PackDate, FarmerName, ProductName, TraceCode, initPrintCodeSrt, initPrintCodeEnd, AlreadyPrinted, ResumeID, showOnly);
		frmDisposePrint.ShowDialog(this);
		int num = 1;
		frmDisposePrint.Dispose();
		dgvPrint.DataSource = null;
		dgvPrint.Columns.Remove("列印");
		dgvPrint.Columns.Remove("撤銷");
		dgvPrint.Columns.Remove("立即回沖");
		InitdgvPrint();
		if (_SortingColIndex >= 0)
		{
			dgvPrint.Sort(dgvPrint.Columns[_SortingColIndex], _SortOrder);
			DisEnableBtns();
		}
	}

	private bool IsANonHeaderLinkCell(DataGridViewCellEventArgs cellEvent)
	{
		if (dgvPrint.Columns[cellEvent.ColumnIndex] is DataGridViewLinkColumn && cellEvent.RowIndex != -1)
		{
			return true;
		}
		return false;
	}

	private bool IsANonHeaderButtonCell(DataGridViewCellEventArgs cellEvent)
	{
		if (dgvPrint.Columns[cellEvent.ColumnIndex] is DataGridViewButtonColumn && cellEvent.RowIndex != -1)
		{
			return true;
		}
		return false;
	}

	private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
	{
		dgvPrint.CurrentCell = dgvPrint.Rows[bs.Position].Cells[1];
		bindingNavigatorPositionItem.Text = Convert.ToString(bs.Position + 1);
	}

	private void bindingNavigatorPositionItem_Click(object sender, EventArgs e)
	{
	}

	private void bindingNavigatorMoveLastItem_Click(object sender, EventArgs e)
	{
		dgvPrint.CurrentCell = dgvPrint.Rows[bs.Position].Cells[1];
		bindingNavigatorPositionItem.Text = Convert.ToString(bs.Position + 1);
	}

	private void bindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
	{
		dgvPrint.CurrentCell = dgvPrint.Rows[bs.Position].Cells[1];
		bindingNavigatorPositionItem.Text = Convert.ToString(bs.Position + 1);
	}

	private void bindingNavigatorMoveFirstItem_Click(object sender, EventArgs e)
	{
		dgvPrint.CurrentCell = dgvPrint.Rows[bs.Position].Cells[1];
		bindingNavigatorPositionItem.Text = Convert.ToString(bs.Position + 1);
	}

	private void bindingNavigatorPositionItem_TextChanged(object sender, EventArgs e)
	{
		try
		{
			int num = Convert.ToInt32(bindingNavigatorPositionItem.Text) - 1;
			if (dgvPrint.Rows.Count > num && num > -1)
			{
				dgvPrint.CurrentCell = dgvPrint.Rows[num].Cells[1];
			}
		}
		catch
		{
		}
	}

	private void dgvPrint_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
		{
			bs.Position = e.RowIndex;
			bindingNavigatorPositionItem.Text = Convert.ToString(e.RowIndex + 1);
			dgvPrint.CurrentCell = dgvPrint.Rows[e.RowIndex].Cells[e.ColumnIndex];
		}
	}

	private void frmList_Load(object sender, EventArgs e)
	{
		ChangeShowHistoryDisplayText();
		SetTimeRangeCond();
		InitdgvPrint();
	}

	private void frmList_MaximumSizeChanged(object sender, EventArgs e)
	{
	}

	private void frmList_SizeChanged(object sender, EventArgs e)
	{
		DoResizeDGV();
	}

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		Program.ShowHistory = !Program.ShowHistory;
		ChangeShowHistoryDisplayText();
		SetTimeRangeCond();
		dgvPrint.DataSource = LoadDataFromDB();
		DisEnableBtns();
		bs.DataSource = dgvPrint.DataSource;
		bn.BindingSource = bs;
	}

	private void ChangeShowHistoryDisplayText()
	{
		btnShowHistory.Text = (Program.ShowHistory ? "不" : "") + "顯示逾期申請資訊";
		btnSetTimeRange.Visible = Program.ShowHistory;
		btnSetTimeRange.Enabled = Program.ShowHistory;
	}

	private void SetTimeRangeCond()
	{
		if (Program.ShowHistory)
		{
			_SqlTimeCond = string.Format("PackDate >=#{0}# AND PackDate <=#{1}#", _SrtTimeRange.ToString("yyyy/MM/dd"), _EndTimeRange.ToString("yyyy/MM/dd"));
			lblTimeRange.Text = string.Format("  目前顯示包裝日期: 從 {0} 至 {1}  ", _SrtTimeRange.ToString("yyyy/MM/dd"), _EndTimeRange.ToString("yyyy/MM/dd"));
		}
		else
		{
			_SqlTimeCond = string.Empty;
			lblTimeRange.Text = string.Empty;
		}
	}

	private void DoResizeDGV()
	{
		if (base.Size.Width > 31 && base.Size.Height > 76)
		{
			double num = (double)(base.Size.Width - 31) / (double)dgvPrint.Width;
			dgvPrint.Width = base.Size.Width - 31;
			dgvPrint.Height = base.Size.Height - 76;
			for (int i = 0; i < dgvPrint.Columns.Count; i++)
			{
				dgvPrint.Columns[i].Width = Convert.ToInt32((double)dgvPrint.Columns[i].Width * num);
			}
			dgvPrint.Refresh();
		}
	}

	private void dgvPrint_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
	{
		DisEnableBtns();
		_SortingColIndex = ((dgvPrint.SortedColumn == null) ? (-1) : dgvPrint.SortedColumn.Index);
		dgvPrint.SortOrder.ToString();
		switch (dgvPrint.SortOrder)
		{
		case SortOrder.Ascending:
			_SortOrder = ListSortDirection.Ascending;
			break;
		case SortOrder.Descending:
			_SortOrder = ListSortDirection.Descending;
			break;
		}
	}

	private void btnSetTimeRange_Click(object sender, EventArgs e)
	{
		frmSetTimeRange frmSetTimeRange = new frmSetTimeRange(_SrtTimeRange, _EndTimeRange);
		if (frmSetTimeRange.ShowDialog(this) == DialogResult.OK)
		{
			_SrtTimeRange = frmSetTimeRange.SrtTime;
			_EndTimeRange = frmSetTimeRange.EndTime;
			SetTimeRangeCond();
			dgvPrint.DataSource = LoadDataFromDB();
			DisEnableBtns();
			bs.DataSource = dgvPrint.DataSource;
			bn.BindingSource = bs;
		}
		frmSetTimeRange.Dispose();
	}

	private void MoveDataToHistory(string TraceCode)
	{
		StringBuilder stringBuilder = new StringBuilder("");
		stringBuilder.AppendFormat("SELECT ProductName, PackDate, CreateDate, FarmerName, AuthName, TraceCode, EanCOde, QRCodeUrl, initPrintCodeSrt, initPrintCodeEnd, initAvailablePrint, PrintCodeSrt, PrintCodeEnd, Unit, Sum(TotalPrinted) as Printed, PreTraceCode, ExpDate, ResumeID, ReceiveOrg, Producer,SourceProducer ");
		stringBuilder.AppendFormat("FROM   ResumeCurrent ");
		stringBuilder.AppendFormat("WHERE  TraceCode =  '{0}' ", TraceCode);
		stringBuilder.AppendFormat("Group by ProductName, PackDate, CreateDate, FarmerName, AuthName, TraceCode, EanCOde, QRCodeUrl, initPrintCodeSrt, initPrintCodeEnd, initAvailablePrint, PrintCodeSrt, PrintCodeEnd, Unit, PreTraceCode, ExpDate, ResumeID, ReceiveOrg, Producer,SourceProducer ");
		using (OleDbConnection oleDbConnection = new OleDbConnection(Program.ConnectionString))
		{
			using (OleDbCommand oleDbCommand = new OleDbCommand(stringBuilder.ToString(), oleDbConnection))
			{
				try
				{
					oleDbCommand.CommandTimeout = 0;
					oleDbConnection.Open();
					object obj = oleDbCommand.ExecuteReader();
					int num = 0;
					string text = "";
					string empty = string.Empty;
					if (obj != null)
					{
						DataTable dataTable = new DataTable();
						dataTable.Load((OleDbDataReader)obj);
						foreach (DataRow row in dataTable.Rows)
						{
							empty = (row["ExpDate"].Equals(DBNull.Value) ? "NULL" : ((DateTime.Compare(Convert.ToDateTime(row["ExpDate"]), Convert.ToDateTime("1900/01/02")) > 0) ? Convert.ToDateTime(row["ExpDate"]).ToString("yyyy/MM/dd") : "NULL"));
							string[,] strFieldArray = new string[19, 2]
							{
								{
									"ProductName",
									row["ProductName"].ToString()
								},
								{
									"PackDate",
									row["PackDate"].ToString()
								},
								{
									"CreateDate",
									row["CreateDate"].ToString()
								},
								{
									"FarmerName",
									row["FarmerName"].ToString()
								},
								{
									"AuthName",
									row["AuthName"].ToString()
								},
								{
									"TraceCode",
									row["TraceCode"].ToString()
								},
								{
									"EanCode",
									row["EanCode"].ToString()
								},
								{
									"QRCodeUrl",
									row["QRCodeUrl"].ToString()
								},
								{
									"Unit",
									row["Unit"].ToString()
								},
								{
									"Producer",
									row["Producer"].ToString()
								},
								{
									"ExpDate",
									empty
								},
								{
									"initPrintCodeSrt",
									row["initPrintCodeSrt"].ToString()
								},
								{
									"initPrintCodeEnd",
									row["initPrintCodeEnd"].ToString()
								},
								{
									"initAvailablePrint",
									row["initAvailablePrint"].ToString()
								},
								{
									"PreTraceCode",
									row["PreTraceCode"].ToString()
								},
								{
									"TotalPrinted",
									row["Printed"].ToString()
								},
								{
									"ReceiveOrg",
									row["ReceiveOrg"].ToString()
								},
								{
									"ResumeID",
									row["ResumeID"].ToString()
								},
								{
									"SourceProducer",
									row["SourceProducer"].ToString()
								}
							};
							DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Insert, "", "ResumeHistory", "", "", strFieldArray, null, CommandOperationType.ExecuteNonQuery);
							if (PrintCodeUtilties.GetNextNPrintCode(row["initPrintCodeEnd"].ToString(), 2) == row["PrintCodeSrt"].ToString())
							{
								num = 0;
								text = PrintCodeUtilties.GetNextNPrintCode(row["initPrintCodeEnd"].ToString(), 2);
							}
							else
							{
								num = (int)(Convert.ToInt64(row["initPrintCodeEnd"]) - Convert.ToInt64(row["PrintCodeSrt"])) + 1;
								text = row["PrintCodeSrt"].ToString();
							}
							strFieldArray = new string[4, 2]
							{
								{
									"PrintCodeSrt",
									text
								},
								{
									"PrintCodeEnd",
									row["initPrintCodeEnd"].ToString()
								},
								{
									"PintCodeCount",
									num.ToString()
								},
								{
									"DisposeReason",
									"NULL"
								}
							};
							DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Insert, "", "FlushPrintCode", "", "", strFieldArray, null, CommandOperationType.ExecuteNonQuery);
							string[] strWhereParameterArray = new string[1]
							{
								row["ResumeID"].ToString()
							};
							foreach (DataRow row2 in ((DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Select, "PrintCodeSrt, PrintCodeEnd, PintCodeCount, DisposeReason", "ResumeDispose", "ResumeID = {0}", "", null, strWhereParameterArray, CommandOperationType.ExecuteReaderReturnDataTable)).Rows)
							{
								strFieldArray = new string[4, 2]
								{
									{
										"PrintCodeSrt",
										row2["PrintCodeSrt"].ToString()
									},
									{
										"PrintCodeEnd",
										row2["PrintCodeEnd"].ToString()
									},
									{
										"PintCodeCount",
										row2["PintCodeCount"].ToString()
									},
									{
										"DisposeReason",
										row2["DisposeReason"].ToString()
									}
								};
								DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Insert, "", "FlushPrintCode", "", "", strFieldArray, null, CommandOperationType.ExecuteNonQuery);
							}
						}
					}
					string[] strWhereParameterArray2 = new string[1]
					{
						TraceCode
					};
					DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Delete, "", "ResumeCurrent", "TraceCode =  {0}", "", null, strWhereParameterArray2, CommandOperationType.ExecuteNonQuery);
				}
				catch (Exception)
				{
				}
			}
		}
	}

	private void flushPrintedBackTo1O()
	{
		Service service = new Service();
		service.Url = Program.OLPUrl;
		service.Timeout = 800000;
		string wSTime = service.GetWSTime();
		L1_1O l1_1O = new L1_1O();
		l1_1O.Url = Program.L1Url;
		l1_1O.Timeout = 800000;
		DataTable dataTable = (DataTable)DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Select, "PrintCodeSrt, PrintCodeEnd, PintCodeCount, DisposeReason", "FlushPrintCode", "", "PrintCodeSrt ASC", null, null, CommandOperationType.ExecuteReaderReturnDataTable);
		if (dataTable != null && dataTable.Rows.Count != 0)
		{
			try
			{
				for (int i = 0; i < dataTable.Rows.Count; i++)
				{
					Application.DoEvents();
					l1_1O.UploadData(GenFlushString(dataTable.Rows[i][0].ToString(), dataTable.Rows[i][1].ToString(), dataTable.Rows[i][2].ToString(), dataTable.Rows[i][3].Equals(DBNull.Value) ? string.Empty : dataTable.Rows[i][3].ToString(), wSTime));
				}
				DataBaseUtilities.DBOperation(Program.ConnectionString, TableOperation.Delete, "", "FlushPrintCode", "", "", null, null, CommandOperationType.ExecuteNonQuery);
			}
			catch
			{
			}
		}
	}

	private string GenFlushString(string PrintLabNumS, string PrintLabNumE, string PrintLabAmt, string DisposeReason, string WSdate)
	{
		StringBuilder stringBuilder = new StringBuilder("");
		stringBuilder.AppendFormat("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
		stringBuilder.AppendFormat("<PrintLabUseUpload>");
		stringBuilder.AppendFormat("<OrgID>{0}</OrgID>", Program.OrgID);
		stringBuilder.AppendFormat("<TrustCode><![CDATA[{0}]]></TrustCode>", Encrypt.EncryptAES(Program.OrgID + "," + WSdate, "offLINETrustCODE"));
		stringBuilder.AppendFormat("<SysCode>{0}</SysCode>", Program.SysSerialNo);
		stringBuilder.AppendFormat("   <PrintUpload>");
		stringBuilder.AppendFormat("       <PrintLabNumS>{0}</PrintLabNumS>", PrintLabNumS);
		stringBuilder.AppendFormat("       <PrintLabNumE>{0}</PrintLabNumE>", PrintLabNumE);
		stringBuilder.AppendFormat("       <PrintLabAmt>{0}</PrintLabAmt>", PrintLabAmt);
		if (DisposeReason != string.Empty)
		{
			stringBuilder.AppendFormat("       <MemoDescp>{0}</MemoDescp>", DisposeReason);
		}
		else
		{
			stringBuilder.AppendFormat("       <NeverPrint>{0}</NeverPrint>", "1");
		}
		stringBuilder.AppendFormat("   </PrintUpload>");
		stringBuilder.AppendFormat("</PrintLabUseUpload>");
		return stringBuilder.ToString();
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
		components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(離線列印Client程式.frmList));
		dgvPrint = new System.Windows.Forms.DataGridView();
		bn = new System.Windows.Forms.BindingNavigator(components);
		bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
		bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
		bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
		bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
		bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
		bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
		bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
		bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		btnShowHistory = new System.Windows.Forms.ToolStripButton();
		lblTimeRange = new System.Windows.Forms.ToolStripLabel();
		btnSetTimeRange = new System.Windows.Forms.ToolStripButton();
		bs = new System.Windows.Forms.BindingSource(components);
		((System.ComponentModel.ISupportInitialize)dgvPrint).BeginInit();
		((System.ComponentModel.ISupportInitialize)bn).BeginInit();
		bn.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)bs).BeginInit();
		SuspendLayout();
		dgvPrint.AllowUserToAddRows = false;
		dgvPrint.AllowUserToDeleteRows = false;
		dgvPrint.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		dgvPrint.Location = new System.Drawing.Point(12, 28);
		dgvPrint.Margin = new System.Windows.Forms.Padding(0);
		dgvPrint.Name = "dgvPrint";
		dgvPrint.ReadOnly = true;
		dgvPrint.RowTemplate.Height = 24;
		dgvPrint.Size = new System.Drawing.Size(988, 309);
		dgvPrint.TabIndex = 0;
		dgvPrint.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(dgvPrint_ColumnHeaderMouseClick);
		dgvPrint.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvPrint_CellClick);
		dgvPrint.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvPrint_CellContentClick);
		bn.AddNewItem = null;
		bn.CountItem = bindingNavigatorCountItem;
		bn.DeleteItem = null;
		bn.Items.AddRange(new System.Windows.Forms.ToolStripItem[12]
		{
			bindingNavigatorMoveFirstItem,
			bindingNavigatorMovePreviousItem,
			bindingNavigatorSeparator,
			bindingNavigatorPositionItem,
			bindingNavigatorCountItem,
			bindingNavigatorSeparator1,
			bindingNavigatorMoveNextItem,
			bindingNavigatorMoveLastItem,
			bindingNavigatorSeparator2,
			btnShowHistory,
			lblTimeRange,
			btnSetTimeRange
		});
		bn.Location = new System.Drawing.Point(0, 0);
		bn.MoveFirstItem = bindingNavigatorMoveFirstItem;
		bn.MoveLastItem = bindingNavigatorMoveLastItem;
		bn.MoveNextItem = bindingNavigatorMoveNextItem;
		bn.MovePreviousItem = bindingNavigatorMovePreviousItem;
		bn.Name = "bn";
		bn.PositionItem = null;
		bn.Size = new System.Drawing.Size(1009, 25);
		bn.TabIndex = 1;
		bn.Text = "bindingNavigator1";
		bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
		bindingNavigatorCountItem.Size = new System.Drawing.Size(24, 22);
		bindingNavigatorCountItem.Text = "/{0}";
		bindingNavigatorCountItem.ToolTipText = "項目總數";
		bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		bindingNavigatorMoveFirstItem.Image = (System.Drawing.Image)resources.GetObject("bindingNavigatorMoveFirstItem.Image");
		bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
		bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
		bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(23, 22);
		bindingNavigatorMoveFirstItem.Text = "移到最前面";
		bindingNavigatorMoveFirstItem.Click += new System.EventHandler(bindingNavigatorMoveFirstItem_Click);
		bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		bindingNavigatorMovePreviousItem.Image = (System.Drawing.Image)resources.GetObject("bindingNavigatorMovePreviousItem.Image");
		bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
		bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
		bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(23, 22);
		bindingNavigatorMovePreviousItem.Text = "移到上一個";
		bindingNavigatorMovePreviousItem.Click += new System.EventHandler(bindingNavigatorMovePreviousItem_Click);
		bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
		bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 25);
		bindingNavigatorPositionItem.AccessibleName = "位置";
		bindingNavigatorPositionItem.AutoSize = false;
		bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
		bindingNavigatorPositionItem.Size = new System.Drawing.Size(50, 22);
		bindingNavigatorPositionItem.Text = "0";
		bindingNavigatorPositionItem.ToolTipText = "目前的位置";
		bindingNavigatorPositionItem.TextChanged += new System.EventHandler(bindingNavigatorPositionItem_TextChanged);
		bindingNavigatorPositionItem.Click += new System.EventHandler(bindingNavigatorPositionItem_Click);
		bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
		bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 25);
		bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		bindingNavigatorMoveNextItem.Image = (System.Drawing.Image)resources.GetObject("bindingNavigatorMoveNextItem.Image");
		bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
		bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
		bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(23, 22);
		bindingNavigatorMoveNextItem.Text = "移到下一個";
		bindingNavigatorMoveNextItem.Click += new System.EventHandler(bindingNavigatorMoveNextItem_Click);
		bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		bindingNavigatorMoveLastItem.Image = (System.Drawing.Image)resources.GetObject("bindingNavigatorMoveLastItem.Image");
		bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
		bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
		bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 22);
		bindingNavigatorMoveLastItem.Text = "移到最後面";
		bindingNavigatorMoveLastItem.Click += new System.EventHandler(bindingNavigatorMoveLastItem_Click);
		bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
		bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 25);
		btnShowHistory.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		btnShowHistory.Image = (System.Drawing.Image)resources.GetObject("btnShowHistory.Image");
		btnShowHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		btnShowHistory.Name = "btnShowHistory";
		btnShowHistory.Size = new System.Drawing.Size(105, 22);
		btnShowHistory.Text = "顯示逾期申請資訊";
		btnShowHistory.Click += new System.EventHandler(toolStripButton1_Click);
		lblTimeRange.Name = "lblTimeRange";
		lblTimeRange.Size = new System.Drawing.Size(101, 22);
		lblTimeRange.Text = "程式置換時間範圍";
		btnSetTimeRange.BackColor = System.Drawing.SystemColors.Control;
		btnSetTimeRange.Image = (System.Drawing.Image)resources.GetObject("btnSetTimeRange.Image");
		btnSetTimeRange.ImageTransparentColor = System.Drawing.Color.Magenta;
		btnSetTimeRange.Name = "btnSetTimeRange";
		btnSetTimeRange.Size = new System.Drawing.Size(97, 22);
		btnSetTimeRange.Text = "設定時間範圍";
		btnSetTimeRange.Click += new System.EventHandler(btnSetTimeRange_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1009, 349);
		base.Controls.Add(bn);
		base.Controls.Add(dgvPrint);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "frmList";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		Text = "標籤清單";
		base.Load += new System.EventHandler(frmList_Load);
		base.SizeChanged += new System.EventHandler(frmList_SizeChanged);
		base.MaximumSizeChanged += new System.EventHandler(frmList_MaximumSizeChanged);
		((System.ComponentModel.ISupportInitialize)dgvPrint).EndInit();
		((System.ComponentModel.ISupportInitialize)bn).EndInit();
		bn.ResumeLayout(false);
		bn.PerformLayout();
		((System.ComponentModel.ISupportInitialize)bs).EndInit();
		ResumeLayout(false);
		PerformLayout();
	}
}
