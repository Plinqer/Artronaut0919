using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.ReplayKit.Editor
{
    internal static class ReplayKitMenuManager
    {
        #region Constants

        private const string kMenuItemPath = "Window/Voxel Busters/Replay Kit";

        #endregion

        #region Menu items

        [MenuItem(kMenuItemPath + "/Open Settings", priority = 0)]
        public static void OpenSettings()
        {
            ReplayKitSettingsEditorUtility.OpenInProjectSettings();
        }

        [MenuItem(kMenuItemPath + "/Migrate To UPM", priority = 2)]
        public static void MigrateToUPM()
        {
            ReplayKitEditorUtility.MigratePackagesToUPM();
        }

        [MenuItem(kMenuItemPath + "/Migrate To UPM", isValidateFunction: true, priority = 2)]
        private static bool ValidateMigrateToUPM()
        {
            return ReplayKitEditorUtility.CanMigratePackagesToUPM();
        }

        [MenuItem(kMenuItemPath + "/Uninstall", priority = 3)]
        public static void Uninstall()
        {
            ReplayKitSettingsEditorUtility.RemoveGlobalDefines();
            UninstallPlugin.Uninstall();
        }

        #endregion
    }
}