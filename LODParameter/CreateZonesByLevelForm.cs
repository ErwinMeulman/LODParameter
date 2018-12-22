using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace LODParameter
{
	public class CreateZonesByLevelForm : Form
	{
		protected const string LOWER_BOUND_NAME = "Bottom of Model";

		protected const string UPPER_BOUND_NAME = "Top of Model";

		protected const string SOUTH_BOUND_NAME = "Origin";

		protected const string NORTH_BOUND_NAME = "North Model Edge";

		protected const string WEST_BOUND_NAME = "Origin";

		protected const string EAST_BOUND_NAME = "East Model Edge";

		protected internal const double UPPER_BOUND_OFFSET = 20.0;

		protected internal const double LOWER_BOUND_OFFSET = -20.0;

		protected internal const double GRID_MIN_OFFSET = 0.0;

		protected internal const double GRID_MAX_OFFSET = 20.0;

		private SortedList<double, Level> m_Levels;

		private SortedList<double, Grid> m_GridsNorthSouth;

		private SortedList<double, Grid> m_GridsEastWest;

		private FormattedInputBox inputBoxLevelOffset;

		private IContainer components = null;

		private Label label1;

		private Label label2;

		private CheckedListBox checkedListBoxLevels;

		private TableLayoutPanel tableLayoutPanel1;

		private CheckedListBox checkedListBoxGridsEastWest;

		private CheckedListBox checkedListBoxGridsNorthSouth;

		private Button buttonCancel;

		private Button buttonOK;

		private Label label3;

		private TextBox textBoxLevelOffset;

		public SortedList<double, Level> SelectedLevels
		{
			get
			{
				SortedList<double, Level> sortedList = new SortedList<double, Level>(checkedListBoxLevels.CheckedIndices.Count + 1);
				foreach (int checkedIndex in checkedListBoxLevels.CheckedIndices)
				{
					if (checkedIndex != 0 && checkedIndex != m_Levels.Count + 1)
					{
						sortedList.Add(m_Levels.Keys[checkedIndex - 1], m_Levels.Values[checkedIndex - 1]);
					}
				}
				return sortedList;
			}
		}

		public SortedList<double, Grid> SelectedGridsNorthSouth
		{
			get
			{
				SortedList<double, Grid> sortedList = new SortedList<double, Grid>(checkedListBoxGridsNorthSouth.CheckedIndices.Count);
				foreach (int checkedIndex in checkedListBoxGridsNorthSouth.CheckedIndices)
				{
					if (checkedIndex != 0 && checkedIndex != m_GridsNorthSouth.Count + 1)
					{
						sortedList.Add(m_GridsNorthSouth.Keys[checkedIndex - 1], m_GridsNorthSouth.Values[checkedIndex - 1]);
					}
				}
				return sortedList;
			}
		}

		public SortedList<double, Grid> SelectedGridsEastWest
		{
			get
			{
				SortedList<double, Grid> sortedList = new SortedList<double, Grid>(checkedListBoxGridsEastWest.CheckedIndices.Count);
				foreach (int checkedIndex in checkedListBoxGridsEastWest.CheckedIndices)
				{
					if (checkedIndex != 0 && checkedIndex != m_GridsEastWest.Count + 1)
					{
						sortedList.Add(m_GridsEastWest.Keys[checkedIndex - 1], m_GridsEastWest.Values[checkedIndex - 1]);
					}
				}
				return sortedList;
			}
		}

		public double OverallLevelOffset => inputBoxLevelOffset.Value;

		public bool HasLowerBound => checkedListBoxLevels.CheckedItems.Contains("Bottom of Model");

		public bool HasUpperBound => checkedListBoxLevels.CheckedItems.Contains("Top of Model");

		public bool HasNorthBound => checkedListBoxGridsEastWest.CheckedItems.Contains("North Model Edge");

		public bool HasSouthBound => checkedListBoxGridsEastWest.CheckedItems.Contains("Origin");

		public bool HasEastBound => checkedListBoxGridsNorthSouth.CheckedItems.Contains("East Model Edge");

		public bool HasWestBound => checkedListBoxGridsNorthSouth.CheckedItems.Contains("Origin");

		public CreateZonesByLevelForm(SortedList<double, Level> levels, SortedList<double, Grid> gridsNorthSouth, SortedList<double, Grid> gridsEastWest, Units docUnits)
		{
			InitializeComponent();
			m_Levels = levels;
			m_GridsNorthSouth = gridsNorthSouth;
			m_GridsEastWest = gridsEastWest;
			checkedListBoxLevels.Items.Add("Bottom of Model", true);
			foreach (Level value in levels.Values)
			{
				checkedListBoxLevels.Items.Add(value.get_Name(), true);
			}
			checkedListBoxLevels.Items.Add("Top of Model", true);
			checkedListBoxGridsEastWest.Items.Add("Origin", true);
			foreach (Grid value2 in gridsEastWest.Values)
			{
				checkedListBoxGridsEastWest.Items.Add(value2.get_Name(), false);
			}
			checkedListBoxGridsEastWest.Items.Add("North Model Edge", true);
			checkedListBoxGridsNorthSouth.Items.Add("Origin", true);
			foreach (Grid value3 in gridsNorthSouth.Values)
			{
				checkedListBoxGridsNorthSouth.Items.Add(value3.get_Name(), false);
			}
			checkedListBoxGridsNorthSouth.Items.Add("East Model Edge", true);
			inputBoxLevelOffset = new FormattedInputBox(textBoxLevelOffset, docUnits, 0);
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.OK;
			Hide();
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			Hide();
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
			label1 = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			checkedListBoxLevels = new System.Windows.Forms.CheckedListBox();
			tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			checkedListBoxGridsEastWest = new System.Windows.Forms.CheckedListBox();
			checkedListBoxGridsNorthSouth = new System.Windows.Forms.CheckedListBox();
			textBoxLevelOffset = new System.Windows.Forms.TextBox();
			label3 = new System.Windows.Forms.Label();
			buttonCancel = new System.Windows.Forms.Button();
			buttonOK = new System.Windows.Forms.Button();
			tableLayoutPanel1.SuspendLayout();
			SuspendLayout();
			label1.AutoSize = true;
			tableLayoutPanel1.SetColumnSpan(label1, 2);
			label1.Location = new System.Drawing.Point(3, 200);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(69, 13);
			label1.TabIndex = 2;
			label1.Text = "Split at Grids:";
			label2.AutoSize = true;
			tableLayoutPanel1.SetColumnSpan(label2, 2);
			label2.Location = new System.Drawing.Point(3, 0);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(76, 13);
			label2.TabIndex = 0;
			label2.Text = "Split at Levels:";
			checkedListBoxLevels.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			checkedListBoxLevels.CheckOnClick = true;
			checkedListBoxLevels.FormattingEnabled = true;
			checkedListBoxLevels.Location = new System.Drawing.Point(3, 23);
			checkedListBoxLevels.Name = "checkedListBoxLevels";
			tableLayoutPanel1.SetRowSpan(checkedListBoxLevels, 2);
			checkedListBoxLevels.Size = new System.Drawing.Size(290, 169);
			checkedListBoxLevels.TabIndex = 1;
			tableLayoutPanel1.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			tableLayoutPanel1.ColumnCount = 2;
			tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
			tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
			tableLayoutPanel1.Controls.Add(checkedListBoxGridsEastWest, 0, 4);
			tableLayoutPanel1.Controls.Add(label2, 0, 0);
			tableLayoutPanel1.Controls.Add(label1, 0, 3);
			tableLayoutPanel1.Controls.Add(checkedListBoxGridsNorthSouth, 1, 4);
			tableLayoutPanel1.Controls.Add(textBoxLevelOffset, 1, 2);
			tableLayoutPanel1.Controls.Add(checkedListBoxLevels, 0, 1);
			tableLayoutPanel1.Controls.Add(label3, 1, 1);
			tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
			tableLayoutPanel1.Name = "tableLayoutPanel1";
			tableLayoutPanel1.RowCount = 5;
			tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20f));
			tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25f));
			tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25f));
			tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20f));
			tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50f));
			tableLayoutPanel1.Size = new System.Drawing.Size(593, 401);
			tableLayoutPanel1.TabIndex = 0;
			checkedListBoxGridsEastWest.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			checkedListBoxGridsEastWest.CheckOnClick = true;
			checkedListBoxGridsEastWest.FormattingEnabled = true;
			checkedListBoxGridsEastWest.Location = new System.Drawing.Point(3, 223);
			checkedListBoxGridsEastWest.Name = "checkedListBoxGridsEastWest";
			checkedListBoxGridsEastWest.Size = new System.Drawing.Size(290, 169);
			checkedListBoxGridsEastWest.TabIndex = 3;
			checkedListBoxGridsNorthSouth.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			checkedListBoxGridsNorthSouth.CheckOnClick = true;
			checkedListBoxGridsNorthSouth.FormattingEnabled = true;
			checkedListBoxGridsNorthSouth.Location = new System.Drawing.Point(299, 223);
			checkedListBoxGridsNorthSouth.Name = "checkedListBoxGridsNorthSouth";
			checkedListBoxGridsNorthSouth.Size = new System.Drawing.Size(291, 169);
			checkedListBoxGridsNorthSouth.TabIndex = 4;
			textBoxLevelOffset.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			textBoxLevelOffset.Location = new System.Drawing.Point(306, 113);
			textBoxLevelOffset.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
			textBoxLevelOffset.Name = "textBoxLevelOffset";
			textBoxLevelOffset.Size = new System.Drawing.Size(277, 20);
			textBoxLevelOffset.TabIndex = 3;
			textBoxLevelOffset.Text = "0' - 0\"";
			label3.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			label3.AutoSize = true;
			label3.Location = new System.Drawing.Point(306, 97);
			label3.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(95, 13);
			label3.TabIndex = 0;
			label3.Text = "Offset from Levels:";
			buttonCancel.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			buttonCancel.Location = new System.Drawing.Point(530, 419);
			buttonCancel.Name = "buttonCancel";
			buttonCancel.Size = new System.Drawing.Size(75, 23);
			buttonCancel.TabIndex = 2;
			buttonCancel.Text = "Cancel";
			buttonCancel.UseVisualStyleBackColor = true;
			buttonCancel.Click += new System.EventHandler(buttonCancel_Click);
			buttonOK.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			buttonOK.Location = new System.Drawing.Point(449, 419);
			buttonOK.Name = "buttonOK";
			buttonOK.Size = new System.Drawing.Size(75, 23);
			buttonOK.TabIndex = 1;
			buttonOK.Text = "OK";
			buttonOK.UseVisualStyleBackColor = true;
			buttonOK.Click += new System.EventHandler(buttonOK_Click);
			base.AcceptButton = buttonOK;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = buttonCancel;
			base.ClientSize = new System.Drawing.Size(617, 454);
			base.Controls.Add(tableLayoutPanel1);
			base.Controls.Add(buttonCancel);
			base.Controls.Add(buttonOK);
			base.KeyPreview = true;
			MinimumSize = new System.Drawing.Size(450, 400);
			base.Name = "CreateZonesByLevelForm";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = "Create Zones by Level";
			tableLayoutPanel1.ResumeLayout(false);
			tableLayoutPanel1.PerformLayout();
			ResumeLayout(false);
		}
	}
}
