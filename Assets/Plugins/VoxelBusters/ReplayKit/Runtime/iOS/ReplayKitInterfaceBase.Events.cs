#if UNITY_IOS
using UnityEngine;
using System.Collections;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.ReplayKit.iOS
{
	public partial class ReplayKitInterfaceBase
	{
        public void OnReplayKitPrepareRecordingStarted(string message)
        {
            SendOnPrepareRecordingStateChange(state: ReplayKitPrepareRecordingState.Started);
        }

        public void OnReplayKitPrepareRecordingFinished(string message)
        {
            SendOnPrepareRecordingStateChange(state: ReplayKitPrepareRecordingState.Finished);
        }

        public void OnReplayKitPrepareRecordingFailed(string message)
        {
            SendOnPrepareRecordingStateChange(state: ReplayKitPrepareRecordingState.Failed, new Error(message ?? "Unknown error."));
        }

        public void OnReplayKitRecordingStopped(string message)
        {
            SendOnRecordingStateChange(ReplayKitRecordingState.Stopped);
        }

        public void OnReplayKitRecordingStarted(string message)
        {
            SendOnRecordingStateChange(ReplayKitRecordingState.Started);
        }

        public void OnReplayKitSaveToGalleryFinished(string message)
        {
            SendOnSavePreviewCompleteCallback(string.IsNullOrEmpty(message) ? null : new Error(message));
        }

        public void OnReplayKitRecordingAvailable(string message)
        {
            SendOnRecordingStateChange(ReplayKitRecordingState.Available);
        }

        public void OnReplayKitRecordingFailed(string message)
        {
            SendOnRecordingStateChange(ReplayKitRecordingState.Failed, new Error(message ?? "Unknown error."));
        }

        public void OnReplayKitRecordingUIStartAction(string message)
        {
            SendOnRecordingUIActionChange(RecordingUIAction.Started);
        }

        public void OnReplayKitRecordingUIStopAction(string message)
        {
            SendOnRecordingUIActionChange(RecordingUIAction.Stopped);
        }
    }
}
#endif