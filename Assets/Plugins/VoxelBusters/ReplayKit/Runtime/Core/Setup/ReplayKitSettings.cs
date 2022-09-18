using System;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.ReplayKit
{
    public partial class ReplayKitSettings : SettingsObject
    {
        #region Static fields

        private     static      ReplayKitSettings       s_sharedInstance;

        private     static      UnityPackageDefinition  s_package;

        #endregion

        #region Fields

        [SerializeField]
        private     bool                        m_isEnabled                     = true;

        [SerializeField]
        private     bool                        m_usesMicrophone                = true;

        [SerializeField]
        private     IosPlatformProperties       m_iosProperties                 = new IosPlatformProperties();

        [SerializeField]
        private     AndroidPlatformProperties   m_androidProperties             = new AndroidPlatformProperties();

        #endregion

        #region Events

        public event Action SettingsUpdated;

        #endregion

        #region Static properties

        internal static UnityPackageDefinition Package
        {
            get
            {
                if (s_package == null)
                {
                    s_package   = new UnityPackageDefinition(
                        name: "com.voxelbusters.replaykit",
                        displayName: "Replay Kit",
                        version: "1.7.0",
                        defaultInstallPath: $"Assets/Plugins/VoxelBusters/ReplayKit");
                }
                return s_package;
            }
        }

        public static string PackageName => Package.Name;

        public static string DisplayName => Package.DisplayName;

        public static string Version => Package.Version;

        public static string DefaultSettingsAssetName => "ReplayKitSettings";

        public static string DefaultSettingsAssetPath => $"{Package.GetMutableResourcesPath()}/{DefaultSettingsAssetName}.asset";

        public static string PersistentDataPath => Package.PersistentDataPath;

        public static ReplayKitSettings Instance
        {
            get { return GetSharedInstanceInternal(); }
        }

        #endregion

        #region Properties

        public bool IsEnabled
        {
            get => m_isEnabled;
            set => m_isEnabled  = value;
        }

        public bool UsesMicrophone
        {
            get => m_usesMicrophone;
            set => m_usesMicrophone = value;
        }

        public IosPlatformProperties IosProperties
        {
            get => m_iosProperties;
            set => m_iosProperties  = value;
        }

        public AndroidPlatformProperties Android
        {
            get => m_androidProperties;
            set => m_androidProperties  = value;
        }

        #endregion

        #region Static methods

        public static void SetSettings(ReplayKitSettings settings)
        {
            Assert.IsArgNotNull(settings, nameof(settings));

            // set properties
            s_sharedInstance    = settings;
        }

        private static ReplayKitSettings GetSharedInstanceInternal(bool throwError = true)
        {
            if (null == s_sharedInstance)
            {
                // check whether we are accessing in edit or play mode
                var     assetPath   = DefaultSettingsAssetName;
                var     settings    = Resources.Load<ReplayKitSettings>(assetPath);
                if (throwError && (null == settings))
                {
                    throw Diagnostics.PluginNotConfiguredException();
                }

                // store reference
                s_sharedInstance = settings;
            }

            return s_sharedInstance;
        }

        private void OnValidate()
        {
            if(SettingsUpdated != null)
            {
                SettingsUpdated();
            }
        }

        #endregion
    }
}