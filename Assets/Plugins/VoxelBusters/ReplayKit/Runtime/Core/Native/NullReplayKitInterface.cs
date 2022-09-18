using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.ReplayKit
{
    public class NullReplayKitInterface : NativeReplayKitInterface
    {
        #region Private static methods

        private static void LogNotSupported()
        {
            Diagnostics.LogNotSupported("ReplayKit");
        }

        #endregion

        #region Base class implementation

        protected override bool GetIsAvailableInternal() => false;

        public override void Initialise()
		{
            LogNotSupported();
            SendOnInitialiseComplete(state: ReplayKitInitialisationState.Failed, error: new Error("Replay kit is not supported."));
        }

        public override bool IsCameraEnabled()
        {
            LogNotSupported();
            return false;
        }

        public override bool IsPreviewAvailable()
        {
            LogNotSupported();
            return false;
        }

        public override bool IsRecording()
        {
            LogNotSupported();
            return false;
        }

        public override bool IsRecordingAPIAvailable()
        {
            LogNotSupported();
            return false;
        }

        public override void SetMicrophoneStatus(bool enable)
        {
            LogNotSupported();
        }

        public override void SetRecordingUIVisibility(bool show)
        {
            LogNotSupported();
        }

        public override void PrepareRecording()
        {
            LogNotSupported();
            SendOnPrepareRecordingStateChange(ReplayKitPrepareRecordingState.Failed, new Error("Not supported."));
        }

        public override void StartRecording()
        {
            LogNotSupported();
            SendOnRecordingStateChange(ReplayKitRecordingState.Failed, new Error("Not supported."));
        }

        public override void StopRecording()
        {
            LogNotSupported();
        }

        public override bool Preview()
        {
            LogNotSupported();
            return false;
        }

        public override bool Discard()
        {
            LogNotSupported();
            return false;
        }

        public override string GetPreviewFilePath()
        {
            LogNotSupported();
            return null;
        }

        protected override void SavePreviewInternal(string filename)
        {
            LogNotSupported();
            SendOnSavePreviewCompleteCallback(new Error("Not supported."));
        }

        public override void SharePreview(string text = null, string subject = null)
        {
            LogNotSupported();
        }

        #endregion
    }
}