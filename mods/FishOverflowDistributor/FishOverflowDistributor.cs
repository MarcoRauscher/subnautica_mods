using System;
using System.IO;
using System.Reflection;
using Harmony;
using SharedCode.Config;
using SharedCode.Logging;
using SharedCode.Utils;
using UnityEngine.SceneManagement;

namespace FishOverflowDistributor
{
    /// <summary>
    ///     Contains mod entry point and basic bootstrapping
    /// </summary>
    public class FishOverflowDistributor
    {
        private const string ModId = "snowrabbit007.subnautica.FishOverflowDistributor"; //same as in mod.json
        private const string ModName = "FishOverflowDistributor";                        //same as in mod.json
        private const string LogFileName = "log.txt";
        private const string ConfigFileName = "config.yml";

        /// <summary>
        ///     Logger instance used for logging throughout the mod. Initialization done in Load() method.
        /// </summary>
        public static ILogger Logger { get; set; }

        /// <summary>
        ///     Config representing config.yml in the mod directory.
        /// </summary>
        public static FishOverflowDistributorConfig Config { get; set; }

        /// <summary>
        ///     Stores the directory path of the mod assembly (e.g. "QMods\SomeGenericMod\")
        /// </summary>
        public static string ModDirectoryPath
        {
            get
            {
                string p = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(p);
                string dir = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(dir);
            }
        }

        /// <summary>
        ///     Stores the full file path of the mod's log file (e.g. "QMods\SomeGenericMod\log.txt")
        /// </summary>
        public static string LogFilePath

        {
            get { return Path.Combine(ModDirectoryPath, LogFileName); }
        }

        /// <summary>
        ///     Stores the full file path of the mod's config file (e.g. "QMods\SomeGenericMod\config.yml")
        /// </summary>
        public static string ConfigFilePath
        {
            get { return Path.Combine(ModDirectoryPath, ConfigFileName); }
        }

        /// <summary>
        ///     Stores the directory path to the Subnautica Data Directory (e.g. Steamapps\Common\Subnautica\Subnautica
        /// </summary>
        public static string SubnauticaDirectoryPath
        {
            get { return Path.Combine(ModDirectoryPath, @"\..\Subnautica_Data\"); }
        }

        /// <summary>
        ///     Main entry point which is called by QMods. Specified in mod.json
        /// </summary>
        public static void Load()
        {
            Config = YamlConfigReader.Readconfig<FishOverflowDistributorConfig>(
                ConfigFilePath,
                x =>
                {
                    Console.WriteLine(
                        $"[{ModName}] [Fatal] Error parsing config file '{ConfigFilePath}. {Environment.NewLine}");

                    Console.WriteLine(ExceptionUtils.GetExceptionErrorString(x));
                });

            if (Config == null)
                return;


            Logger = new QModLogger()
                     .WithTarget(new QModFileLoggerTarget(LogFilePath, Config.LogLevel))
                     .WithTarget(new SubnauticaConsoleLoggerTarget(ModName, LogLevel.Error))
                     .Open();

            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;

            OnGameStart();
        }

        /// <summary>
        ///     Called when the game starts (We will be in main menu after this)
        /// </summary>
        public static void OnGameStart()
        {
            Logger.LogInfo($"Game start. Applying patches.");
            HarmonyInstance.Create(ModId).PatchAll(Assembly.GetExecutingAssembly());
        }

        /// <summary>
        ///     Called when scene "main" is loaded (the user has started a new game/loaded a save)
        /// </summary>
        public static void OnSceneStart()
        {
            Logger.LogInfo("Scene 'main' loaded.");

            //TODO: Apply behaviors which need to be done when scene has started
            //e.g. GameObject go = new GameObject();
            //e.g. go.AddComponent<SomeImportantComponent>();
        }

        /// <summary>
        ///     Called when scene "main" is unloaded (user exits subnautica)
        /// </summary>
        public static void OnSceneFinish()
        {
            Logger.LogInfo("scene 'main' unloaded.");

            //TODO: Do things here before the scene is destroyed
            //e.g. File.Write(SavePath, ImportantSaveInformation);
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Logger.LogTrace($"Scene '{scene.name}' loaded.");

            if (scene.name == "Main")
                OnSceneStart();
        }

        private static void OnSceneUnloaded(Scene scene)
        {
            Logger.LogTrace($"Scene '{scene.name}' unloaded.");

            if (scene.name == "Main")
                OnSceneFinish();
        }
    }
}
