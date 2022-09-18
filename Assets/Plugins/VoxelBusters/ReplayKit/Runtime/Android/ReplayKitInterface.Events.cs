#if UNITY_ANDROID
using UnityEngine;
using System.Collections;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.ReplayKit.Android
{
	public partial class ReplayKitInterface : NativeReplayKitInterface
    {
        public void OnReplayKitPrepareRecordingStarted(string message)
        {
            SendOnPrepareRecordingStateChange(ReplayKitPrepareRecordingState.Started);
        }

        public void OnReplayKitPrepareRecordingFinished(string message)
        {
            SendOnPrepareRecordingStateChange(ReplayKitPrepareRecordingState.Finished);
        }

        public void OnReplayKitPrepareRecordingFailed(string message)
        {
            SendOnPrepareRecordingStateChange(ReplayKitPrepareRecordingState.Failed, new Error(message));
        }

        public void OnReplayKitInitialiseSuccess(string message)
        {
            SendOnInitialiseComplete(ReplayKitInitialisationState.Success);
        }

        public void OnReplayKitInitialiseFailed(string message)
        {
            SendOnInitialiseComplete(ReplayKitInitialisationState.Failed, new Error(message));
        }

        public void OnReplayKitRecordingStarted(string message)
        {
            if (m_allowControllingAudio)
            {
                ResumeAudio();
            }
            SendOnRecordingStateChange(ReplayKitRecordingState.Started);
        }

        public void OnReplayKitRecordingStopped(string message)
        {
            if (m_allowControllingAudio)
            {
                ResumeAudio();
            }
            SendOnRecordingStateChange(ReplayKitRecordingState.Stopped);
        }

        public void OnReplayKitRecordingAvailable(string message)
        {
            // This is required as in future we may have some video processing (Audio+Video Mux)
            SendOnRecordingStateChange(ReplayKitRecordingState.Available);
        }

        public void OnReplayKitRecordingFailed(string message)
        {
            if (m_allowControllingAudio)
                ResumeAudio();

            SendOnRecordingStateChange(ReplayKitRecordingState.Failed, new Error(message));
        }

        public void OnReplayKitPreviewOpened(string message)
        {
            SendOnPreviewStateChange(ReplayKitPreviewState.Opened);
        }

        public void OnReplayKitPreviewClosed(string message)
        {
            SendOnPreviewStateChange(ReplayKitPreviewState.Closed);
        }

        public void OnReplayKitPreviewShared(string message)
        {
            SendOnPreviewStateChange(ReplayKitPreviewState.Shared);
        }

        public void OnReplayKitPreviewSaveSuccess(string message)
        {
            SendOnSavePreviewCompleteCallback(null);
        }

        public void OnReplayKitPreviewSaveFailed(string message)
        {
            //PREVIEW_UNAVAILABLE
            //PERMISSION_UNAVAILABLE
            SendOnSavePreviewCompleteCallback(new Error(message));
        }
    }
}
#endif