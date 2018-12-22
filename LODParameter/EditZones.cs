using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.Exceptions;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace LODParameter
{
	[Transaction(TransactionMode.Manual)]
	[Regeneration(RegenerationOption.Manual)]
	internal class EditZones
	{
		private EditZonesForm editZonesForm;

		public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
		{
			UIApplication val = commandData.get_Application();
			Document val2 = val.get_ActiveUIDocument().get_Document();
			Selection val3 = val.get_ActiveUIDocument().get_Selection();
			IList<ZoneData> projectZonesAsZoneData = ZoneData.GetProjectZonesAsZoneData(val2);
			IEnumerable<Element> enumerable = new FilteredElementCollector(val2).WhereElementIsNotElementType().OfClass(typeof(Level)).ToElements();
			SortedList<double, Level> sortedList = new SortedList<double, Level>(enumerable.Count());
			foreach (Level item in enumerable)
			{
				Level val4 = item;
				sortedList.Add(val4.get_Elevation(), val4);
			}
			IList<Element> list = new FilteredElementCollector(val2).WhereElementIsNotElementType().OfClass(typeof(Grid)).ToElements();
			SortedList<double, Grid> sortedList2 = new SortedList<double, Grid>();
			SortedList<double, Grid> sortedList3 = new SortedList<double, Grid>();
			foreach (Grid item2 in list)
			{
				Grid val5 = item2;
				if (isNorthSouth(val5))
				{
					sortedList2.Add(val5.get_Curve().GetEndPoint(0).get_X(), val5);
				}
				else if (isEastWest(val5))
				{
					sortedList3.Add(val5.get_Curve().GetEndPoint(0).get_Y(), val5);
				}
			}
			editZonesForm = new EditZonesForm(projectZonesAsZoneData, sortedList, sortedList2, sortedList3, val2.GetUnits());
			editZonesForm.ShowDialog();
			if (editZonesForm.DialogResult == DialogResult.OK)
			{
				IList<ZoneData> editedZones = editZonesForm.EditedZones;
				Transaction val6 = new Transaction(val2, "Update Project Zones");
				try
				{
					val6.Start();
					ZoneData.UpdateRevitProjectZones(val2, editedZones);
					val6.Commit();
				}
				catch (OperationCanceledException)
				{
					return 1;
				}
				catch (Exception ex)
				{
					message = ex.Message;
					return -1;
				}
				return 0;
			}
			return 1;
		}

		protected static bool isNorthSouth(Grid grid)
		{
			if (grid.get_Curve() is Line)
			{
				Line val = grid.get_Curve() as Line;
				return val.get_Direction().get_Y() >= 0.99 || val.get_Direction().get_Y() <= -0.99;
			}
			return false;
		}

		protected static bool isEastWest(Grid grid)
		{
			if (grid.get_Curve() is Line)
			{
				Line val = grid.get_Curve() as Line;
				return val.get_Direction().get_X() >= 0.99 || val.get_Direction().get_X() <= -0.99;
			}
			return false;
		}
	}
}
