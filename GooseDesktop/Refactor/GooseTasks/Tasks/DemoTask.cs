using GooseShared;

namespace GooseDesktop.Refactor.GooseTasks.Tasks
{
    internal class DemoTask : GooseTaskInfo
    {
        public const string TaskID = "DemoTask";

        private const float ThisFloatDefaultValue = 4.53f;


        public DemoTask()
        {
            this.canBePickedRandomly = false;
            this.shortName = "Demo task - don't use?";
            this.description = "This is a demo task that does nothing useful!";
            this.taskID = "DemoTask";
        }

        public override GooseTaskData GetNewTaskData(GooseEntity goose)
        {
            return new DemoTaskData
            {
                thisIsAFloat = 4.53f,
                gooseXPositionOnTaskStart = goose.position.x
            };
        }

        public override void RunTask(GooseEntity goose)
        {
            ((DemoTaskData)goose.currentTaskData).thisIsAFloat += 1f;
        }

        public class DemoTaskData : GooseTaskData
        {
            public float thisIsAFloat;

            public float gooseXPositionOnTaskStart;
        }
    }
}
