using Rhino;
using Rhino.PlugIns;

namespace Catsule
{
    public class Catsule : PlugIn
    {
        public Catsule() => Instance = this;

        public override PlugInLoadTime LoadTime => PlugInLoadTime.AtStartup;
        
        public static Catsule Instance { get; private set; }

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