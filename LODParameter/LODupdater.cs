using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LODParameter
{
	public class LODupdater
	{
		private static UpdaterId m_updaterId;

		public LODupdater(AddInId id)
		{
			m_updaterId = new UpdaterId(id, new Guid("FBFBF6B2-4C06-42e7-97C1-D1B4EB593EFF"));
		}

		public void Execute(UpdaterData data)
		{
			Document doc = data.GetDocument();
			if (!doc.get_IsFamilyDocument())
			{
				Definition parameterDefinition = LODapp.GetParameterDefinition(doc, "Current_LOD");
				if (parameterDefinition != null)
				{
					ICollection<ElementId> addedElementIds = data.GetAddedElementIds();
					IList<Element> elems = (from ElementId id in addedElementIds
					select doc.GetElement(id)).ToList();
					LODapp.SetParameterOfElementsIfNotSet((IEnumerable<Element>)elems, parameterDefinition, 200);
				}
			}
		}

		public UpdaterId GetUpdaterId()
		{
			return m_updaterId;
		}

		public ChangePriority GetChangePriority()
		{
			return 16;
		}

		public string GetUpdaterName()
		{
			return "LOD Parameter Updater";
		}

		public string GetAdditionalInformation()
		{
			return "LOD Parameter updater: sets the default CurrentLOD value for all newly added Elements";
		}
	}
}
