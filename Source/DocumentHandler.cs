using System.Collections.Generic;
using Rhino.UI;
using Rhino.Commands;

namespace GhPod
{
    [CommandStyle(Style.ScriptRunner)]
    public class DocumentHandler
    {
        public static void OnRhinoDocumentOpen(object sender, Rhino.DocumentOpenEventArgs args)
        {
            if (GhPod.Instance.IsActive)
            {
                Rhino.RhinoApp.WriteLine("Plugin is active");
                string
                    rhinoFile = args.FileName,
                    grasshopperFile = rhinoFile.Replace(".3dm", ".gh");

                if (System.IO.File.Exists(grasshopperFile))
                    DocumentHandler.OpenGrasshopperFiles(new List<string>() { grasshopperFile });
            }
            else
            {
                Rhino.RhinoApp.WriteLine("Plugin is not active");
            }
        }

        public static void OpenGrasshopperFiles(IEnumerable<string> filePaths)
        {
            string cmd = "_-Grasshopper Banner Disable ";
            foreach (string filepath in filePaths)
                cmd += $"Document Open {filepath} ";
            cmd += "Window Load Window Show _Enter";
            Rhino.RhinoApp.RunScript(cmd, false);
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
                Grasshopper.Instances.UnloadAllObjects();
                Grasshopper.Instances.ReloadMemoryAssemblies();
            }

            return openFiles;
        }
    }
}
