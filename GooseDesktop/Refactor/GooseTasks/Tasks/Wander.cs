using GooseShared;
using SamEngine;

namespace GooseDesktop.Refactor.GooseTasks.Tasks
{
    public class Wander : GooseTaskInfo
    {
        public const string TaskID = "Wander";

        private const float MinPauseTime = 1f;

        private const float MaxPauseTime = 2f;

        public const float GoodEnoughDistance = 20f;


        public Wander()
        {
            this.canBePickedRandomly = false;
            this.shortName = "Wandering";
            this.description = "Just the goose's wandering around, default state.";
            this.taskID = "Wander";
        }

        public override GooseTaskData GetNewTaskData(GooseEntity goose)
        {
            WanderTaskData wanderTaskData = new WanderTaskData();
            GooseFunctions.SetSpeed(goose, GooseEntity.SpeedTiers.Walk);
            wanderTaskData.pauseStartTime = -1f;
            wanderTaskData.wanderingStartTime = Time.time;
            wanderTaskData.wanderingDuration = GetRandomWanderDuration();
            return wanderTaskData;
        }

        public override void RunTask(GooseEntity goose)
        {
            WanderTaskData wanderTaskData = (WanderTaskData)goose.currentTaskData;
            if (!IPCManager.ConnectedToTwitch && Time.time - wanderTaskData.wanderingStartTime > wanderTaskData.wanderingDuration)
            {
                GooseFunctions.ChooseRandomTask(goose);
                return;
            }
            if (wanderTaskData.pauseStartTime <= 0f)
            {
                if (Vector2.Distance(goose.position, goose.targetPos) < 20f)
                {
                    wanderTaskData.pauseStartTime = Time.time;
                    wanderTaskData.pauseDuration = GetRandomPauseDuration();
                }
                return;
            }
            if (Time.time - wanderTaskData.pauseStartTime > wanderTaskData.pauseDuration)
            {
                var desktopBounds = Program.GetDesktopBounds();
                wanderTaskData.pauseStartTime = -1f;
                float num = GetRandomWalkTime() * goose.currentSpeed;
                goose.targetPos = new Vector2(SamMath.RandomRange((float)desktopBounds.Left, (float)desktopBounds.Right), SamMath.RandomRange((float)desktopBounds.Top, (float)desktopBounds.Bottom));
                if (Vector2.Distance(goose.position, goose.targetPos) > num)
                {
                    goose.targetPos = goose.position + Vector2.Normalize(goose.targetPos - goose.position) * num;
                }
                return;
            }
            goose.velocity = Vector2.zero;
        }

        private static float GetRandomPauseDuration()
        {
            return 1f + (float)SamMath.Rand.NextDouble() * 1f;
        }

        private static float GetRandomWanderDuration()
        {
            if (Time.time < 1f)
            {
                return GooseConfig.settings.FirstWanderTimeSeconds;
            }
            return SamMath.RandomRange(GooseConfig.settings.MinWanderingTimeSeconds, GooseConfig.settings.MaxWanderingTimeSeconds);
        }

        private static float GetRandomWalkTime()
        {
            return SamMath.RandomRange(1f, 6f);
        }

        public class WanderTaskData : GooseTaskData
        {
            public float wanderingStartTime;

            public float wanderingDuration;

            public float pauseStartTime;

            public float pauseDuration;
        }
    }
}
