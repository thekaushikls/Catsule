using System.Collections.Generic;
using Rhino;
using Rhino.Commands;

namespace Catsule
{
    [CommandStyle(Style.ScriptRunner)]
    public class GhReloadCommand : Command
    {
        public override string EnglishName => "GhReload";

        public GhReloadCommand() => Instance = this;

        ///<summary>The only instance of the GhReload command.</summary>
        public static GhReloadCommand Instance { get; private set; }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            List<string> openFiles = DocumentHandler.UnloadGrasshopper();
            if (openFiles.Count > 0)
            {
                openFiles.Reverse(); // Preserve file order
                DocumentHandler.OpenGrasshopperFiles(openFiles);
                return Result.Success;
            }
            return Result.Failure;
        }
    }
}