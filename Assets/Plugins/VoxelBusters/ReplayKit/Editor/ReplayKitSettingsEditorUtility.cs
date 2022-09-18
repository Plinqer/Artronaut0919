using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.CoreLibrary.Editor;

namespace VoxelBusters.ReplayKit.Editor
{
    [InitializeOnLoad]
    public static class ReplayKitSettingsEditorUtility
    {
        #region Constants

        private     const       string                      kLocalPathInProjectSettings     = "Project/Voxel Busters/Replay Kit";

        #endregion

        #region Static fields

        private     static      ReplayKitSettings           s_defaultSettings;

        #endregion

        #region Static properties

        public static ReplayKitSettings DefaultSettings
        {
            get
            {
                if (s_defaultSettings == null)
                {
                    var     instance    = LoadDefaultSettingsObject(throwError: false);
                    if (null == instance)
                    {
                        instance        = CreateDefaultSettingsObject();
                    }
                    s_defaultSettings   = instance;
                }
                return s_defaultSettings;
            }
            set
            {
                Assert.IsPropertyNotNull(value, nameof(value));

                // set new value
                s_defaultSettings       = value;
            }
        }

        public static bool SettingsExists
        {
            get
            {
                if (s_defaultSettings == null)
                {
                    s_defaultSettings   = LoadDefaultSettingsObject(throwError: false);
                }
                return (s_defaultSettings != null);
            }
        }

        #endregion

         #region Constructors

        static ReplayKitSettingsEditorUtility()
        {
            AddGlobalDefines();
        }

        #endregion

        #region Static methods

        public static void ShowSettingsNotFoundErrorDialog()
        {
            EditorUtility.DisplayDialog(
                title: "Error",
                message: "Replay Kit plugin is not configured. Please select plugin settings file from menu and configure it according to your preference.",
                ok: "Ok");
        }

        public static void AddGlobalDefines()
        {
            ScriptingDefinesManager.AddDefine("ENABLE_VOXELBUSTERS_REPLAY_KIT");
        }
        
        public static void RemoveGlobalDefines()
        {
            ScriptingDefinesManager.RemoveDefine("ENABLE_VOXELBUSTERS_REPLAY_KIT");
        }

        public static void OpenInProjectSettings()
        {
            if (!SettingsExists)
            {
                CreateDefaultSettingsObject();
            }
            Selection.activeObject  = null;
            SettingsService.OpenProjectSettings(kLocalPathInProjectSettings);
        }

        [SettingsProvider]
        private static SettingsProvider CreateSettingsProvider()
        {
            return SettingsProviderZ.Create(
                settingsObject: DefaultSettings,
                path: kLocalPathInProjectSettings,
                scopes: SettingsScope.Project);
        }

        #endregion

        #region Private static methods

        private static ReplayKitSettings CreateDefaultSettingsObject()
        {
            return AssetDatabaseUtility.CreateScriptableObject<ReplayKitSettings>(
                assetPath: ReplayKitSettings.DefaultSettingsAssetPath);
        }

        private static ReplayKitSettings LoadDefaultSettingsObject(bool throwError = true)
        {
            var     throwErrorFunc      = throwError ? () => Diagnostics.PluginNotConfiguredException() : (System.Func<System.Exception>)null;
            return AssetDatabaseUtility.LoadScriptableObject<ReplayKitSettings>(
                assetPath: ReplayKitSettings.DefaultSettingsAssetPath,
                throwErrorFunc: throwErrorFunc);
        }

        #endregion

        #region Internal methods

        internal static void Rebuild()
        {
            // Actions
            WriteAndroidManifestFile();

            // Refresh Database
            AssetDatabase.Refresh();
        }

        public static void WriteAndroidManifestFile()
        {
            /*
            string _manifestFolderPath = Constants.kAndroidPluginsReplayKitPath;

            if (AssetDatabaseUtils.FolderExists(_manifestFolderPath))
            {
                ReplayKitAndroidManifestGenerator _generator = new ReplayKitAndroidManifestGenerator();
#if UNITY_2018_4_OR_NEWER
                _generator.SaveManifest("com.voxelbusters.replaykitplugin", _manifestFolderPath + "/AndroidManifest.xml");
#else
				_generator.SaveManifest("com.voxelbusters.replaykitplugin", _manifestFolderPath + "/AndroidManifest.xml");
#endif
            }
            */
        }

        #endregion
    }
}