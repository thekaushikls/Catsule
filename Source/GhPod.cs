using Rhino;
using Rhino.PlugIns;

namespace GhPod
{
    public class GhPod : PlugIn
    {
        public GhPod() => Instance = this;

        public override PlugInLoadTime LoadTime => PlugInLoadTime.AtStartup;
        
        public static GhPod Instance { get; private set; }

        public bool IsActive
        {
            get => this.Settings.GetBool("IsActive", true);
            set => this.Settings.SetBool("IsActive", value);
        }

        protected override LoadReturnCode OnLoad(ref string errorMessage)
        {
            RhinoDoc.EndOpenDocument += DocumentHandler.OnRhinoDocumentOpen;
            return base.OnLoad(ref errorMessage);
        }
    }
}