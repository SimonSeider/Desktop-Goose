namespace GooseShared
{
    public static class API
    {
        public static GooseFunctionPointers Goose;

        public static ModHelperFunctions Helper;

        public static TaskDatabaseQueryFunctions TaskDatabase;

        public class GooseFunctionPointers
        {
            public SetSpeedFunction setSpeed;

            public SetTargetOffscreenFunction setTargetOffscreen;

            public IsGooseAtTargetFunction isGooseAtTarget;

            public GetDistanceToTargetFunction getDistanceToTarget;

            public SetCurrentTaskByIDFunction setCurrentTaskByID;

            public ChooseRandomTaskFunction chooseRandomTask;

            public SetTaskToRoaming setTaskRoaming;

            public PlayHonckSoundFunction playHonckSound;

            public delegate void SetSpeedFunction(GooseEntity g, GooseEntity.SpeedTiers tier);

            public delegate ScreenDirection SetTargetOffscreenFunction(GooseEntity g, bool canExitTop = false);

            public delegate bool IsGooseAtTargetFunction(GooseEntity g, float distanceToTrigger);

            public delegate float GetDistanceToTargetFunction(GooseEntity g);

            public delegate void SetCurrentTaskByIDFunction(GooseEntity g, string id, bool honk = true);

            public delegate void ChooseRandomTaskFunction(GooseEntity g);

            public delegate void SetTaskToRoaming(GooseEntity g);

            public delegate void PlayHonckSoundFunction();
        }

        public class ModHelperFunctions
        {
            public GetModDirectoryFunction getModDirectory;

            public delegate string GetModDirectoryFunction(IMod mod);
        }

        public class TaskDatabaseQueryFunctions
        {
            public GetTaskIndexByIDFunction getTaskIndexByID;

            public GetAllLoadedTaskIDsFunction getAllLoadedTaskIDs;

            public GetNextRandomTaskFunction getRandomTaskID;

            public delegate int GetTaskIndexByIDFunction(string id);

            public delegate string[] GetAllLoadedTaskIDsFunction();

            public delegate string GetNextRandomTaskFunction();
        }
    }
}
