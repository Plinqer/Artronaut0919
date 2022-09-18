using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.CoreLibrary.Editor;

namespace VoxelBusters.ReplayKit.Editor
{
    public static class ReplayKitEditorUtility
    {
        #region Constants

        private     const   string      kDefaultSettingsAssetOldPath    = "Assets/Resources/ReplayKit/ReplayKitSettings.asset";

		// URL
        private     const   string      kProductUrl                     = "http://u3d.as/1nN3";

		private 	const   string      kSupportUrl			            = "https://discord.gg/jegTXvqPKQ";

		private		const   string      kTutorialUrl		            = "https://assetstore.replaykit.voxelbusters.com";		

		private		const   string	    kSubscribePageUrl	            = "http://bit.ly/2ESQfAg";

        #endregion

        #region Public methods

        public static bool CanMigratePackagesToUPM()
        {
            return ReplayKitSettings.Package.IsInstalledWithinAssets();
        }

        public static void MigratePackagesToUPM()
        {
            CoreLibrarySettings.Package.MigrateToUPM();
            ReplayKitSettings.Package.MigrateToUPM();
        }

        #endregion

        #region Resource methods

        public static void OpenTutorialsPage()
        {
            Application.OpenURL(kTutorialUrl);
        }

        public static void OpenSupportPage()
        {
            Application.OpenURL(kSupportUrl);
        }

        public static void OpenSubscribePage()
        {
            Application.OpenURL(kSubscribePageUrl);
        }

        public static void OpenProductPage()
        {
            Application.OpenURL(kProductUrl);
        }

        #endregion

        #region Private methods

        private static bool IsFileStructureOutdated()
        {
            return IOServices.FileExists(kDefaultSettingsAssetOldPath);
        }

        private static void MigrateToNewFileStructure()
        {
            if (!AssetDatabase.IsValidFolder("Assets/Resources"))
            {
                AssetDatabase.CreateFolder("Assets", "Resources");
            }
            AssetDatabase.MoveAsset(kDefaultSettingsAssetOldPath, ReplayKitSettings.DefaultSettingsAssetPath);
        }

        #endregion

        #region Callback methods

        [InitializeOnLoadMethod]
        private static void PostProcessPackage()
        {
            EditorApplication.delayCall += () =>
            {
                if (IsFileStructureOutdated())
                {
                    MigrateToNewFileStructure();
                    AssetDatabase.DeleteAsset(kDefaultSettingsAssetOldPath.Substring(0, kDefaultSettingsAssetOldPath.LastIndexOf('/')));
                }
            };
        }

        #endregion
    }
}