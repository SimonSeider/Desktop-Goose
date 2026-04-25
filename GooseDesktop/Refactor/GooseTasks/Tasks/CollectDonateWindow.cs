using GooseDesktop.Refactor.CustomFormTypes;
using GooseShared;

namespace GooseDesktop.Refactor.GooseTasks.Tasks
{
    internal class CollectDonateWindow : RunCollectWindow
    {
        public new const string TaskID = "CollectDonateWindow";

        public CollectDonateWindow()
        {
            this.canBePickedRandomly = false;
            this.shortName = "Collect donation window";
            this.description = "Make the goose run offscreen, and collect the donation window.";
            this.taskID = "CollectDonateWindow";
        }

        public override GooseTaskData GetNewTaskData(GooseEntity goose)
        {
            RunCollectWindow.CollectWindowTaskData collectWindowTaskData = new RunCollectWindow.CollectWindowTaskData();
            collectWindowTaskData.mainForm = new SimpleDonateForm(goose);
            base.SetupScreenTargetAndBeakOffset(collectWindowTaskData, goose);
            return collectWindowTaskData;
        }

    }
}
