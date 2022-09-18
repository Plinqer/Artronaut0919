#if UNITY_IOS
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.IO;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.ReplayKit.iOS
{
    public abstract partial class ReplayKitInterfaceBase : NativeReplayKitInterface
    {
        #region Properties

        protected bool IsMicrophoneEnabled { get; set; } = false;

        #endregion

        #region External methods

        [DllImport("__Internal")]
	    public static extern int UnityShouldAutorotate();

        [DllImport("__Internal")]
        protected static extern void replaykit_prepareRecording();

        [DllImport("__Internal")]
        protected static extern void replaykit_startRecording();

        [DllImport("__Internal")]
        protected static extern void replaykit_stopRecording();

        [DllImport("__Internal")]
        protected static extern string replaykit_getPreviewFilePath();

        [DllImport("__Internal")]
        protected static extern bool replaykit_isAPIAvailable();

        [DllImport("__Internal")]
        protected static extern bool replaykit_isRecording();

        [DllImport("__Internal")]
        protected static extern bool replaykit_isPreviewAvailable();

        [DllImport("__Internal")]
        protected static extern bool replaykit_previewRecording();

        [DllImport("__Internal")]
        protected static extern void replaykit_sharePreview (string text, string subject);

        [DllImport("__Internal")]
        protected static extern void replaykit_savePreview (string filename);

        [DllImport("__Internal")]
        protected static extern bool replaykit_discardRecording ();

        [DllImport("__Internal")]
        protected static extern void replaykit_setRecordingUIVisibility(bool visible);

        [DllImport("__Internal")]
        protected static extern void replaykit_setMicrophoneStatus(bool isEnabled);

        #endregion

        #region Base class implementation

        protected override bool GetIsAvailableInternal() => true;

        public override void Initialise()
        {
            if (IsRecordingAPIAvailable())
            {
                SendOnInitialiseComplete(ReplayKitInitialisationState.Success);
            }
            else
            {
                SendOnInitialiseComplete(ReplayKitInitialisationState.Failed, new Error("ReplayKit API not available."));
            }
        }

        public override bool IsCameraEnabled()
        {
            return false;
        }

        public override void SetMicrophoneStatus(bool enable)
        {
            // cache value
            IsMicrophoneEnabled = enable;
        }

        #endregion
    }
}
#endif