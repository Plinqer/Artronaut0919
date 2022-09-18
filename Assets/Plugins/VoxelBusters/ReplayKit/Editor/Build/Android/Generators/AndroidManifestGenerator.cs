#if UNITY_ANDROID
using System;
using System.Xml;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.Editor.NativePlugins.Build.Android;

namespace VoxelBusters.ReplayKit
{
	public class AndroidManifestGenerator
	{
        #region Static fields

        private static string s_androidLibraryRootPackageName = "com.voxelbusters.replaykit";

        #endregion

        #region Public methods

        public static void GenerateManifest(ReplayKitSettings settings, string savePath = null)
        {
            Manifest manifest = new Manifest();
            manifest.AddAttribute("xmlns:android", "http://schemas.android.com/apk/res/android");
            manifest.AddAttribute("xmlns:tools", "http://schemas.android.com/tools");
            manifest.AddAttribute("package", s_androidLibraryRootPackageName + "plugin");
            manifest.AddAttribute("android:versionCode", "1");
            manifest.AddAttribute("android:versionName", "1.0");

            AddQueries(manifest, settings);


            Application application = new Application();

            AddActivities(application, settings);
            AddProviders(application, settings);
            AddServices(application, settings);
            AddReceivers(application, settings);
            AddMetaData(application, settings);

            manifest.Add(application);

            AddPermissions(manifest, settings);
            AddFeatures(manifest, settings);


            XmlDocument xmlDocument = new XmlDocument();
            XmlNode xmlNode = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);

            // Append xml node
            xmlDocument.AppendChild(xmlNode);

            // Get xml hierarchy
            XmlElement element = manifest.GenerateXml(xmlDocument);
            xmlDocument.AppendChild(element);

            // Save to androidmanifest.xml
            xmlDocument.Save(savePath == null ? IOServices.CombinePath(AssetConstants.AndroidProjectPath, "AndroidManifest.xml") : savePath);
        }

        #endregion

        #region Private methods

        private static void AddQueries(Manifest manifest, ReplayKitSettings settings)
        {
        }

        private static void AddActivities(Application application, ReplayKitSettings settings)
        {
        }

        private static void AddProviders(Application application, ReplayKitSettings settings)
        {
        }

        private static void AddServices(Application application, ReplayKitSettings settings)
        {
        }

        private static void AddReceivers(Application application, ReplayKitSettings settings)
        {
        }

        private static void AddMetaData(Application application, ReplayKitSettings settings)
        {
        }

        private static void AddFeatures(Manifest manifest, ReplayKitSettings settings)
        {
        }

        private static void AddPermissions(Manifest manifest, ReplayKitSettings settings)
        {
            Permission permission;


            if (settings.UsesMicrophone)
            {
                //Record Audio Permission
                permission = new Permission();
                permission.AddAttribute("android:name", "android.permission.RECORD_AUDIO");
                manifest.Add(permission);
            }

            if (ReplayKitSettings.Instance.Android.UsesSavePreview)
            {
                //For reading files from external storage
                permission = new Permission();
                permission.AddAttribute("android:name", "android.permission.READ_EXTERNAL_STORAGE");
                permission.AddAttribute("tools:remove", "android:maxSdkVersion");
                manifest.Add(permission);

                //For storing files in external storage
                permission = new Permission();
                permission.AddAttribute("android:name", "android.permission.WRITE_EXTERNAL_STORAGE");
                permission.AddAttribute("tools:remove", "android:maxSdkVersion");
                manifest.Add(permission);
            }
        }

        #endregion
    }
}
#endif