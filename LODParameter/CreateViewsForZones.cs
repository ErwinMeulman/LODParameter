using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.Exceptions;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace LODParameter
{
	[Transaction(TransactionMode.Manual)]
	[Regeneration(RegenerationOption.Manual)]
	internal class CreateViewsForZones
	{
		protected readonly double[] VIEW_CROP_OFFSETS = new double[6]
		{
			2.0,
			0.0,
			2.0,
			2.0,
			2.0,
			2.0
		};

		public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
		{
			UIApplication val = commandData.get_Application();
			Document val2 = val.get_ActiveUIDocument().get_Document();
			IList<View3D> views = new FilteredElementCollector(val2).WhereElementIsNotElementType().OfClass(typeof(View3D)).ToElements()
				.Cast<View3D>()
				.ToList();
			ViewsForZonesForm viewsForZonesForm = new ViewsForZonesForm(views, val2.get_ActiveView() as View3D);
			viewsForZonesForm.ShowDialog();
			if (viewsForZonesForm.DialogResult != DialogResult.OK)
			{
				return 1;
			}
			View3D selectedView = viewsForZonesForm.SelectedView;
			XYZ val3 = new XYZ(0.0 - VIEW_CROP_OFFSETS[5], 0.0 - VIEW_CROP_OFFSETS[3], 0.0 - VIEW_CROP_OFFSETS[1]);
			XYZ val4 = new XYZ(VIEW_CROP_OFFSETS[4], VIEW_CROP_OFFSETS[2], VIEW_CROP_OFFSETS[0]);
			IList<FamilyInstance> projectZones = ZoneData.GetProjectZones(val2);
			Transaction val5 = new Transaction(val2, "Create Views for Project Zones");
			try
			{
				val5.Start();
				foreach (FamilyInstance item in projectZones)
				{
					ElementId val6 = selectedView.Duplicate(0);
					View3D val7 = val2.GetElement(val6) as View3D;
					BoundingBoxXYZ val8 = item.get_BoundingBox(null);
					BoundingBoxXYZ val9 = new BoundingBoxXYZ();
					val9.set_Min(val8.get_Min() + val3);
					val9.set_Max(val8.get_Max() + val4);
					val7.set_Name("3D " + item.LookupParameter("Name").AsString());
					val7.SetSectionBox(val9);
				}
				val5.Commit();
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
	}
}
