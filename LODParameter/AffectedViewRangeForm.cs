using Autodesk.Revit.DB;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace LODParameter
{
	public class AffectedViewRangeForm : Form
	{
		private FormattedInputBox inputBoxTopOffset;

		private FormattedInputBox inputBoxBottomOffset;

		private IContainer components = null;

		private GroupBox groupBoxSettings;

		private TextBox textBoxBottomOffset;

		private Label label2;

		private ComboBox comboBoxBottomLevel;

		private Label label1;

		private Button buttonOK;

		private Button buttonCancel;

		private CheckBox checkBoxExcludeColumns;

		private TextBox textBoxTopOffset;

		private Label label4;

		private ComboBox comboBoxTopLevel;

		private Label label3;

		public bool ExcludeColumns => checkBoxExcludeColumns.Checked;

		public double TopOffset => inputBoxTopOffset.Value;

		public double BottomOffset => inputBoxBottomOffset.Value;

		public int TopLevelIndex => comboBoxTopLevel.SelectedIndex;

		public int BottomLevelIndex => comboBoxBottomLevel.SelectedIndex;

		public bool IsTopUnlimited => (string)comboBoxTopLevel.SelectedItem == "Unlimited";

		public bool IsBottomUnlimited => (string)comboBoxBottomLevel.SelectedItem == "Unlimited";

		public AffectedViewRangeForm(string[] levelsInView, Units docUnits)
		{
			InitializeComponent();
			comboBoxTopLevel.Items.AddRange(levelsInView);
			comboBoxBottomLevel.Items.AddRange(levelsInView);
			comboBoxTopLevel.Items.Add("Unlimited");
			comboBoxBottomLevel.Items.Add("Unlimited");
			comboBoxTopLevel.SelectedItem = "Unlimited";
			comboBoxBottomLevel.SelectedItem = "Unlimited";
			inputBoxTopOffset = new FormattedInputBox(textBoxTopOffset, docUnits, 0);
			inputBoxBottomOffset = new FormattedInputBox(textBoxBottomOffset, docUnits, 0);
		}

		private void AffectedViewRangeForm_Shown(object sender, EventArgs e)
		{
			comboBoxTopLevel.Focus();
		}

		private void AffectedViewRangeForm_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				base.DialogResult = DialogResult.Cancel;
				Hide();
			}
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			Hide();
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.OK;
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
			groupBoxSettings = new System.Windows.Forms.GroupBox();
			textBoxTopOffset = new System.Windows.Forms.TextBox();
			label4 = new System.Windows.Forms.Label();
			textBoxBottomOffset = new System.Windows.Forms.TextBox();
			comboBoxTopLevel = new System.Windows.Forms.ComboBox();
			label2 = new System.Windows.Forms.Label();
			label3 = new System.Windows.Forms.Label();
			comboBoxBottomLevel = new System.Windows.Forms.ComboBox();
			label1 = new System.Windows.Forms.Label();
			checkBoxExcludeColumns = new System.Windows.Forms.CheckBox();
			buttonOK = new System.Windows.Forms.Button();
			buttonCancel = new System.Windows.Forms.Button();
			groupBoxSettings.SuspendLayout();
			SuspendLayout();
			groupBoxSettings.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			groupBoxSettings.Controls.Add(textBoxTopOffset);
			groupBoxSettings.Controls.Add(label4);
			groupBoxSettings.Controls.Add(textBoxBottomOffset);
			groupBoxSettings.Controls.Add(comboBoxTopLevel);
			groupBoxSettings.Controls.Add(label2);
			groupBoxSettings.Controls.Add(label3);
			groupBoxSettings.Controls.Add(comboBoxBottomLevel);
			groupBoxSettings.Controls.Add(label1);
			groupBoxSettings.Controls.Add(checkBoxExcludeColumns);
			groupBoxSettings.Location = new System.Drawing.Point(12, 12);
			groupBoxSettings.Name = "groupBoxSettings";
			groupBoxSettings.Size = new System.Drawing.Size(438, 145);
			groupBoxSettings.TabIndex = 0;
			groupBoxSettings.TabStop = false;
			groupBoxSettings.Text = "Affected Range";
			textBoxTopOffset.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			textBoxTopOffset.Location = new System.Drawing.Point(332, 19);
			textBoxTopOffset.Name = "textBoxTopOffset";
			textBoxTopOffset.Size = new System.Drawing.Size(100, 20);
			textBoxTopOffset.TabIndex = 3;
			textBoxTopOffset.Text = "0' - 0\"";
			label4.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			label4.AutoSize = true;
			label4.Location = new System.Drawing.Point(288, 22);
			label4.Name = "label4";
			label4.Size = new System.Drawing.Size(38, 13);
			label4.TabIndex = 2;
			label4.Text = "Offset:";
			textBoxBottomOffset.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			textBoxBottomOffset.Location = new System.Drawing.Point(332, 58);
			textBoxBottomOffset.Name = "textBoxBottomOffset";
			textBoxBottomOffset.Size = new System.Drawing.Size(100, 20);
			textBoxBottomOffset.TabIndex = 7;
			textBoxBottomOffset.Text = "0' - 0\"";
			comboBoxTopLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			comboBoxTopLevel.FormattingEnabled = true;
			comboBoxTopLevel.Location = new System.Drawing.Point(82, 19);
			comboBoxTopLevel.MaxDropDownItems = 10;
			comboBoxTopLevel.Name = "comboBoxTopLevel";
			comboBoxTopLevel.Size = new System.Drawing.Size(182, 21);
			comboBoxTopLevel.TabIndex = 1;
			label2.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(288, 61);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(38, 13);
			label2.TabIndex = 6;
			label2.Text = "Offset:";
			label3.AutoSize = true;
			label3.Location = new System.Drawing.Point(6, 22);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(29, 13);
			label3.TabIndex = 0;
			label3.Text = "Top:";
			comboBoxBottomLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			comboBoxBottomLevel.FormattingEnabled = true;
			comboBoxBottomLevel.Location = new System.Drawing.Point(82, 58);
			comboBoxBottomLevel.MaxDropDownItems = 10;
			comboBoxBottomLevel.Name = "comboBoxBottomLevel";
			comboBoxBottomLevel.Size = new System.Drawing.Size(182, 21);
			comboBoxBottomLevel.TabIndex = 5;
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(6, 61);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(43, 13);
			label1.TabIndex = 4;
			label1.Text = "Bottom:";
			checkBoxExcludeColumns.AutoSize = true;
			checkBoxExcludeColumns.Location = new System.Drawing.Point(99, 104);
			checkBoxExcludeColumns.Name = "checkBoxExcludeColumns";
			checkBoxExcludeColumns.Size = new System.Drawing.Size(155, 17);
			checkBoxExcludeColumns.TabIndex = 8;
			checkBoxExcludeColumns.Text = "Exclude Structural Columns";
			checkBoxExcludeColumns.UseVisualStyleBackColor = true;
			buttonOK.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			buttonOK.Location = new System.Drawing.Point(294, 163);
			buttonOK.Name = "buttonOK";
			buttonOK.Size = new System.Drawing.Size(75, 23);
			buttonOK.TabIndex = 1;
			buttonOK.Text = "OK";
			buttonOK.UseVisualStyleBackColor = true;
			buttonOK.Click += new System.EventHandler(buttonOK_Click);
			buttonCancel.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			buttonCancel.Location = new System.Drawing.Point(375, 163);
			buttonCancel.Name = "buttonCancel";
			buttonCancel.Size = new System.Drawing.Size(75, 23);
			buttonCancel.TabIndex = 2;
			buttonCancel.Text = "Cancel";
			buttonCancel.UseVisualStyleBackColor = true;
			buttonCancel.Click += new System.EventHandler(buttonCancel_Click);
			base.AcceptButton = buttonOK;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = buttonCancel;
			base.ClientSize = new System.Drawing.Size(462, 198);
			base.Controls.Add(buttonCancel);
			base.Controls.Add(buttonOK);
			base.Controls.Add(groupBoxSettings);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "AffectedViewRangeForm";
			base.ShowInTaskbar = false;
			base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = "Select Portion of View to Affect";
			base.Shown += new System.EventHandler(AffectedViewRangeForm_Shown);
			base.KeyDown += new System.Windows.Forms.KeyEventHandler(AffectedViewRangeForm_KeyDown);
			groupBoxSettings.ResumeLayout(false);
			groupBoxSettings.PerformLayout();
			ResumeLayout(false);
		}
	}
}
