using System.Collections.Generic;
using Rhino.UI;
using Rhino.Commands;

namespace Catsule
{
    [CommandStyle(Style.ScriptRunner)]
    public class DocumentHandler
    {
        public static void OnRhinoDocumentOpen(object sender, Rhino.DocumentOpenEventArgs args)
        {
            if (Catsule.Instance.IsActive)
            {
                string
                    rhinoFile = args.FileName,
                    grasshopperFile = rhinoFile.Replace(".3dm", ".gh");

                if (System.IO.File.Exists(grasshopperFile))
                    DocumentHandler.OpenGrasshopperFiles(new List<string>() { grasshopperFile });
                else
                    Rhino.RhinoApp.WriteLine($"Catsule: ❌ {System.IO.Path.GetFileName(grasshopperFile)} file was not found.");
            }
        }

        public static void OpenGrasshopperFiles(IEnumerable<string> filePaths)
        {
            foreach (string filepath in filePaths)
                Grasshopper.Plugin.Commands.Run_GrasshopperOpen(filepath);
        }

        public static List<string> UnloadGrasshopper()
        {
            bool flag = true;
            List<string> openFiles = new List<string>();

            foreach (Grasshopper.Kernel.GH_Document ghDoc in Grasshopper.Instances.DocumentServer)
            {
                openFiles.Add(ghDoc.FilePath);
                if (ghDoc.DisplayName.EndsWith("*"))
                    flag = false;
            }

            if (flag == false)
            {
                ShowMessageResult result = Dialogs.ShowMessage("Some files are not saved. Do you want to Proceed?", "Unsaved Files", ShowMessageButton.YesNo, ShowMessageIcon.Warning);

                if (result == ShowMessageResult.Yes)
                    flag = true;
            }

            if (flag == true)
            {
                Grasshopper.Instances.ReloadMemoryAssemblies();
                Grasshopper.Plugin.GH_PluginUtil.UnloadGrasshopper();
            }

            return openFiles;
        }
    }
}
