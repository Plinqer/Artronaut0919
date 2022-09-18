using UnityEngine;
using System.Collections;

namespace VoxelBusters.ReplayKit
{
	internal static class AssetConstants
	{
		#region Assets Path	

		public static string ExtrasPath => "Assets/Plugins/VoxelBusters/ReplayKit/Essentials";

        public static string EditorExtrasPath => $"{ExtrasPath}/Editor";

		public static string AndroidPluginPath => $"Assets/Plugins/Android";

		public static string AndroidProjectFolderName => "com.voxelbusters.replaykit.androidlib";

		public static string AndroidProjectPath => $"{AndroidPluginPath}/{AndroidProjectFolderName}";

		public static string NativePluginsExporterName => "ReplayKit";

		#endregion
	}
}