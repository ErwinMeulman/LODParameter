using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.Exceptions;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LODParameter
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class ToggleZoneVisibility
	{
		public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
		{
			UIApplication val = commandData.get_Application();
			Document doc = val.get_ActiveUIDocument().get_Document();
			IList<FamilyInstance> projectZones = ZoneData.GetProjectZones(doc);
			IList<ElementId> list = (from z in (IEnumerable<FamilyInstance>)projectZones
			select z.get_Id()).ToList();
			bool flag = projectZones.All((FamilyInstance z) => z.IsHidden(doc.get_ActiveView()));
			Transaction val2 = new Transaction(doc, flag ? "Unhide Zones" : "Hide Zones");
			try
			{
				val2.Start();
				if (flag)
				{
					doc.get_ActiveView().UnhideElements((ICollection<ElementId>)list);
				}
				else
				{
					doc.get_ActiveView().HideElements((ICollection<ElementId>)list);
				}
				val2.Commit();
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
