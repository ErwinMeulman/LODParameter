using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LODParameter
{
	public class ViewsForZonesForm : Form
	{
		private IList<View3D> m_Views;

		private IContainer components = null;

		private Label label1;

		private Label label2;

		private ComboBox comboBoxViews;

		private Button buttonOK;

		private Button buttonCancel;

		public View3D SelectedView => m_Views[comboBoxViews.SelectedIndex];

		public ViewsForZonesForm(IList<View3D> views, View3D defalutView)
		{
			InitializeComponent();
			m_Views = views;
			string[] array = (from v in views
			select v.get_Name()).ToArray();
			ComboBox.ObjectCollection items = comboBoxViews.Items;
			object[] items2 = array;
			items.AddRange(items2);
			comboBoxViews.SelectedItem = (((defalutView != null) ? defalutView.get_Name() : null) ?? array.FirstOrDefault());
			comboBoxViews.Focus();
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
			comboBoxViews = new System.Windows.Forms.ComboBox();
			buttonOK = new System.Windows.Forms.Button();
			buttonCancel = new System.Windows.Forms.Button();
			SuspendLayout();
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(13, 13);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(313, 13);
			label1.TabIndex = 0;
			label1.Text = "Creates 3D views for each of the Project Zones in the document.";
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(9, 44);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(238, 13);
			label2.TabIndex = 0;
			label2.Text = "Select an existing view to copy visual styles from:";
			comboBoxViews.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			comboBoxViews.FormattingEnabled = true;
			comboBoxViews.Location = new System.Drawing.Point(12, 60);
			comboBoxViews.Name = "comboBoxViews";
			comboBoxViews.Size = new System.Drawing.Size(314, 21);
			comboBoxViews.TabIndex = 1;
			buttonOK.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			buttonOK.Location = new System.Drawing.Point(170, 105);
			buttonOK.Name = "buttonOK";
			buttonOK.Size = new System.Drawing.Size(75, 23);
			buttonOK.TabIndex = 5;
			buttonOK.Text = "OK";
			buttonOK.UseVisualStyleBackColor = true;
			buttonOK.Click += new System.EventHandler(buttonOK_Click);
			buttonCancel.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			buttonCancel.Location = new System.Drawing.Point(251, 105);
			buttonCancel.Name = "buttonCancel";
			buttonCancel.Size = new System.Drawing.Size(75, 23);
			buttonCancel.TabIndex = 6;
			buttonCancel.Text = "Cancel";
			buttonCancel.UseVisualStyleBackColor = true;
			buttonCancel.Click += new System.EventHandler(buttonCancel_Click);
			base.AcceptButton = buttonOK;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = buttonCancel;
			base.ClientSize = new System.Drawing.Size(338, 140);
			base.Controls.Add(buttonCancel);
			base.Controls.Add(buttonOK);
			base.Controls.Add(comboBoxViews);
			base.Controls.Add(label2);
			base.Controls.Add(label1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "ViewsForZonesForm";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = "Create Zone Views";
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
