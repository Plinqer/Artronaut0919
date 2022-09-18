using UnityEngine;
using UnityEngine.Serialization;

namespace VoxelBusters.ReplayKit
{
	public partial class ReplayKitSettings
	{
		/// <summary>
		/// Application Settings specific to Android platform.
		/// </summary>
		[System.Serializable]
		public class AndroidPlatformProperties
		{
            #region Fields

            [FormerlySerializedAs("m_videoMaxQuality")]
            [SerializeField]
            [Tooltip("Set the resolution at which you want to record. Setting higher resolution will have larger final video sizes.")]
            private     VideoQuality            m_videoQuality              = VideoQuality.QUALITY_720P;

            [SerializeField]
            [Tooltip("Enabling custom bitrates lets you set recommended bitrates compared to default values which give very big file sizes")]
            private     CustomBitRateSetting    m_customVideoBitrate        = null;

            [FormerlySerializedAs("m_allowExternalStoragePermission")]
            [SerializeField]
            [Tooltip("Enable this if you want to use SavePreview feature. This adds external storage permission to the manifest. Default is true.")]
            private     bool                    m_usesSavePreview           = true;

            [SerializeField]
            [Header("Advanced Settings")]
            [Tooltip("Enabling this will allow ReplayKit plugin to pause/resume audio sources to reduce load while starting/stopping recording. It is recommended to keep this setting on.")]
            private     bool                    m_allowControllingAudio     = true;

            [SerializeField]
            [Tooltip("This captures app audio better when enabled")]
            private     bool                    m_prioritiseAppAudioWhenUsingMicrophone = false;

            #endregion

            #region Properties

            public VideoQuality VideoQuality => m_videoQuality;

            public CustomBitRateSetting CustomBitrateSetting => m_customVideoBitrate;

            public bool UsesSavePreview => m_usesSavePreview;

            public bool AllowControllingAudio => m_allowControllingAudio;

            public bool PrioritiseAppAudioWhenUsingMicrophone => m_prioritiseAppAudioWhenUsingMicrophone;

            #endregion

            #region Constructors

            public AndroidPlatformProperties(VideoQuality videoQuality = VideoQuality.QUALITY_720P, CustomBitRateSetting customBitRate = null,
                bool usesSavePreview = true, bool allowControllingAudio = true,
                bool prioritiseAppAudioWhenUsingMicrophone = false)
            {
                // set properties
                m_videoQuality                          = videoQuality;
                m_customVideoBitrate                    = customBitRate;
                m_usesSavePreview                       = usesSavePreview;
                m_allowControllingAudio                 = allowControllingAudio;
                m_prioritiseAppAudioWhenUsingMicrophone = prioritiseAppAudioWhenUsingMicrophone;
            }

            #endregion
        }
    }
}