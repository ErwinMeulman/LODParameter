using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LODParameter
{
	public class ZoneParameterUpdater
	{
		private static UpdaterId m_updaterId;

		public ZoneParameterUpdater(AddInId id)
		{
			m_updaterId = new UpdaterId(id, new Guid("FBFBF6B2-4C09-42e7-97C1-D1B4EB593EFF"));
		}

		public void Execute(UpdaterData data)
		{
			Document doc = data.GetDocument();
			List<ElementId> list = data.GetAddedElementIds().ToList();
			list.AddRange(data.GetModifiedElementIds());
			IList<Element> first = (from ElementId id in list
			select doc.GetElement(id)).ToList();
			IList<FamilyInstance> projectZones = ZoneData.GetProjectZones(doc);
			IList<ElementId> list2 = (from Element pZ in projectZones
			select pZ.get_Id()).ToList();
			Parameter val = (from FamilyInstance pZ in projectZones
			let param = pZ.LookupParameter("Name")
			where param != null
			select param).First();
			Definition zoneNameDef = val.get_Definition();
			foreach (Element item in first.Except((IEnumerable<Element>)projectZones))
			{
				BoundingBoxXYZ val2 = item.get_BoundingBox(null);
				if (val2 != null)
				{
					BoundingBoxIntersectsFilter val3 = new BoundingBoxIntersectsFilter(ToOutline(val2));
					IList<Element> source = new FilteredElementCollector(doc, (ICollection<ElementId>)list2).WherePasses(val3).ToElements();
					IEnumerable<string> values = from Element zone in source
					let name = zone.get_Parameter(zoneNameDef).AsString()
					where !string.IsNullOrWhiteSpace(name)
					select name;
					string text = string.Join(", ", values);
					item.LookupParameter("Zone").Set(text);
				}
			}
		}

		public string GetAdditionalInformation()
		{
			return "Zone Parameter Updater: updates zone parameter of elements based on location";
		}

		public ChangePriority GetChangePriority()
		{
			return 16;
		}

		public UpdaterId GetUpdaterId()
		{
			return m_updaterId;
		}

		public string GetUpdaterName()
		{
			return "ZoneParameterUpdater";
		}

		private static Outline ToOutline(BoundingBoxXYZ bb)
		{
			return new Outline(bb.get_Min(), bb.get_Max());
		}
	}
}
