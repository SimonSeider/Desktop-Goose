using GooseShared;
using SamEngine;

namespace GooseDesktop.Refactor.GooseTasks.Tasks
{
    internal class TrackMud : GooseTaskInfo
    {
        public const string TaskID = "TrackMud";


        public TrackMud()
        {
            this.canBePickedRandomly = true;
            this.shortName = "Tracking mud";
            this.description = "The goose runs off the screen, and runs back on leaving MUDDY FOOTPRINTS!";
            this.taskID = "TrackMud";
        }

        public override GooseTaskData GetNewTaskData(GooseEntity goose)
        {
            return new TrackMudTaskData();
        }

        public override void RunTask(GooseEntity g)
        {
            var desktopBounds = Program.GetDesktopBounds();
            TrackMudTaskData trackMudTaskData = (TrackMudTaskData)g.currentTaskData;
            switch (trackMudTaskData.stage)
            {
                case TrackMudTaskData.Stage.DecideToRun:
                    GooseFunctions.SetTargetOffscreen(g, false);
                    GooseFunctions.SetSpeed(g, GooseEntity.SpeedTiers.Run);
                    trackMudTaskData.stage = TrackMudTaskData.Stage.RunningOffscreen;
                    return;
                case TrackMudTaskData.Stage.RunningOffscreen:
                    if (Vector2.Distance(g.position, g.targetPos) < 5f)
                    {
                        g.targetPos = new Vector2(SamMath.RandomRange((float)desktopBounds.Left, (float)desktopBounds.Right), SamMath.RandomRange((float)desktopBounds.Top, (float)desktopBounds.Bottom));
                        trackMudTaskData.nextDirChangeTime = Time.time + TrackMudTaskData.GetDirChangeInterval();
                        trackMudTaskData.timeToStopRunning = Time.time + 2f;
                        g.trackMudEndTime = Time.time + g.parameters.DurationToTrackMud;
                        trackMudTaskData.stage = TrackMudTaskData.Stage.RunningWandering;
                        Sound.PlayMudSquith();
                        return;
                    }
                    break;
                case TrackMudTaskData.Stage.RunningWandering:
                    if (Vector2.Distance(g.position, g.targetPos) < 5f || Time.time > trackMudTaskData.nextDirChangeTime)
                    {
                        g.targetPos = new Vector2(SamMath.RandomRange((float)desktopBounds.Left, (float)desktopBounds.Right), SamMath.RandomRange((float)desktopBounds.Top, (float)desktopBounds.Bottom));
                        trackMudTaskData.nextDirChangeTime = Time.time + TrackMudTaskData.GetDirChangeInterval();
                    }
                    if (Time.time > trackMudTaskData.timeToStopRunning)
                    {
                        g.targetPos = g.position + new Vector2(30f, 3f);
                        g.targetPos.x = SamMath.Clamp(g.targetPos.x, (float)(desktopBounds.Left + 55), (float)(desktopBounds.Right - 55));
                        g.targetPos.y = SamMath.Clamp(g.targetPos.y, (float)(desktopBounds.Top + 80), (float)(desktopBounds.Bottom - 80));
                        GooseFunctions.SetTaskByID(g, "Wander", false);
                    }
                    break;
                default:
                    return;
            }
        }

        public class TrackMudTaskData : GooseTaskData
        {
            public static float GetDirChangeInterval()
            {
                return 100f;
            }

            public const float DurationToRunAmok = 2f;

            public float nextDirChangeTime;

            public float timeToStopRunning;

            public TrackMudTaskData.Stage stage;

            public enum Stage
            {
                DecideToRun,
                RunningOffscreen,
                RunningWandering
            }
        }
    }
}
