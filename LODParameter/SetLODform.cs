using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace LODParameter
{
	public class SetLODform : Form
	{
		private string m_SelectedLODtype;

		private int m_SelectedLODvalue;

		private IContainer components = null;

		private GroupBox groupBox1;

		private RadioButton radioTargetLOD;

		private RadioButton radioCurrentLOD;

		private Label labelLODvalue;

		private Button buttonOK;

		private Button buttonCancel;

		private ListBox listBoxLODvalue;

		public string SelectedLODtype => m_SelectedLODtype;

		public int SelectedLODvalue => m_SelectedLODvalue;

		public SetLODform()
		{
			InitializeComponent();
			listBoxLODvalue.SelectedIndex = 1;
		}

		private void SetLODform_Shown(object sender, EventArgs e)
		{
			listBoxLODvalue.Focus();
		}

		private void SetLODform_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				base.DialogResult = DialogResult.Cancel;
				Hide();
			}
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			if (radioCurrentLOD.Checked)
			{
				m_SelectedLODtype = "Current_LOD";
			}
			else
			{
				m_SelectedLODtype = "Target_LOD";
			}
			switch (listBoxLODvalue.SelectedIndex)
			{
			case 0:
				m_SelectedLODvalue = 200;
				break;
			case 1:
				m_SelectedLODvalue = 300;
				break;
			case 2:
				m_SelectedLODvalue = 350;
				break;
			case 3:
				m_SelectedLODvalue = 400;
				break;
			default:
				throw new Exception("Illegal value from LOD drop down box");
			}
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
			groupBox1 = new System.Windows.Forms.GroupBox();
			listBoxLODvalue = new System.Windows.Forms.ListBox();
			radioTargetLOD = new System.Windows.Forms.RadioButton();
			radioCurrentLOD = new System.Windows.Forms.RadioButton();
			labelLODvalue = new System.Windows.Forms.Label();
			buttonOK = new System.Windows.Forms.Button();
			buttonCancel = new System.Windows.Forms.Button();
			groupBox1.SuspendLayout();
			SuspendLayout();
			groupBox1.Controls.Add(listBoxLODvalue);
			groupBox1.Controls.Add(radioTargetLOD);
			groupBox1.Controls.Add(radioCurrentLOD);
			groupBox1.Controls.Add(labelLODvalue);
			groupBox1.Location = new System.Drawing.Point(12, 12);
			groupBox1.Name = "groupBox1";
			groupBox1.Size = new System.Drawing.Size(211, 119);
			groupBox1.TabIndex = 0;
			groupBox1.TabStop = false;
			groupBox1.Text = "Settings";
			listBoxLODvalue.FormattingEnabled = true;
			listBoxLODvalue.Items.AddRange(new object[4]
			{
				"LOD200 - Approximate",
				"LOD300 - Specific",
				"LOD350 - Detailed, Coordination-Ready",
				"LOD400 - Fabrication"
			});
			listBoxLODvalue.Location = new System.Drawing.Point(6, 55);
			listBoxLODvalue.Name = "listBoxLODvalue";
			listBoxLODvalue.Size = new System.Drawing.Size(199, 56);
			listBoxLODvalue.TabIndex = 2;
			radioTargetLOD.AutoSize = true;
			radioTargetLOD.Location = new System.Drawing.Point(99, 19);
			radioTargetLOD.Name = "radioTargetLOD";
			radioTargetLOD.Size = new System.Drawing.Size(81, 17);
			radioTargetLOD.TabIndex = 1;
			radioTargetLOD.Text = "Target LOD";
			radioTargetLOD.UseVisualStyleBackColor = true;
			radioCurrentLOD.AutoSize = true;
			radioCurrentLOD.Checked = true;
			radioCurrentLOD.Location = new System.Drawing.Point(9, 19);
			radioCurrentLOD.Name = "radioCurrentLOD";
			radioCurrentLOD.Size = new System.Drawing.Size(84, 17);
			radioCurrentLOD.TabIndex = 0;
			radioCurrentLOD.TabStop = true;
			radioCurrentLOD.Text = "Current LOD";
			radioCurrentLOD.UseVisualStyleBackColor = true;
			labelLODvalue.AutoSize = true;
			labelLODvalue.Location = new System.Drawing.Point(6, 39);
			labelLODvalue.Name = "labelLODvalue";
			labelLODvalue.Size = new System.Drawing.Size(62, 13);
			labelLODvalue.TabIndex = 1;
			labelLODvalue.Text = "LOD Value:";
			buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			buttonOK.Location = new System.Drawing.Point(67, 137);
			buttonOK.Name = "buttonOK";
			buttonOK.Size = new System.Drawing.Size(75, 23);
			buttonOK.TabIndex = 1;
			buttonOK.Text = "Set LOD";
			buttonOK.UseVisualStyleBackColor = true;
			buttonOK.Click += new System.EventHandler(buttonOK_Click);
			buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			buttonCancel.Location = new System.Drawing.Point(148, 137);
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
			base.ClientSize = new System.Drawing.Size(235, 171);
			base.Controls.Add(buttonCancel);
			base.Controls.Add(buttonOK);
			base.Controls.Add(groupBox1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.KeyPreview = true;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "SetLODform";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = "Select LOD Value to Set";
			base.Shown += new System.EventHandler(SetLODform_Shown);
			base.KeyDown += new System.Windows.Forms.KeyEventHandler(SetLODform_KeyDown);
			groupBox1.ResumeLayout(false);
			groupBox1.PerformLayout();
			ResumeLayout(false);
		}
	}
}
