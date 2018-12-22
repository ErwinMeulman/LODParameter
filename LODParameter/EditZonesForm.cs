using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LODParameter
{
	public class EditZonesForm : Form
	{
		protected class LevelOffsetPair
		{
			public Level Level
			{
				get;
			}

			public double Offset
			{
				get;
			}

			public LevelOffsetPair(Level level, double offset)
			{
				Level = level;
				Offset = offset;
			}
		}

		protected class GridOffsetPair
		{
			public Grid Grid
			{
				get;
			}

			public double Offset
			{
				get;
			}

			public GridOffsetPair(Grid grid, double offset)
			{
				Grid = grid;
				Offset = offset;
			}
		}

		protected internal const string UNBOUND_STRING = "Origin";

		private SortedList<double, Level> m_Levels;

		private SortedList<double, Grid> m_GridsNorthSouth;

		private SortedList<double, Grid> m_GridsEastWest;

		private Units m_docUnits;

		private IContainer components = null;

		private ListView listView;

		private ColumnHeader colZone;

		private ColumnHeader colBot;

		private ColumnHeader colTop;

		private ColumnHeader colNorth;

		private ColumnHeader colSouth;

		private ColumnHeader colEast;

		private ColumnHeader colWest;

		private Button buttonOK;

		private Button buttonCancel;

		private Button buttonNew;

		private Button buttonDelete;

		private Button buttonEdit;

		private LinkLabel linkCreateZonesByLevel;

		public IList<ZoneData> EditedZones => (from ListViewItem item in listView.Items
		select item.Tag as ZoneData).ToList();

		public EditZonesForm(ICollection<ZoneData> zones, SortedList<double, Level> levels, SortedList<double, Grid> gridsNorthSouth, SortedList<double, Grid> gridsEastWest, Units docUnits)
		{
			InitializeComponent();
			m_Levels = levels;
			m_GridsNorthSouth = gridsNorthSouth;
			m_GridsEastWest = gridsEastWest;
			m_docUnits = docUnits;
			foreach (ZoneData zone in zones)
			{
				listView.Items.Add(ListViewItemFromZone(zone, m_docUnits));
			}
		}

		private static ListViewItem ListViewItemFromZone(ZoneData zone, Units docUnits)
		{
			ListViewItem listViewItem = new ListViewItem(zone.Name);
			listViewItem.Tag = zone;
			string[] array = new string[12]
			{
				"Bottom",
				FormatDatumOffsetString(zone.BaseLevel, zone.BaseOffset, docUnits),
				"Top",
				FormatDatumOffsetString(zone.TopLevel, zone.TopOffset, docUnits),
				"North",
				FormatDatumOffsetString(zone.NorthGrid, zone.NorthOffset, docUnits),
				"South",
				FormatDatumOffsetString(zone.SouthGrid, zone.SouthOffset, docUnits),
				"East",
				FormatDatumOffsetString(zone.EastGrid, zone.EastOffset, docUnits),
				"West",
				FormatDatumOffsetString(zone.WestGrid, zone.WestOffset, docUnits)
			};
			for (int i = 0; i < array.Length; i += 2)
			{
				ListViewItem.ListViewSubItem listViewSubItem = new ListViewItem.ListViewSubItem();
				listViewSubItem.Name = array[i];
				listViewSubItem.Text = array[i + 1];
				listViewItem.SubItems.Add(listViewSubItem);
			}
			return listViewItem;
		}

		private static string FormatDatumOffsetString(DatumPlane datum, double offset, Units docUnits)
		{
			if (datum == null)
			{
				return UnitFormatUtils.Format(docUnits, 0, offset, false, false);
			}
			double num = Math.Abs(offset);
			bool flag = offset < 0.0;
			return "(" + datum.get_Name() + ")" + (flag ? " - " : " + ") + UnitFormatUtils.Format(docUnits, 0, num, false, false);
		}

		private void EditProjectZone(ListViewItem zoneItem)
		{
			ZoneData zoneData = zoneItem.Tag as ZoneData;
			EditZoneBoundsForm editZoneBoundsForm = new EditZoneBoundsForm(zoneData, m_Levels, m_GridsNorthSouth, m_GridsEastWest, m_docUnits);
			editZoneBoundsForm.ShowDialog();
			if (editZoneBoundsForm.DialogResult == DialogResult.OK)
			{
				zoneData.Name = editZoneBoundsForm.ZoneName;
				zoneData.TopLevel = editZoneBoundsForm.SelectedTopLevel;
				zoneData.BaseLevel = editZoneBoundsForm.SelectedBaseLevel;
				zoneData.NorthGrid = editZoneBoundsForm.SelectedNorthGrid;
				zoneData.SouthGrid = editZoneBoundsForm.SelectedSouthGrid;
				zoneData.EastGrid = editZoneBoundsForm.SelectedEastGrid;
				zoneData.WestGrid = editZoneBoundsForm.SelectedWestGrid;
				zoneData.TopOffset = editZoneBoundsForm.TopOffset;
				zoneData.BaseOffset = editZoneBoundsForm.BaseOffset;
				zoneData.NorthOffset = editZoneBoundsForm.NorthOffset;
				zoneData.SouthOffset = editZoneBoundsForm.SouthOffset;
				zoneData.EastOffset = editZoneBoundsForm.EastOffset;
				zoneData.WestOffset = editZoneBoundsForm.WestOffset;
			}
			zoneItem.ListView.Items[zoneItem.Index] = ListViewItemFromZone(zoneData, m_docUnits);
		}

		private void DeleteProjectZone(ListViewItem zoneItem)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Expected O, but got Unknown
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Invalid comparison between Unknown and I4
			string str = (zoneItem.Text != string.Empty) ? zoneItem.Text : "the selected zone";
			TaskDialog val = new TaskDialog("Confirm Delete");
			val.set_MainInstruction("Are you sure you want to delete " + str + "?");
			val.set_CommonButtons(6);
			val.set_DefaultButton(7);
			TaskDialogResult val2 = val.Show();
			if ((int)val2 == 6)
			{
				listView.Items.Remove(zoneItem);
			}
		}

		private void CreateZonesByLevel()
		{
			CreateZonesByLevelForm createZonesByLevelForm = new CreateZonesByLevelForm(m_Levels, m_GridsNorthSouth, m_GridsEastWest, m_docUnits);
			createZonesByLevelForm.ShowDialog();
			if (createZonesByLevelForm.DialogResult == DialogResult.OK)
			{
				SortedList<double, Level> selectedLevels = createZonesByLevelForm.SelectedLevels;
				SortedList<double, Grid> selectedGridsNorthSouth = createZonesByLevelForm.SelectedGridsNorthSouth;
				SortedList<double, Grid> selectedGridsEastWest = createZonesByLevelForm.SelectedGridsEastWest;
				double overallLevelOffset = createZonesByLevelForm.OverallLevelOffset;
				List<LevelOffsetPair> list = new List<LevelOffsetPair>(selectedLevels.Count + 2);
				Level val = m_Levels.Values.FirstOrDefault();
				if (createZonesByLevelForm.HasLowerBound && val != null)
				{
					list.Add(new LevelOffsetPair(val, -20.0));
				}
				foreach (Level value in selectedLevels.Values)
				{
					list.Add(new LevelOffsetPair(value, overallLevelOffset));
				}
				Level val2 = m_Levels.Values.LastOrDefault();
				if (createZonesByLevelForm.HasUpperBound && val2 != null)
				{
					list.Add(new LevelOffsetPair(val2, 20.0));
				}
				List<GridOffsetPair> list2 = new List<GridOffsetPair>(selectedGridsNorthSouth.Count + 2);
				if (createZonesByLevelForm.HasWestBound)
				{
					list2.Add(new GridOffsetPair(null, 0.0));
				}
				foreach (Grid value2 in selectedGridsNorthSouth.Values)
				{
					list2.Add(new GridOffsetPair(value2, 0.0));
				}
				if (createZonesByLevelForm.HasEastBound)
				{
					list2.Add(new GridOffsetPair(m_GridsNorthSouth.Values.LastOrDefault(), 20.0));
				}
				List<GridOffsetPair> list3 = new List<GridOffsetPair>(selectedGridsEastWest.Count + 2);
				if (createZonesByLevelForm.HasSouthBound)
				{
					list3.Add(new GridOffsetPair(null, 0.0));
				}
				foreach (Grid value3 in selectedGridsEastWest.Values)
				{
					list3.Add(new GridOffsetPair(value3, 0.0));
				}
				if (createZonesByLevelForm.HasNorthBound)
				{
					list3.Add(new GridOffsetPair(m_GridsEastWest.Values.LastOrDefault(), 20.0));
				}
				bool flag = list3.Count > 2 || list2.Count > 2;
				IList<ZoneData> list4 = new List<ZoneData>();
				for (int i = 0; i < list.Count - 1; i++)
				{
					char c = 'A';
					for (int j = 0; j < list2.Count - 1; j++)
					{
						for (int k = 0; k < list3.Count - 1; k++)
						{
							ZoneData zoneData = new ZoneData();
							if (flag)
							{
								ZoneData zoneData2 = zoneData;
								object arg = i;
								char num = c;
								c = (char)(num + 1);
								zoneData2.Name = $"Z-{arg:00}{num}";
							}
							else
							{
								zoneData.Name = $"Z-{i:00}";
							}
							zoneData.BaseLevel = list[i].Level;
							zoneData.BaseOffset = list[i].Offset;
							zoneData.WestGrid = list2[j].Grid;
							zoneData.WestOffset = list2[j].Offset;
							zoneData.SouthGrid = list3[k].Grid;
							zoneData.SouthOffset = list3[k].Offset;
							zoneData.TopLevel = list[i + 1].Level;
							zoneData.TopOffset = list[i + 1].Offset;
							zoneData.EastGrid = list2[j + 1].Grid;
							zoneData.EastOffset = list2[j + 1].Offset;
							zoneData.NorthGrid = list3[k + 1].Grid;
							zoneData.NorthOffset = list3[k + 1].Offset;
							list4.Add(zoneData);
						}
					}
				}
				ListViewItem[] items = (from zd in list4
				select ListViewItemFromZone(zd, m_docUnits)).ToArray();
				listView.Items.AddRange(items);
			}
		}

		private void buttonNew_Click(object sender, EventArgs e)
		{
			ZoneData zone = new ZoneData();
			ListViewItem listViewItem = ListViewItemFromZone(zone, m_docUnits);
			listView.Items.Add(listViewItem);
			EditProjectZone(listViewItem);
		}

		private void listView_DoubleClick(object sender, EventArgs e)
		{
			if (listView.SelectedItems.Count > 0)
			{
				EditProjectZone(listView.SelectedItems[0]);
			}
		}

		private void buttonEdit_Click(object sender, EventArgs e)
		{
			if (listView.SelectedItems.Count > 0)
			{
				EditProjectZone(listView.SelectedItems[0]);
			}
		}

		private void buttonDelete_Click(object sender, EventArgs e)
		{
			if (listView.SelectedItems.Count > 0)
			{
				DeleteProjectZone(listView.SelectedItems[0]);
			}
		}

		private void EditZonesForm_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
			case Keys.Return:
				if (listView.SelectedItems.Count > 0)
				{
					EditProjectZone(listView.SelectedItems[0]);
					e.Handled = true;
				}
				else
				{
					buttonOK_Click(sender, e);
				}
				break;
			case Keys.Delete:
				if (listView.SelectedItems.Count > 0)
				{
					DeleteProjectZone(listView.SelectedItems[0]);
					e.Handled = true;
				}
				break;
			case Keys.Escape:
				base.DialogResult = DialogResult.Cancel;
				Hide();
				break;
			}
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

		private void linkCreateZonesByLevel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			CreateZonesByLevel();
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
			listView = new System.Windows.Forms.ListView();
			colZone = new System.Windows.Forms.ColumnHeader();
			colBot = new System.Windows.Forms.ColumnHeader();
			colTop = new System.Windows.Forms.ColumnHeader();
			colNorth = new System.Windows.Forms.ColumnHeader();
			colSouth = new System.Windows.Forms.ColumnHeader();
			colEast = new System.Windows.Forms.ColumnHeader();
			colWest = new System.Windows.Forms.ColumnHeader();
			buttonOK = new System.Windows.Forms.Button();
			buttonCancel = new System.Windows.Forms.Button();
			buttonNew = new System.Windows.Forms.Button();
			buttonDelete = new System.Windows.Forms.Button();
			buttonEdit = new System.Windows.Forms.Button();
			linkCreateZonesByLevel = new System.Windows.Forms.LinkLabel();
			SuspendLayout();
			listView.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[7]
			{
				colZone,
				colBot,
				colTop,
				colNorth,
				colSouth,
				colEast,
				colWest
			});
			listView.FullRowSelect = true;
			listView.HideSelection = false;
			listView.Location = new System.Drawing.Point(12, 12);
			listView.MultiSelect = false;
			listView.Name = "listView";
			listView.Size = new System.Drawing.Size(885, 276);
			listView.Sorting = System.Windows.Forms.SortOrder.Ascending;
			listView.TabIndex = 0;
			listView.UseCompatibleStateImageBehavior = false;
			listView.View = System.Windows.Forms.View.Details;
			listView.DoubleClick += new System.EventHandler(listView_DoubleClick);
			colZone.Text = "Zone";
			colZone.Width = 120;
			colBot.Text = "Bottom";
			colBot.Width = 180;
			colTop.Text = "Top";
			colTop.Width = 180;
			colNorth.Text = "North";
			colNorth.Width = 100;
			colSouth.Text = "South";
			colSouth.Width = 100;
			colEast.Text = "East";
			colEast.Width = 100;
			colWest.Text = "West";
			colWest.Width = 100;
			buttonOK.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			buttonOK.Location = new System.Drawing.Point(741, 294);
			buttonOK.Name = "buttonOK";
			buttonOK.Size = new System.Drawing.Size(75, 23);
			buttonOK.TabIndex = 5;
			buttonOK.Text = "OK";
			buttonOK.UseVisualStyleBackColor = true;
			buttonOK.Click += new System.EventHandler(buttonOK_Click);
			buttonCancel.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			buttonCancel.Location = new System.Drawing.Point(822, 294);
			buttonCancel.Name = "buttonCancel";
			buttonCancel.Size = new System.Drawing.Size(75, 23);
			buttonCancel.TabIndex = 6;
			buttonCancel.Text = "Cancel";
			buttonCancel.UseVisualStyleBackColor = true;
			buttonCancel.Click += new System.EventHandler(buttonCancel_Click);
			buttonNew.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			buttonNew.Location = new System.Drawing.Point(13, 294);
			buttonNew.Name = "buttonNew";
			buttonNew.Size = new System.Drawing.Size(75, 23);
			buttonNew.TabIndex = 1;
			buttonNew.Text = "New";
			buttonNew.UseVisualStyleBackColor = true;
			buttonNew.Click += new System.EventHandler(buttonNew_Click);
			buttonDelete.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			buttonDelete.Location = new System.Drawing.Point(175, 294);
			buttonDelete.Name = "buttonDelete";
			buttonDelete.Size = new System.Drawing.Size(75, 23);
			buttonDelete.TabIndex = 3;
			buttonDelete.Text = "Delete";
			buttonDelete.UseVisualStyleBackColor = true;
			buttonDelete.Click += new System.EventHandler(buttonDelete_Click);
			buttonEdit.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			buttonEdit.Location = new System.Drawing.Point(94, 294);
			buttonEdit.Name = "buttonEdit";
			buttonEdit.Size = new System.Drawing.Size(75, 23);
			buttonEdit.TabIndex = 2;
			buttonEdit.Text = "Edit";
			buttonEdit.UseVisualStyleBackColor = true;
			buttonEdit.Click += new System.EventHandler(buttonEdit_Click);
			linkCreateZonesByLevel.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			linkCreateZonesByLevel.AutoSize = true;
			linkCreateZonesByLevel.Location = new System.Drawing.Point(256, 299);
			linkCreateZonesByLevel.Name = "linkCreateZonesByLevel";
			linkCreateZonesByLevel.Size = new System.Drawing.Size(124, 13);
			linkCreateZonesByLevel.TabIndex = 4;
			linkCreateZonesByLevel.TabStop = true;
			linkCreateZonesByLevel.Text = "Create Zones By Level...";
			linkCreateZonesByLevel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkCreateZonesByLevel_LinkClicked);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = buttonCancel;
			base.ClientSize = new System.Drawing.Size(909, 329);
			base.Controls.Add(linkCreateZonesByLevel);
			base.Controls.Add(buttonEdit);
			base.Controls.Add(buttonDelete);
			base.Controls.Add(buttonNew);
			base.Controls.Add(buttonCancel);
			base.Controls.Add(buttonOK);
			base.Controls.Add(listView);
			base.KeyPreview = true;
			MinimumSize = new System.Drawing.Size(600, 200);
			base.Name = "EditZonesForm";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = "Edit Project Zones";
			base.KeyDown += new System.Windows.Forms.KeyEventHandler(EditZonesForm_KeyDown);
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
