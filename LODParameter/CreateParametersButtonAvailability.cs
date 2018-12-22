using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace LODParameter
{
	internal class CreateParametersButtonAvailability
	{
		public bool IsCommandAvailable(UIApplication appData, CategorySet selectedCategories)
		{
			try
			{
				Document doc = appData.get_ActiveUIDocument().get_Document();
				Definition parameterDefinition = LODapp.GetParameterDefinition(doc, "Current_LOD");
				if (parameterDefinition == null)
				{
					return true;
				}
				return false;
			}
			catch
			{
				return false;
			}
		}
	}
}
