using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace LODParameter
{
	public class LODfilterForm : Form
	{
		private IContainer components = null;

		private GroupBox groupBox1;

		private CheckBox checkBoxVisibility400;

		private CheckBox checkBoxFill400;

		private CheckBox checkBoxVisibility350;

		private CheckBox checkBoxFill350;

		private CheckBox checkBoxVisibility300;

		private CheckBox checkBoxFill300;

		private CheckBox checkBoxVisibility200;

		private CheckBox checkBoxFill200;

		private Label labelLOD400;

		private Label labelLOD350;

		private Label labelLOD300;

		private Label labelFill;

		private Label labelLOD200;

		private Label labelVisibility;

		private NumericUpDown numericTrans400;

		private NumericUpDown numericTrans350;

		private NumericUpDown numericTrans300;

		private NumericUpDown numericTrans200;

		private Label labelTransparency;

		private Button buttonApply;

		private Button buttonCancel;

		private Label label1;

		private CheckBox checkBoxLine400;

		private CheckBox checkBoxLine350;

		private CheckBox checkBoxLine300;

		private CheckBox checkBoxLine200;

		private Label labelLine;

		private Label labelColor;

		public LODfilterForm()
		{
			InitializeComponent();
		}

		public bool[] GetFillColorEnabled()
		{
			return new bool[4]
			{
				checkBoxFill200.Checked,
				checkBoxFill300.Checked,
				checkBoxFill350.Checked,
				checkBoxFill400.Checked
			};
		}

		public bool[] GetLineColorEnabled()
		{
			return new bool[4]
			{
				checkBoxLine200.Checked,
				checkBoxLine300.Checked,
				checkBoxLine350.Checked,
				checkBoxLine400.Checked
			};
		}

		public bool[] GetVisibilitesEnabled()
		{
			return new bool[4]
			{
				checkBoxVisibility200.Checked,
				checkBoxVisibility300.Checked,
				checkBoxVisibility350.Checked,
				checkBoxVisibility400.Checked
			};
		}

		public int[] GetTransparencies()
		{
			return new int[4]
			{
				(int)numericTrans200.Value,
				(int)numericTrans300.Value,
				(int)numericTrans350.Value,
				(int)numericTrans400.Value
			};
		}

		private void LODfilterForm_KeyDown(object sender, KeyEventArgs e)
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

		private void buttonApply_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.OK;
			Hide();
		}

		private void labelColor_Click(object sender, EventArgs e)
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
			groupBox1 = new System.Windows.Forms.GroupBox();
			numericTrans400 = new System.Windows.Forms.NumericUpDown();
			numericTrans350 = new System.Windows.Forms.NumericUpDown();
			numericTrans300 = new System.Windows.Forms.NumericUpDown();
			numericTrans200 = new System.Windows.Forms.NumericUpDown();
			checkBoxVisibility400 = new System.Windows.Forms.CheckBox();
			checkBoxFill400 = new System.Windows.Forms.CheckBox();
			checkBoxVisibility350 = new System.Windows.Forms.CheckBox();
			checkBoxFill350 = new System.Windows.Forms.CheckBox();
			checkBoxVisibility300 = new System.Windows.Forms.CheckBox();
			checkBoxFill300 = new System.Windows.Forms.CheckBox();
			checkBoxVisibility200 = new System.Windows.Forms.CheckBox();
			checkBoxFill200 = new System.Windows.Forms.CheckBox();
			labelLOD400 = new System.Windows.Forms.Label();
			labelLOD350 = new System.Windows.Forms.Label();
			labelLOD300 = new System.Windows.Forms.Label();
			labelTransparency = new System.Windows.Forms.Label();
			labelVisibility = new System.Windows.Forms.Label();
			labelFill = new System.Windows.Forms.Label();
			labelLOD200 = new System.Windows.Forms.Label();
			buttonApply = new System.Windows.Forms.Button();
			buttonCancel = new System.Windows.Forms.Button();
			label1 = new System.Windows.Forms.Label();
			labelLine = new System.Windows.Forms.Label();
			checkBoxLine200 = new System.Windows.Forms.CheckBox();
			checkBoxLine300 = new System.Windows.Forms.CheckBox();
			checkBoxLine350 = new System.Windows.Forms.CheckBox();
			checkBoxLine400 = new System.Windows.Forms.CheckBox();
			labelColor = new System.Windows.Forms.Label();
			groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)numericTrans400).BeginInit();
			((System.ComponentModel.ISupportInitialize)numericTrans350).BeginInit();
			((System.ComponentModel.ISupportInitialize)numericTrans300).BeginInit();
			((System.ComponentModel.ISupportInitialize)numericTrans200).BeginInit();
			SuspendLayout();
			groupBox1.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			groupBox1.Controls.Add(numericTrans400);
			groupBox1.Controls.Add(numericTrans350);
			groupBox1.Controls.Add(numericTrans300);
			groupBox1.Controls.Add(numericTrans200);
			groupBox1.Controls.Add(checkBoxVisibility400);
			groupBox1.Controls.Add(checkBoxLine400);
			groupBox1.Controls.Add(checkBoxFill400);
			groupBox1.Controls.Add(checkBoxVisibility350);
			groupBox1.Controls.Add(checkBoxLine350);
			groupBox1.Controls.Add(checkBoxFill350);
			groupBox1.Controls.Add(checkBoxVisibility300);
			groupBox1.Controls.Add(checkBoxLine300);
			groupBox1.Controls.Add(checkBoxFill300);
			groupBox1.Controls.Add(checkBoxVisibility200);
			groupBox1.Controls.Add(checkBoxLine200);
			groupBox1.Controls.Add(checkBoxFill200);
			groupBox1.Controls.Add(labelLOD400);
			groupBox1.Controls.Add(labelLOD350);
			groupBox1.Controls.Add(labelLOD300);
			groupBox1.Controls.Add(labelTransparency);
			groupBox1.Controls.Add(labelLine);
			groupBox1.Controls.Add(labelVisibility);
			groupBox1.Controls.Add(labelColor);
			groupBox1.Controls.Add(labelFill);
			groupBox1.Controls.Add(labelLOD200);
			groupBox1.Location = new System.Drawing.Point(12, 12);
			groupBox1.Name = "groupBox1";
			groupBox1.Size = new System.Drawing.Size(302, 156);
			groupBox1.TabIndex = 0;
			groupBox1.TabStop = false;
			groupBox1.Text = "Filters";
			numericTrans400.Increment = new decimal(new int[4]
			{
				10,
				0,
				0,
				0
			});
			numericTrans400.Location = new System.Drawing.Point(220, 128);
			numericTrans400.Name = "numericTrans400";
			numericTrans400.Size = new System.Drawing.Size(69, 20);
			numericTrans400.TabIndex = 2;
			numericTrans350.Increment = new decimal(new int[4]
			{
				10,
				0,
				0,
				0
			});
			numericTrans350.Location = new System.Drawing.Point(220, 102);
			numericTrans350.Name = "numericTrans350";
			numericTrans350.Size = new System.Drawing.Size(69, 20);
			numericTrans350.TabIndex = 2;
			numericTrans300.Increment = new decimal(new int[4]
			{
				10,
				0,
				0,
				0
			});
			numericTrans300.Location = new System.Drawing.Point(220, 76);
			numericTrans300.Name = "numericTrans300";
			numericTrans300.Size = new System.Drawing.Size(69, 20);
			numericTrans300.TabIndex = 2;
			numericTrans200.Increment = new decimal(new int[4]
			{
				10,
				0,
				0,
				0
			});
			numericTrans200.Location = new System.Drawing.Point(220, 50);
			numericTrans200.Name = "numericTrans200";
			numericTrans200.Size = new System.Drawing.Size(69, 20);
			numericTrans200.TabIndex = 2;
			checkBoxVisibility400.AutoSize = true;
			checkBoxVisibility400.Checked = true;
			checkBoxVisibility400.CheckState = System.Windows.Forms.CheckState.Checked;
			checkBoxVisibility400.Location = new System.Drawing.Point(165, 130);
			checkBoxVisibility400.Name = "checkBoxVisibility400";
			checkBoxVisibility400.Size = new System.Drawing.Size(15, 14);
			checkBoxVisibility400.TabIndex = 1;
			checkBoxVisibility400.UseVisualStyleBackColor = true;
			checkBoxFill400.AutoSize = true;
			checkBoxFill400.Location = new System.Drawing.Point(78, 130);
			checkBoxFill400.Name = "checkBoxFill400";
			checkBoxFill400.Size = new System.Drawing.Size(15, 14);
			checkBoxFill400.TabIndex = 1;
			checkBoxFill400.UseVisualStyleBackColor = true;
			checkBoxVisibility350.AutoSize = true;
			checkBoxVisibility350.Checked = true;
			checkBoxVisibility350.CheckState = System.Windows.Forms.CheckState.Checked;
			checkBoxVisibility350.Location = new System.Drawing.Point(165, 104);
			checkBoxVisibility350.Name = "checkBoxVisibility350";
			checkBoxVisibility350.Size = new System.Drawing.Size(15, 14);
			checkBoxVisibility350.TabIndex = 1;
			checkBoxVisibility350.UseVisualStyleBackColor = true;
			checkBoxFill350.AutoSize = true;
			checkBoxFill350.Location = new System.Drawing.Point(78, 104);
			checkBoxFill350.Name = "checkBoxFill350";
			checkBoxFill350.Size = new System.Drawing.Size(15, 14);
			checkBoxFill350.TabIndex = 1;
			checkBoxFill350.UseVisualStyleBackColor = true;
			checkBoxVisibility300.AutoSize = true;
			checkBoxVisibility300.Checked = true;
			checkBoxVisibility300.CheckState = System.Windows.Forms.CheckState.Checked;
			checkBoxVisibility300.Location = new System.Drawing.Point(165, 78);
			checkBoxVisibility300.Name = "checkBoxVisibility300";
			checkBoxVisibility300.Size = new System.Drawing.Size(15, 14);
			checkBoxVisibility300.TabIndex = 1;
			checkBoxVisibility300.UseVisualStyleBackColor = true;
			checkBoxFill300.AutoSize = true;
			checkBoxFill300.Location = new System.Drawing.Point(78, 78);
			checkBoxFill300.Name = "checkBoxFill300";
			checkBoxFill300.Size = new System.Drawing.Size(15, 14);
			checkBoxFill300.TabIndex = 1;
			checkBoxFill300.UseVisualStyleBackColor = true;
			checkBoxVisibility200.AutoSize = true;
			checkBoxVisibility200.Checked = true;
			checkBoxVisibility200.CheckState = System.Windows.Forms.CheckState.Checked;
			checkBoxVisibility200.Location = new System.Drawing.Point(165, 52);
			checkBoxVisibility200.Name = "checkBoxVisibility200";
			checkBoxVisibility200.Size = new System.Drawing.Size(15, 14);
			checkBoxVisibility200.TabIndex = 1;
			checkBoxVisibility200.UseVisualStyleBackColor = true;
			checkBoxFill200.AutoSize = true;
			checkBoxFill200.Location = new System.Drawing.Point(78, 52);
			checkBoxFill200.Name = "checkBoxFill200";
			checkBoxFill200.Size = new System.Drawing.Size(15, 14);
			checkBoxFill200.TabIndex = 1;
			checkBoxFill200.UseVisualStyleBackColor = true;
			labelLOD400.AutoSize = true;
			labelLOD400.Location = new System.Drawing.Point(6, 131);
			labelLOD400.Name = "labelLOD400";
			labelLOD400.Size = new System.Drawing.Size(47, 13);
			labelLOD400.TabIndex = 0;
			labelLOD400.Text = "LOD400";
			labelLOD350.AutoSize = true;
			labelLOD350.Location = new System.Drawing.Point(6, 105);
			labelLOD350.Name = "labelLOD350";
			labelLOD350.Size = new System.Drawing.Size(47, 13);
			labelLOD350.TabIndex = 0;
			labelLOD350.Text = "LOD350";
			labelLOD300.AutoSize = true;
			labelLOD300.Location = new System.Drawing.Point(6, 79);
			labelLOD300.Name = "labelLOD300";
			labelLOD300.Size = new System.Drawing.Size(47, 13);
			labelLOD300.TabIndex = 0;
			labelLOD300.Text = "LOD300";
			labelTransparency.AutoSize = true;
			labelTransparency.Location = new System.Drawing.Point(217, 27);
			labelTransparency.Name = "labelTransparency";
			labelTransparency.Size = new System.Drawing.Size(72, 13);
			labelTransparency.TabIndex = 0;
			labelTransparency.Text = "Transparency";
			labelTransparency.Click += new System.EventHandler(labelColor_Click);
			labelVisibility.AutoSize = true;
			labelVisibility.Location = new System.Drawing.Point(152, 27);
			labelVisibility.Name = "labelVisibility";
			labelVisibility.Size = new System.Drawing.Size(43, 13);
			labelVisibility.TabIndex = 0;
			labelVisibility.Text = "Visibility";
			labelVisibility.Click += new System.EventHandler(labelColor_Click);
			labelFill.AutoSize = true;
			labelFill.Location = new System.Drawing.Point(75, 27);
			labelFill.Name = "labelFill";
			labelFill.Size = new System.Drawing.Size(19, 13);
			labelFill.TabIndex = 0;
			labelFill.Text = "Fill";
			labelLOD200.AutoSize = true;
			labelLOD200.Location = new System.Drawing.Point(6, 52);
			labelLOD200.Name = "labelLOD200";
			labelLOD200.Size = new System.Drawing.Size(47, 13);
			labelLOD200.TabIndex = 0;
			labelLOD200.Text = "LOD200";
			buttonApply.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			buttonApply.Location = new System.Drawing.Point(120, 193);
			buttonApply.Name = "buttonApply";
			buttonApply.Size = new System.Drawing.Size(94, 23);
			buttonApply.TabIndex = 1;
			buttonApply.Text = "Apply";
			buttonApply.UseVisualStyleBackColor = true;
			buttonApply.Click += new System.EventHandler(buttonApply_Click);
			buttonCancel.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			buttonCancel.Location = new System.Drawing.Point(220, 193);
			buttonCancel.Name = "buttonCancel";
			buttonCancel.Size = new System.Drawing.Size(94, 23);
			buttonCancel.TabIndex = 1;
			buttonCancel.Text = "Cancel";
			buttonCancel.UseVisualStyleBackColor = true;
			buttonCancel.Click += new System.EventHandler(buttonCancel_Click);
			label1.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(21, 171);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(160, 13);
			label1.TabIndex = 2;
			label1.Text = "Note: Filters apply to active view";
			labelLine.AutoSize = true;
			labelLine.Location = new System.Drawing.Point(104, 27);
			labelLine.Name = "labelLine";
			labelLine.Size = new System.Drawing.Size(27, 13);
			labelLine.TabIndex = 0;
			labelLine.Text = "Line";
			checkBoxLine200.AutoSize = true;
			checkBoxLine200.Location = new System.Drawing.Point(112, 52);
			checkBoxLine200.Name = "checkBoxLine200";
			checkBoxLine200.Size = new System.Drawing.Size(15, 14);
			checkBoxLine200.TabIndex = 1;
			checkBoxLine200.UseVisualStyleBackColor = true;
			checkBoxLine300.AutoSize = true;
			checkBoxLine300.Location = new System.Drawing.Point(112, 78);
			checkBoxLine300.Name = "checkBoxLine300";
			checkBoxLine300.Size = new System.Drawing.Size(15, 14);
			checkBoxLine300.TabIndex = 1;
			checkBoxLine300.UseVisualStyleBackColor = true;
			checkBoxLine350.AutoSize = true;
			checkBoxLine350.Location = new System.Drawing.Point(112, 104);
			checkBoxLine350.Name = "checkBoxLine350";
			checkBoxLine350.Size = new System.Drawing.Size(15, 14);
			checkBoxLine350.TabIndex = 1;
			checkBoxLine350.UseVisualStyleBackColor = true;
			checkBoxLine400.AutoSize = true;
			checkBoxLine400.Location = new System.Drawing.Point(112, 130);
			checkBoxLine400.Name = "checkBoxLine400";
			checkBoxLine400.Size = new System.Drawing.Size(15, 14);
			checkBoxLine400.TabIndex = 1;
			checkBoxLine400.UseVisualStyleBackColor = true;
			labelColor.AutoSize = true;
			labelColor.Location = new System.Drawing.Point(85, 13);
			labelColor.Name = "labelColor";
			labelColor.Size = new System.Drawing.Size(31, 13);
			labelColor.TabIndex = 0;
			labelColor.Text = "Color";
			base.AcceptButton = buttonApply;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = buttonCancel;
			base.ClientSize = new System.Drawing.Size(326, 228);
			base.Controls.Add(label1);
			base.Controls.Add(buttonCancel);
			base.Controls.Add(buttonApply);
			base.Controls.Add(groupBox1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "LODfilterForm";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = "Apply Filters by LOD Value";
			base.KeyDown += new System.Windows.Forms.KeyEventHandler(LODfilterForm_KeyDown);
			groupBox1.ResumeLayout(false);
			groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)numericTrans400).EndInit();
			((System.ComponentModel.ISupportInitialize)numericTrans350).EndInit();
			((System.ComponentModel.ISupportInitialize)numericTrans300).EndInit();
			((System.ComponentModel.ISupportInitialize)numericTrans200).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
