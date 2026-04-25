using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using GooseDesktop.Refactor.GooseTasks;
using GooseShared;

namespace GooseDesktop.Refactor
{
    internal static class ModSupport
    {
        private static List<IMod> modEntryPoints = new List<IMod>();

        private static Dictionary<IMod, string> modPaths = new Dictionary<IMod, string>();

        public static void LoadMods()
        {
            Type modEntryType = typeof(IMod);
            Type taskType = typeof(GooseTaskInfo);
            string pathToFileInAssembly = Program.GetPathToFileInAssembly("Assets/Mods/");
            if (!Directory.Exists(pathToFileInAssembly))
            {
                MessageBox.Show("Error: Could not find 'Assets/Mods' directory. Continuing without loading mods.", "Could not find mods folder", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string[] directories = Directory.GetDirectories(pathToFileInAssembly);
            for (int i = 0; i < directories.Length; i++)
            {
                foreach (string text in Directory.GetFiles(directories[i]))
                {
                    if (text.EndsWith(".dll"))
                    {
                        Assembly assembly = Assembly.UnsafeLoadFrom(Path.GetFullPath(text));
                        for (; ; )
                        {
                            try
                            {
                                Type[] array = assembly.GetTypes()
                                    .Where((Type p) => modEntryType.IsAssignableFrom(p) && p.IsClass)
                                    .ToArray();
                                for (int k = 0; k < array.Length; k++)
                                {
                                    IMod mod = (IMod)Activator.CreateInstance(array[k]);
                                    modEntryPoints.Add(mod);
                                    modPaths.Add(mod, directories[i]);
                                }
                                array = assembly.GetTypes()
                                    .Where((Type p) => p.IsSubclassOf(taskType) && p.IsClass)
                                    .ToArray();
                                for (int k = 0; k < array.Length; k++)
                                {
                                    GooseTaskDatabase.RegisterTask((GooseTaskInfo)Activator.CreateInstance(array[k]));
                                }
                            }
                            catch
                            {
                                string text2 = Path.GetFileName(Path.GetDirectoryName(text)) + "/" + Path.GetFileName(text);
                                DialogResult dialogResult = MessageBox.Show("Could not load mod \"" + text2 + "\"\n\nIt may be for a different version of the goose.", "Couldn't Load Mod", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Hand);
                                if (dialogResult == DialogResult.Retry)
                                {
                                    continue;
                                }
                                if (dialogResult == DialogResult.Abort)
                                {
                                    Environment.Exit(1);
                                }
                            }
                            break;
                        }
                    }
                }
            }
            API.GooseFunctionPointers gooseFunctionPointers = new API.GooseFunctionPointers();
            gooseFunctionPointers.setSpeed = new API.GooseFunctionPointers.SetSpeedFunction(GooseFunctions.SetSpeed);
            gooseFunctionPointers.setTargetOffscreen = new API.GooseFunctionPointers.SetTargetOffscreenFunction(GooseFunctions.SetTargetOffscreen);
            gooseFunctionPointers.setCurrentTaskByID = new API.GooseFunctionPointers.SetCurrentTaskByIDFunction(GooseFunctions.SetTaskByID);
            gooseFunctionPointers.chooseRandomTask = new API.GooseFunctionPointers.ChooseRandomTaskFunction(GooseFunctions.ChooseRandomTask);
            gooseFunctionPointers.setTaskRoaming = new API.GooseFunctionPointers.SetTaskToRoaming(GooseFunctions.SetTaskDefault);
            gooseFunctionPointers.isGooseAtTarget = new API.GooseFunctionPointers.IsGooseAtTargetFunction(GooseFunctions.IsGooseAtTarget);
            gooseFunctionPointers.getDistanceToTarget = new API.GooseFunctionPointers.GetDistanceToTargetFunction(GooseFunctions.GetDistanceToTarget);
            gooseFunctionPointers.playHonckSound = new API.GooseFunctionPointers.PlayHonckSoundFunction(Sound.HONCC);
            API.ModHelperFunctions modHelperFunctions = new API.ModHelperFunctions();
            modHelperFunctions.getModDirectory = new API.ModHelperFunctions.GetModDirectoryFunction(GetModDirectory);
            API.TaskDatabaseQueryFunctions taskDatabaseQueryFunctions = new API.TaskDatabaseQueryFunctions();
            taskDatabaseQueryFunctions.getTaskIndexByID = new API.TaskDatabaseQueryFunctions.GetTaskIndexByIDFunction(GooseTaskDatabase.GetTaskIndexByID);
            taskDatabaseQueryFunctions.getAllLoadedTaskIDs = new API.TaskDatabaseQueryFunctions.GetAllLoadedTaskIDsFunction(GooseTaskDatabase.GetAllLoadedTaskIDs);
            taskDatabaseQueryFunctions.getRandomTaskID = new API.TaskDatabaseQueryFunctions.GetNextRandomTaskFunction(GooseTaskDatabase.GetRandomTaskID);
            API.Goose = gooseFunctionPointers;
            API.Helper = modHelperFunctions;
            API.TaskDatabase = taskDatabaseQueryFunctions;
            foreach (IMod mod2 in modEntryPoints)
            {
                mod2.Init();
            }
            InjectionPoints.RaisePostModLoad();
        }

        public static string GetModDirectory(IMod mod)
        {
            if (!modPaths.ContainsKey(mod))
            {
                return "No such mod found, at least not loaded at initialization.";
            }
            return modPaths[mod];
        }
    }
}
