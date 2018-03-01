using System;
using System.IO;
using System.Reflection;
using UnityEngine.SceneManagement;
using SharedCode.Logging;

namespace SharedCode.ModBase
{
    public abstract class ModBase
    {
        public static ILogger Logger;
        public static string ModDirectory;
        public static string SubnauticaDirectory;
        public static string SubnauticaManagedDirectory;

        public void QModInitialize()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var uriBuilder = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uriBuilder.Path);

            ModDirectory = Path.GetDirectoryName(path);
            SubnauticaDirectory = Directory.GetParent(ModDirectory).FullName;
            SubnauticaManagedDirectory = Path.Combine(SubnauticaDirectory, "Subnautica_Data");

            //TODO: initialize config
            //TODO: initialize logger

            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "Main")
                OnGameStarted();
        }

        private static void OnSceneUnloaded(Scene scene)
        {
            if (scene.name == "Main")
                OnGameEnded();
        }

        //virtual static void OnLoad();

        private static void OnLoad()
        {
            Logger.Open();
            //TODO: apply patches here
        }

        private static void OnGameStarted()
        {
            //TODO: apply behaviour here
        }

        private static void OnGameEnded()
        {
            Logger.Close();
        }


    }
}
