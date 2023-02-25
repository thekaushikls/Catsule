using Rhino;
using Rhino.Commands;
using Rhino.Input.Custom;

namespace Catsule
{
    public class GhSettingsCommand : Command
    {
        public override string EnglishName => "GhSettings";

        public GhSettingsCommand() => Instance = this;

        ///<summary>The only instance of the GhSettings command.</summary>
        public static GhSettingsCommand Instance { get; private set; }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            OptionToggle isActiveOption = new OptionToggle(Catsule.Instance.IsActive, "Disabled", "Active");

            GetOption getOption = new GetOption();
            getOption.AddOptionToggle("Status", ref isActiveOption);
            getOption.SetCommandPrompt("Catsule Settings");
            Rhino.Input.GetResult result = getOption.Get();

            if (result == Rhino.Input.GetResult.Cancel)
                return Result.Cancel;
            else if (getOption.CommandResult() == Result.Success)
            {
                // `result` will always be of type => `Option`
                Catsule.Instance.IsActive = isActiveOption.CurrentValue;
                string status = isActiveOption.CurrentValue ? "Enabled" : "Disabled";
                RhinoApp.WriteLine($"Autoload {status}");
                return Result.Success;
            }
            else
                return Result.Failure;
        }
    }
}