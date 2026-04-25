using System.Threading;
using System.Windows.Forms;
using GooseDesktop.Refactor.CustomFormTypes;
using GooseShared;
using SamEngine;

namespace GooseDesktop.Refactor.GooseTasks.Tasks
{
    internal class RunCollectWindow : GooseTaskInfo
    {
        public const string TaskID = "";

        private const float ThisFloatDefaultValue = 4.53f;


        public RunCollectWindow()
        {
            this.canBePickedRandomly = false;
            this.shortName = "Internal task. Don't use.";
            this.description = "This is an internal 'task' that handles all cases of collecting windows. Cannot be called on its own.";
            this.taskID = "";
        }

        public override GooseTaskData GetNewTaskData(GooseEntity goose)
        {
            MessageBox.Show("ERROR: BASE WINDOW TASK DATA GETTING CALLED WITHOUT ANY OTHER CONTEXT.");
            return null;
        }

        public void SetupScreenTargetAndBeakOffset(CollectWindowTaskData data, GooseEntity goose)
        {
            data.screenDirection = GooseFunctions.SetTargetOffscreen(goose, false);
            switch (data.screenDirection)
            {
                case ScreenDirection.Left:
                    data.windowOffsetToBeak = new Vector2((float)data.mainForm.Width, (float)(data.mainForm.Height / 2));
                    return;
                case ScreenDirection.Top:
                    data.windowOffsetToBeak = new Vector2((float)(data.mainForm.Width / 2), (float)data.mainForm.Height);
                    return;
                case ScreenDirection.Right:
                    data.windowOffsetToBeak = new Vector2(0f, (float)(data.mainForm.Height / 2));
                    return;
                default:
                    return;
            }
        }

        public override void RunTask(GooseEntity goose)
        {
            if (goose.currentTaskData == null)
            {
                MessageBox.Show("Cannot run CollectWindow task without specifying a window type.", "CollectWindow Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                GooseFunctions.SetTaskDefault(goose);
                return;
            }
            CollectWindowTaskData taskData = (CollectWindowTaskData)goose.currentTaskData;
            switch (taskData.stage)
            {
                case CollectWindowTaskData.Stage.WalkingOffscreen:
                    if (Vector2.Distance(goose.position, goose.targetPos) < 5f)
                    {
                        taskData.secsToWait = CollectWindowTaskData.GetWaitTime();
                        taskData.waitStartTime = Time.time;
                        taskData.stage = CollectWindowTaskData.Stage.WaitingToBringWindowBack;
                        return;
                    }
                    break;
                case CollectWindowTaskData.Stage.WaitingToBringWindowBack:
                    if (Time.time - taskData.waitStartTime > taskData.secsToWait)
                    {
                        var desktopBounds = Program.GetDesktopBounds();
                        taskData.mainForm.FormClosing += OnGiftClosed;
                        Thread dialogThread = new Thread(new ThreadStart(() =>
                        {
                            taskData.mainForm.ShowDialog();
                        }));
                        dialogThread.SetApartmentState(ApartmentState.STA);
                        dialogThread.Start();
                        switch (taskData.screenDirection)
                        {
                            case ScreenDirection.Left:
                                goose.targetPos.y = SamMath.Lerp(goose.position.y, (float)(desktopBounds.Top + desktopBounds.Height / 2), SamMath.RandomRange(0.2f, 0.3f));
                                goose.targetPos.x = (float)(desktopBounds.Left + taskData.mainForm.Width) + SamMath.RandomRange(15f, 20f);
                                break;
                            case ScreenDirection.Top:
                                goose.targetPos.y = (float)(desktopBounds.Top + taskData.mainForm.Height) + SamMath.RandomRange(80f, 100f);
                                goose.targetPos.x = SamMath.Lerp(goose.position.x, (float)(desktopBounds.Left + desktopBounds.Width / 2), SamMath.RandomRange(0.2f, 0.3f));
                                break;
                            case ScreenDirection.Right:
                                goose.targetPos.y = SamMath.Lerp(goose.position.y, (float)(desktopBounds.Top + desktopBounds.Height / 2), SamMath.RandomRange(0.2f, 0.3f));
                                goose.targetPos.x = (float)desktopBounds.Right - ((float)taskData.mainForm.Width + SamMath.RandomRange(20f, 30f));
                                break;
                        }
                        goose.targetPos.x = SamMath.Clamp(goose.targetPos.x, (float)(desktopBounds.Left + taskData.mainForm.Width + 55), (float)(desktopBounds.Right - (taskData.mainForm.Width + 55)));
                        goose.targetPos.y = SamMath.Clamp(goose.targetPos.y, (float)(desktopBounds.Top + taskData.mainForm.Height + 80), (float)desktopBounds.Bottom);
                        taskData.stage = CollectWindowTaskData.Stage.DraggingWindowBack;
                        return;
                    }
                    break;
                case CollectWindowTaskData.Stage.DraggingWindowBack:
                    if (Vector2.Distance(goose.position, goose.targetPos) < 5f)
                    {
                        goose.targetPos = goose.position + Vector2.GetFromAngleDegrees(goose.direction + 180f) * 40f;
                        GooseFunctions.SetTaskByID(goose, "Wander", true);
                        return;
                    }
                    goose.extendingNeck = true;
                    goose.targetDirection = goose.targetPos - goose.position;
                    taskData.mainForm.SetWindowPositionThreadsafe(RenderFuncs.ToIntPoint(goose.rig.head2EndPoint - taskData.windowOffsetToBeak));
                    break;
                default:
                    return;
            }
        }

        public static void OnGiftClosed(object sender, FormClosingEventArgs args)
        {
            GooseEntity ownerGoose = ((MovableForm)sender).ownerGoose;
            if (GooseConfig.settings.Task_CanAttackMouse)
            {
                GooseFunctions.SetTaskByID(ownerGoose, "NabMouse", true);
                return;
            }
            if (ownerGoose.currentTaskData is CollectWindowTaskData && ((CollectWindowTaskData)ownerGoose.currentTaskData).mainForm == (MovableForm)sender)
            {
                GooseFunctions.SetTaskByID(ownerGoose, "Wander", true);
            }
        }

        public class CollectWindowTaskData : GooseTaskData
        {
            public static float GetWaitTime()
            {
                return SamMath.RandomRange(2f, 3.5f);
            }

            public MovableForm mainForm;

            public CollectWindowTaskData.Stage stage;

            public float secsToWait;

            public float waitStartTime;

            public ScreenDirection screenDirection;

            public Vector2 windowOffsetToBeak;

            public enum Stage
            {
                WalkingOffscreen,
                WaitingToBringWindowBack,
                DraggingWindowBack
            }
        }
    }
}
