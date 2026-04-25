using GooseDesktop.Refactor.CustomFormTypes;
using GooseShared;

namespace GooseDesktop.Refactor.GooseTasks.Tasks
{
    internal class CollectMeme : RunCollectWindow
    {
        public new const string TaskID = "CollectMeme";

        public CollectMeme()
        {
            this.canBePickedRandomly = true;
            this.shortName = "Collect meme";
            this.description = "Make the goose run offscreen, and collect a meme.";
            this.taskID = "CollectMeme";
        }

        public override GooseTaskData GetNewTaskData(GooseEntity goose)
        {
            RunCollectWindow.CollectWindowTaskData collectWindowTaskData = new RunCollectWindow.CollectWindowTaskData();
            collectWindowTaskData.mainForm = new SimpleImageForm(goose);
            collectWindowTaskData.mainForm.FormClosing += RunCollectWindow.OnGiftClosed;
            base.SetupScreenTargetAndBeakOffset(collectWindowTaskData, goose);
            return collectWindowTaskData;
        }

    }
}
