using UnityEngine;
using System.Collections;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.ReplayKit.Simulator
{
	public class ReplayKitInterface : NativeReplayKitInterface
	{
        #region Fields

        private     bool    m_isRecording           = false;

        private     bool    m_isPreviewAvailable    = false;

        private     string  m_previewVideoFile      = "https://www.youtube.com/watch?v=aqz-KE-bpKQ";

        #endregion

        #region Base class implementation

        protected override bool GetIsAvailableInternal() => true;

        public override void Initialise()
		{
            if (IsRecordingAPIAvailable())
            {
                SendOnInitialiseComplete(state:  ReplayKitInitialisationState.Success);
            }
            else
            {
                SendOnInitialiseComplete(state: ReplayKitInitialisationState.Failed, error: new Error("Replay kit API not available"));
            }
        }

        public override bool IsCameraEnabled()
        {
            return false;
        }

        public override bool IsPreviewAvailable()
        {
            return m_isPreviewAvailable;
        }

        public override bool IsRecording()
        {
            return m_isRecording;
        }

        public override bool IsRecordingAPIAvailable()
        {
            return true;
        }

        public override void SetMicrophoneStatus(bool enable)
        { }

        public override void SetRecordingUIVisibility(bool show)
        {
            DebugLogger.LogWarning("Not implemented on editor. Please check on device for the UI");
        }

        public override void PrepareRecording()
        {
            Debug.Log("Preparing for recording...");
            SendOnPrepareRecordingStateChange(ReplayKitPrepareRecordingState.Started);
            SendOnPrepareRecordingStateChange(ReplayKitPrepareRecordingState.Finished);
        }

        public override void StartRecording()
        {
            m_isRecording           = true;
            SendOnRecordingStateChange(ReplayKitRecordingState.Started);
        }

        public override void StopRecording()
        {
            m_isRecording           = false;
            SendOnRecordingStateChange(ReplayKitRecordingState.Stopped);
            m_isPreviewAvailable    = true;
            SendOnRecordingStateChange(ReplayKitRecordingState.Available);
        }

        public override bool Preview()
        {
#if UNITY_EDITOR
            if(m_isPreviewAvailable)
            {
                Application.OpenURL(m_previewVideoFile);
                return true;
            }
            else
#endif
            {
                return false;
            }
        }

        public override bool Discard()
        {
            m_isPreviewAvailable    = false;
            return true;
        }

        public override string GetPreviewFilePath()
        {
            if (!m_isPreviewAvailable)
            {
                return null;
            }
            else
            {
                return m_previewVideoFile;
            }
        }

        protected override void SavePreviewInternal(string filename)
        {
            SendOnSavePreviewCompleteCallback(null);
        }

        public override void SharePreview(string text = null, string subject = null)
        {
            SendOnPreviewStateChange(ReplayKitPreviewState.Shared);
        }

        #endregion
    }
}