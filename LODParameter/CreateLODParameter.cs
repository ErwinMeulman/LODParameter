using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;

namespace LODParameter
{
	[Transaction(TransactionMode.Manual)]
	[Regeneration(RegenerationOption.Manual)]
	[Journaling(JournalingMode.NoCommandData)]
	public class CreateLODParameter
	{
		public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
		{
			Application val = commandData.get_Application().get_Application();
			Document val2 = commandData.get_Application().get_ActiveUIDocument().get_Document();
			Definition val3 = LODapp.GetParameterDefinition(val2, "Current_LOD");
			Definition val4 = LODapp.GetParameterDefinition(val2, "Target_LOD");
			Definition val5 = LODapp.GetParameterDefinition(val2, "MEA");
			Definition val6 = LODapp.GetParameterDefinition(val2, "Zone");
			Transaction val7 = new Transaction(val2, "Create LOD Parameter");
			try
			{
				val7.Start();
				string tempPath = Path.GetTempPath();
				tempPath += "LODmanager\\";
				Directory.CreateDirectory(tempPath);
				string text = tempPath + "RevitParameters.txt";
				FileStream fileStream = File.Create(text);
				fileStream.Close();
				val.set_SharedParametersFilename(text);
				DefinitionFile val8 = val.OpenSharedParameterFile();
				DefinitionGroup val9 = val8.get_Groups().Create("BIM");
				if (val3 == null)
				{
					ExternalDefinitionCreationOptions val10 = new ExternalDefinitionCreationOptions("Current_LOD", 2);
					val10.set_Description("The Current Level of Development of the Object. \nOptions: 100, 200, 300, 350, 400. \nSee LOD Specification for details.");
					val3 = val9.get_Definitions().Create(val10);
				}
				if (val4 == null)
				{
					ExternalDefinitionCreationOptions val11 = new ExternalDefinitionCreationOptions("Target_LOD", 2);
					val11.set_Description("The Target Level of Development of the Object. \nOptions: 100, 200, 300, 350, 400. \nSee LOD Specification for details.");
					val4 = val9.get_Definitions().Create(val11);
				}
				if (val5 == null)
				{
					ExternalDefinitionCreationOptions val12 = new ExternalDefinitionCreationOptions("MEA", 1);
					val12.set_Description("The Model Element Author for the Object.");
					val5 = val9.get_Definitions().Create(val12);
				}
				if (val6 == null)
				{
					ExternalDefinitionCreationOptions val13 = new ExternalDefinitionCreationOptions("Zone", 1);
					val13.set_Description("The Zone in which this Object resides.");
					val6 = val9.get_Definitions().Create(val13);
				}
				CategorySet val14 = val.get_Create().NewCategorySet();
				BuiltInCategory[] lodCatArray = LODapp.lodCatArray;
				foreach (BuiltInCategory val15 in lodCatArray)
				{
					Category val16 = val2.get_Settings().get_Categories().get_Item(val15);
					val14.Insert(val16);
				}
				InstanceBinding val17 = val.get_Create().NewInstanceBinding(val14);
				val2.get_ParameterBindings().Insert(val3, val17, -5000175);
				val2.get_ParameterBindings().Insert(val4, val17, -5000175);
				val2.get_ParameterBindings().Insert(val5, val17, -5000175);
				val2.get_ParameterBindings().Insert(val6, val17, -5000175);
				val7.Commit();
			}
			catch (Exception ex)
			{
				val7.RollBack();
				message = ex.ToString();
				return -1;
			}
			val7 = new Transaction(val2, "Set Default LOD Values");
			try
			{
				val7.Start();
				BuiltInCategory[] lodCatArray2 = LODapp.lodCatArray;
				foreach (BuiltInCategory val18 in lodCatArray2)
				{
					FilteredElementCollector val19 = new FilteredElementCollector(val2);
					val19.OfCategory(val18);
					IEnumerable<Element> elems = val19.ToElements();
					LODapp.SetParameterOfElementsIfNotSet(elems, val3, 200);
				}
				val7.Commit();
			}
			catch (Exception ex2)
			{
				val7.RollBack();
				message = ex2.ToString();
				return -1;
			}
			return 0;
		}
	}
}
