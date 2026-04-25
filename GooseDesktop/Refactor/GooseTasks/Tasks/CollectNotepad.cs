using GooseDesktop.Refactor.CustomFormTypes;
using GooseShared;

namespace GooseDesktop.Refactor.GooseTasks.Tasks
{
    internal class CollectNotepad : RunCollectWindow
    {
        public new const string TaskID = "CollectNotepad";

        public CollectNotepad()
        {
            this.canBePickedRandomly = true;
            this.shortName = "Collect Not-epad window";
            this.description = "Make the goose run offscreen, and collect a \"Goose Not-epad\" document.";
            this.taskID = "CollectNotepad";
        }

        public override GooseTaskData GetNewTaskData(GooseEntity goose)
        {
            RunCollectWindow.CollectWindowTaskData collectWindowTaskData = new RunCollectWindow.CollectWindowTaskData();
            collectWindowTaskData.mainForm = new SimpleTextForm(goose);
            base.SetupScreenTargetAndBeakOffset(collectWindowTaskData, goose);
            return collectWindowTaskData;
        }

    }
}
