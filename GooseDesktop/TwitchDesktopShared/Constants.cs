namespace TwitchDesktopShared
{
    internal static class Constants
    {
        public const int currentVersion = 0;

        public const string MMQName_Twitch = "TwitchMessages";

        public const int MMQ_TwitchSize = 4096;

        public enum GooseTask
        {
            Wander,
            NabMouse,
            CollectWindow_Meme,
            CollectWindow_Notepad,
            CollectWindow_Donate,
            CollectWindow_DONOTSET,
            TrackMud,
            Count
        }

        public struct TaskTwitchInfo
        {
            public TaskTwitchInfo(int _dbIndex, string taskID, string taskName, string taskDescription, string twitchCommand)
            {
                this.code = _dbIndex;
                this.ID = taskID;
                this.name = taskName;
                this.description = taskDescription;
                this.twitchCommandArg = twitchCommand;
            }

            public int code;

            public string ID;

            public string name;

            public string description;

            public string twitchCommandArg;
        }

        public static class GooseTaskDatabase
        {
            public static int GetTaskCode(int ledgerIndex)
            {
                return GooseTaskDatabase.twitchTaskDB[ledgerIndex].code;
            }

            public static string GetTaskName(int ledgerIndex)
            {
                return GooseTaskDatabase.twitchTaskDB[ledgerIndex].name;
            }

            public static string GetTaskTwitchCommand(int ledgerIndex)
            {
                return GooseTaskDatabase.twitchTaskDB[ledgerIndex].twitchCommandArg;
            }

            public static int GetLoadedTasksNumber()
            {
                return GooseTaskDatabase.twitchTaskDB.Length;
            }

            private static TaskTwitchInfo[] twitchTaskDB;
        }
    }
}
