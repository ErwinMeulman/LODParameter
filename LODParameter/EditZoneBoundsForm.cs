using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LODParameter
{
	public class EditZoneBoundsForm : Form
	{
		private FormattedInputBox inputBoxTopOffset;

		private FormattedInputBox inputBoxBottomOffset;

		private FormattedInputBox inputBoxNorthOffset;

		private FormattedInputBox inputBoxSouthOffset;

		private FormattedInputBox inputBoxEastOffset;

		private FormattedInputBox inputBoxWestOffset;

		private SortedList<double, Level> m_Levels;

		private SortedList<double, Grid> m_GridsNorthSouth;

		private SortedList<double, Grid> m_GridsEastWest;

		private IContainer components = null;

		private Button buttonOK;

		private Button buttonCancel;

		private GroupBox groupBox1;

		private GroupBox groupBox2;

		private Label label1;

		private ComboBox comboBoxTopLevel;

		private TextBox textBoxTopOffset;

		private Label label4;

		private TextBox textBoxBottomOffset;

		private Label label2;

		private ComboBox comboBoxBottomLevel;

		private Label label3;

		private TextBox textBoxWestOffset;

		private Label label12;

		private TextBox textBoxEastOffset;

		private Label label10;

		private TextBox textBoxSouthOffset;

		private Label label11;

		private Label label8;

		private Label label9;

		private TextBox textBoxNorthOffset;

		private ComboBox comboBoxWestGrid;

		private Label label7;

		private ComboBox comboBoxEastGrid;

		private Label label6;

		private ComboBox comboBoxSouthGrid;

		private Label label5;

		private ComboBox comboBoxNorthGrid;

		private TextBox textBoxName;

		private Label label13;

		public string ZoneName => textBoxName.Text;

		public Level SelectedTopLevel => m_Levels.Values[comboBoxTopLevel.SelectedIndex];

		public Level SelectedBaseLevel => m_Levels.Values[comboBoxBottomLevel.SelectedIndex];

		public Grid SelectedNorthGrid => ((string)comboBoxNorthGrid.SelectedItem == "Origin") ? null : m_GridsEastWest.Values[comboBoxNorthGrid.SelectedIndex];

		public Grid SelectedSouthGrid => ((string)comboBoxSouthGrid.SelectedItem == "Origin") ? null : m_GridsEastWest.Values[comboBoxSouthGrid.SelectedIndex];

		public Grid SelectedEastGrid => ((string)comboBoxEastGrid.SelectedItem == "Origin") ? null : m_GridsNorthSouth.Values[comboBoxEastGrid.SelectedIndex];

		public Grid SelectedWestGrid => ((string)comboBoxWestGrid.SelectedItem == "Origin") ? null : m_GridsNorthSouth.Values[comboBoxWestGrid.SelectedIndex];

		public double TopOffset => inputBoxTopOffset.Value;

		public double BaseOffset => inputBoxBottomOffset.Value;

		public double NorthOffset => inputBoxNorthOffset.Value;

		public double SouthOffset => inputBoxSouthOffset.Value;

		public double EastOffset => inputBoxEastOffset.Value;

		public double WestOffset => inputBoxWestOffset.Value;

		public EditZoneBoundsForm(ZoneData zone, SortedList<double, Level> levels, SortedList<double, Grid> gridsNorthSouth, SortedList<double, Grid> gridsEastWest, Units docUnits)
		{
			InitializeComponent();
			if (!string.IsNullOrWhiteSpace(zone.Name))
			{
				textBoxName.Text = zone.Name;
			}
			else
			{
				textBoxName.Text = "Zone";
			}
			m_Levels = levels;
			m_GridsNorthSouth = gridsNorthSouth;
			m_GridsEastWest = gridsEastWest;
			string[] array = (from l in m_Levels.Values
			select l.get_Name()).ToArray();
			ComboBox[] array2 = new ComboBox[2]
			{
				comboBoxTopLevel,
				comboBoxBottomLevel
			};
			ComboBox[] array3 = array2;
			object[] items2;
			foreach (ComboBox comboBox in array3)
			{
				ComboBox.ObjectCollection items = comboBox.Items;
				items2 = array;
				items.AddRange(items2);
			}
			ComboBox comboBox2 = comboBoxTopLevel;
			object topLevel = (object)zone.TopLevel;
			comboBox2.SelectedItem = (((topLevel != null) ? topLevel.get_Name() : null) ?? array.Last());
			ComboBox comboBox3 = comboBoxBottomLevel;
			object baseLevel = (object)zone.BaseLevel;
			comboBox3.SelectedItem = (((baseLevel != null) ? baseLevel.get_Name() : null) ?? array.First());
			string[] array4 = (from g in m_GridsNorthSouth.Values
			select g.get_Name()).ToArray();
			string[] array5 = (from g in m_GridsEastWest.Values
			select g.get_Name()).ToArray();
			ComboBox.ObjectCollection items3 = comboBoxNorthGrid.Items;
			items2 = array5;
			items3.AddRange(items2);
			ComboBox.ObjectCollection items4 = comboBoxSouthGrid.Items;
			items2 = array5;
			items4.AddRange(items2);
			ComboBox.ObjectCollection items5 = comboBoxEastGrid.Items;
			items2 = array4;
			items5.AddRange(items2);
			ComboBox.ObjectCollection items6 = comboBoxWestGrid.Items;
			items2 = array4;
			items6.AddRange(items2);
			comboBoxNorthGrid.Items.Add("Origin");
			comboBoxSouthGrid.Items.Add("Origin");
			comboBoxEastGrid.Items.Add("Origin");
			comboBoxWestGrid.Items.Add("Origin");
			ComboBox comboBox4 = comboBoxNorthGrid;
			object northGrid = (object)zone.NorthGrid;
			comboBox4.SelectedItem = (((northGrid != null) ? northGrid.get_Name() : null) ?? "Origin");
			ComboBox comboBox5 = comboBoxSouthGrid;
			object southGrid = (object)zone.SouthGrid;
			comboBox5.SelectedItem = (((southGrid != null) ? southGrid.get_Name() : null) ?? "Origin");
			ComboBox comboBox6 = comboBoxEastGrid;
			object eastGrid = (object)zone.EastGrid;
			comboBox6.SelectedItem = (((eastGrid != null) ? eastGrid.get_Name() : null) ?? "Origin");
			ComboBox comboBox7 = comboBoxWestGrid;
			object westGrid = (object)zone.WestGrid;
			comboBox7.SelectedItem = (((westGrid != null) ? westGrid.get_Name() : null) ?? "Origin");
			inputBoxTopOffset = new FormattedInputBox(textBoxTopOffset, docUnits, 0);
			inputBoxBottomOffset = new FormattedInputBox(textBoxBottomOffset, docUnits, 0);
			inputBoxNorthOffset = new FormattedInputBox(textBoxNorthOffset, docUnits, 0);
			inputBoxSouthOffset = new FormattedInputBox(textBoxSouthOffset, docUnits, 0);
			inputBoxEastOffset = new FormattedInputBox(textBoxEastOffset, docUnits, 0);
			inputBoxWestOffset = new FormattedInputBox(textBoxWestOffset, docUnits, 0);
			inputBoxTopOffset.Value = zone.TopOffset;
			inputBoxBottomOffset.Value = zone.BaseOffset;
			inputBoxNorthOffset.Value = zone.NorthOffset;
			inputBoxSouthOffset.Value = zone.SouthOffset;
			inputBoxEastOffset.Value = zone.EastOffset;
			inputBoxWestOffset.Value = zone.WestOffset;
			textBoxName.SelectAll();
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
			buttonOK = new System.Windows.Forms.Button();
			buttonCancel = new System.Windows.Forms.Button();
			groupBox1 = new System.Windows.Forms.GroupBox();
			textBoxBottomOffset = new System.Windows.Forms.TextBox();
			label2 = new System.Windows.Forms.Label();
			comboBoxBottomLevel = new System.Windows.Forms.ComboBox();
			label3 = new System.Windows.Forms.Label();
			textBoxTopOffset = new System.Windows.Forms.TextBox();
			label4 = new System.Windows.Forms.Label();
			comboBoxTopLevel = new System.Windows.Forms.ComboBox();
			label1 = new System.Windows.Forms.Label();
			groupBox2 = new System.Windows.Forms.GroupBox();
			textBoxWestOffset = new System.Windows.Forms.TextBox();
			label12 = new System.Windows.Forms.Label();
			textBoxEastOffset = new System.Windows.Forms.TextBox();
			label10 = new System.Windows.Forms.Label();
			textBoxSouthOffset = new System.Windows.Forms.TextBox();
			label11 = new System.Windows.Forms.Label();
			label8 = new System.Windows.Forms.Label();
			label9 = new System.Windows.Forms.Label();
			textBoxNorthOffset = new System.Windows.Forms.TextBox();
			comboBoxWestGrid = new System.Windows.Forms.ComboBox();
			label7 = new System.Windows.Forms.Label();
			comboBoxEastGrid = new System.Windows.Forms.ComboBox();
			label6 = new System.Windows.Forms.Label();
			comboBoxSouthGrid = new System.Windows.Forms.ComboBox();
			label5 = new System.Windows.Forms.Label();
			comboBoxNorthGrid = new System.Windows.Forms.ComboBox();
			textBoxName = new System.Windows.Forms.TextBox();
			label13 = new System.Windows.Forms.Label();
			groupBox1.SuspendLayout();
			groupBox2.SuspendLayout();
			SuspendLayout();
			buttonOK.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			buttonOK.Location = new System.Drawing.Point(309, 337);
			buttonOK.Name = "buttonOK";
			buttonOK.Size = new System.Drawing.Size(75, 23);
			buttonOK.TabIndex = 4;
			buttonOK.Text = "OK";
			buttonOK.UseVisualStyleBackColor = true;
			buttonOK.Click += new System.EventHandler(buttonOK_Click);
			buttonCancel.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			buttonCancel.Location = new System.Drawing.Point(390, 337);
			buttonCancel.Name = "buttonCancel";
			buttonCancel.Size = new System.Drawing.Size(75, 23);
			buttonCancel.TabIndex = 5;
			buttonCancel.Text = "Cancel";
			buttonCancel.UseVisualStyleBackColor = true;
			buttonCancel.Click += new System.EventHandler(buttonCancel_Click);
			groupBox1.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			groupBox1.Controls.Add(textBoxBottomOffset);
			groupBox1.Controls.Add(label2);
			groupBox1.Controls.Add(comboBoxBottomLevel);
			groupBox1.Controls.Add(label3);
			groupBox1.Controls.Add(textBoxTopOffset);
			groupBox1.Controls.Add(label4);
			groupBox1.Controls.Add(comboBoxTopLevel);
			groupBox1.Controls.Add(label1);
			groupBox1.Location = new System.Drawing.Point(12, 38);
			groupBox1.Name = "groupBox1";
			groupBox1.Size = new System.Drawing.Size(453, 102);
			groupBox1.TabIndex = 2;
			groupBox1.TabStop = false;
			groupBox1.Text = "Vertical Bounds";
			textBoxBottomOffset.Location = new System.Drawing.Point(332, 62);
			textBoxBottomOffset.Name = "textBoxBottomOffset";
			textBoxBottomOffset.Size = new System.Drawing.Size(100, 20);
			textBoxBottomOffset.TabIndex = 7;
			textBoxBottomOffset.Text = "0' - 0\"";
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(288, 65);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(38, 13);
			label2.TabIndex = 6;
			label2.Text = "Offset:";
			comboBoxBottomLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			comboBoxBottomLevel.FormattingEnabled = true;
			comboBoxBottomLevel.Location = new System.Drawing.Point(82, 62);
			comboBoxBottomLevel.MaxDropDownItems = 10;
			comboBoxBottomLevel.Name = "comboBoxBottomLevel";
			comboBoxBottomLevel.Size = new System.Drawing.Size(182, 21);
			comboBoxBottomLevel.TabIndex = 5;
			label3.AutoSize = true;
			label3.Location = new System.Drawing.Point(6, 65);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(43, 13);
			label3.TabIndex = 4;
			label3.Text = "Bottom:";
			textBoxTopOffset.Location = new System.Drawing.Point(332, 23);
			textBoxTopOffset.Name = "textBoxTopOffset";
			textBoxTopOffset.Size = new System.Drawing.Size(100, 20);
			textBoxTopOffset.TabIndex = 3;
			textBoxTopOffset.Text = "0' - 0\"";
			label4.AutoSize = true;
			label4.Location = new System.Drawing.Point(288, 26);
			label4.Name = "label4";
			label4.Size = new System.Drawing.Size(38, 13);
			label4.TabIndex = 2;
			label4.Text = "Offset:";
			comboBoxTopLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			comboBoxTopLevel.FormattingEnabled = true;
			comboBoxTopLevel.Location = new System.Drawing.Point(82, 23);
			comboBoxTopLevel.MaxDropDownItems = 10;
			comboBoxTopLevel.Name = "comboBoxTopLevel";
			comboBoxTopLevel.Size = new System.Drawing.Size(182, 21);
			comboBoxTopLevel.TabIndex = 1;
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(6, 26);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(29, 13);
			label1.TabIndex = 0;
			label1.Text = "Top:";
			groupBox2.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			groupBox2.Controls.Add(textBoxWestOffset);
			groupBox2.Controls.Add(label12);
			groupBox2.Controls.Add(textBoxEastOffset);
			groupBox2.Controls.Add(label10);
			groupBox2.Controls.Add(textBoxSouthOffset);
			groupBox2.Controls.Add(label11);
			groupBox2.Controls.Add(label8);
			groupBox2.Controls.Add(label9);
			groupBox2.Controls.Add(textBoxNorthOffset);
			groupBox2.Controls.Add(comboBoxWestGrid);
			groupBox2.Controls.Add(label7);
			groupBox2.Controls.Add(comboBoxEastGrid);
			groupBox2.Controls.Add(label6);
			groupBox2.Controls.Add(comboBoxSouthGrid);
			groupBox2.Controls.Add(label5);
			groupBox2.Controls.Add(comboBoxNorthGrid);
			groupBox2.Location = new System.Drawing.Point(12, 146);
			groupBox2.Name = "groupBox2";
			groupBox2.Size = new System.Drawing.Size(453, 185);
			groupBox2.TabIndex = 3;
			groupBox2.TabStop = false;
			groupBox2.Text = "Horizontal Bounds";
			textBoxWestOffset.Location = new System.Drawing.Point(332, 145);
			textBoxWestOffset.Name = "textBoxWestOffset";
			textBoxWestOffset.Size = new System.Drawing.Size(100, 20);
			textBoxWestOffset.TabIndex = 15;
			textBoxWestOffset.Text = "0' - 0\"";
			label12.AutoSize = true;
			label12.Location = new System.Drawing.Point(288, 148);
			label12.Name = "label12";
			label12.Size = new System.Drawing.Size(38, 13);
			label12.TabIndex = 14;
			label12.Text = "Offset:";
			textBoxEastOffset.Location = new System.Drawing.Point(332, 103);
			textBoxEastOffset.Name = "textBoxEastOffset";
			textBoxEastOffset.Size = new System.Drawing.Size(100, 20);
			textBoxEastOffset.TabIndex = 11;
			textBoxEastOffset.Text = "0' - 0\"";
			label10.AutoSize = true;
			label10.Location = new System.Drawing.Point(288, 106);
			label10.Name = "label10";
			label10.Size = new System.Drawing.Size(38, 13);
			label10.TabIndex = 10;
			label10.Text = "Offset:";
			textBoxSouthOffset.Location = new System.Drawing.Point(332, 61);
			textBoxSouthOffset.Name = "textBoxSouthOffset";
			textBoxSouthOffset.Size = new System.Drawing.Size(100, 20);
			textBoxSouthOffset.TabIndex = 7;
			textBoxSouthOffset.Text = "0' - 0\"";
			label11.AutoSize = true;
			label11.Location = new System.Drawing.Point(6, 148);
			label11.Name = "label11";
			label11.Size = new System.Drawing.Size(35, 13);
			label11.TabIndex = 12;
			label11.Text = "West:";
			label8.AutoSize = true;
			label8.Location = new System.Drawing.Point(288, 64);
			label8.Name = "label8";
			label8.Size = new System.Drawing.Size(38, 13);
			label8.TabIndex = 6;
			label8.Text = "Offset:";
			label9.AutoSize = true;
			label9.Location = new System.Drawing.Point(6, 106);
			label9.Name = "label9";
			label9.Size = new System.Drawing.Size(31, 13);
			label9.TabIndex = 8;
			label9.Text = "East:";
			textBoxNorthOffset.Location = new System.Drawing.Point(332, 19);
			textBoxNorthOffset.Name = "textBoxNorthOffset";
			textBoxNorthOffset.Size = new System.Drawing.Size(100, 20);
			textBoxNorthOffset.TabIndex = 3;
			textBoxNorthOffset.Text = "0' - 0\"";
			comboBoxWestGrid.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			comboBoxWestGrid.FormattingEnabled = true;
			comboBoxWestGrid.Location = new System.Drawing.Point(82, 145);
			comboBoxWestGrid.MaxDropDownItems = 10;
			comboBoxWestGrid.Name = "comboBoxWestGrid";
			comboBoxWestGrid.Size = new System.Drawing.Size(182, 21);
			comboBoxWestGrid.TabIndex = 13;
			label7.AutoSize = true;
			label7.Location = new System.Drawing.Point(6, 64);
			label7.Name = "label7";
			label7.Size = new System.Drawing.Size(38, 13);
			label7.TabIndex = 4;
			label7.Text = "South:";
			comboBoxEastGrid.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			comboBoxEastGrid.FormattingEnabled = true;
			comboBoxEastGrid.Location = new System.Drawing.Point(82, 103);
			comboBoxEastGrid.MaxDropDownItems = 10;
			comboBoxEastGrid.Name = "comboBoxEastGrid";
			comboBoxEastGrid.Size = new System.Drawing.Size(182, 21);
			comboBoxEastGrid.TabIndex = 9;
			label6.AutoSize = true;
			label6.Location = new System.Drawing.Point(288, 22);
			label6.Name = "label6";
			label6.Size = new System.Drawing.Size(38, 13);
			label6.TabIndex = 2;
			label6.Text = "Offset:";
			comboBoxSouthGrid.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			comboBoxSouthGrid.FormattingEnabled = true;
			comboBoxSouthGrid.Location = new System.Drawing.Point(82, 61);
			comboBoxSouthGrid.MaxDropDownItems = 10;
			comboBoxSouthGrid.Name = "comboBoxSouthGrid";
			comboBoxSouthGrid.Size = new System.Drawing.Size(182, 21);
			comboBoxSouthGrid.TabIndex = 5;
			label5.AutoSize = true;
			label5.Location = new System.Drawing.Point(6, 22);
			label5.Name = "label5";
			label5.Size = new System.Drawing.Size(36, 13);
			label5.TabIndex = 0;
			label5.Text = "North:";
			comboBoxNorthGrid.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			comboBoxNorthGrid.FormattingEnabled = true;
			comboBoxNorthGrid.Location = new System.Drawing.Point(82, 19);
			comboBoxNorthGrid.MaxDropDownItems = 10;
			comboBoxNorthGrid.Name = "comboBoxNorthGrid";
			comboBoxNorthGrid.Size = new System.Drawing.Size(182, 21);
			comboBoxNorthGrid.TabIndex = 1;
			textBoxName.Location = new System.Drawing.Point(94, 12);
			textBoxName.Name = "textBoxName";
			textBoxName.Size = new System.Drawing.Size(350, 20);
			textBoxName.TabIndex = 1;
			label13.AutoSize = true;
			label13.Location = new System.Drawing.Point(18, 15);
			label13.Name = "label13";
			label13.Size = new System.Drawing.Size(38, 13);
			label13.TabIndex = 0;
			label13.Text = "Name:";
			base.AcceptButton = buttonOK;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = buttonCancel;
			base.ClientSize = new System.Drawing.Size(477, 372);
			base.Controls.Add(textBoxName);
			base.Controls.Add(groupBox2);
			base.Controls.Add(groupBox1);
			base.Controls.Add(buttonCancel);
			base.Controls.Add(buttonOK);
			base.Controls.Add(label13);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "EditZoneBoundsForm";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = "Edit Zone";
			groupBox1.ResumeLayout(false);
			groupBox1.PerformLayout();
			groupBox2.ResumeLayout(false);
			groupBox2.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
