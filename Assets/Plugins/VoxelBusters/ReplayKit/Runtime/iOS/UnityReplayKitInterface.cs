#if UNITY_IOS
using UnityEngine;
using System.Collections;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.ReplayKit.iOS
{
    public class UnityReplayKitInterface : ReplayKitInterfaceBase
    {
        #region Fields

        private     string      m_cachedFilePath    = null;

        #endregion

        #region Base class implementation

        public override bool IsRecordingAPIAvailable()
        {
            return UnityEngine.Apple.ReplayKit.ReplayKit.APIAvailable;
        }

        public override bool IsRecording()
        {
            return UnityEngine.Apple.ReplayKit.ReplayKit.isRecording;
        }

        public override bool IsPreviewAvailable()
        {
            return !string.IsNullOrEmpty(m_cachedFilePath);   
        }

        public override void PrepareRecording()
        {
            replaykit_prepareRecording();
        }

        public override void StartRecording()
        {
            if (IsPreviewAvailable())
            {
                Discard();
            }

            bool    status  = UnityEngine.Apple.ReplayKit.ReplayKit.StartRecording(IsMicrophoneEnabled);
            if (status)
            {
                SendOnRecordingStateChange(ReplayKitRecordingState.Started);
            }
            else
            {
                Debug.LogError("Recording failed : " + UnityEngine.Apple.ReplayKit.ReplayKit.lastError);
                SendOnRecordingStateChange(ReplayKitRecordingState.Failed, new Error(IsRecordingAPIAvailable() ? "START_RECORDING_FAILED"  : "API_UNAVAILABLE"));
            }
        }

        public override void StopRecording()
        {
            bool status = UnityEngine.Apple.ReplayKit.ReplayKit.StopRecording();

            StartCoroutine(CheckVideoAvailable(status));
        }

        public override bool Preview()
        {
            return replaykit_previewRecording();
            //UnityEngine.Apple.ReplayKit.ReplayKit.Preview();
        }

        public override bool Discard()
        {
            m_cachedFilePath    = null;
            return UnityEngine.Apple.ReplayKit.ReplayKit.Discard();
        }

        public override string GetPreviewFilePath()
        {
            return m_cachedFilePath;
        }

        protected override void SavePreviewInternal(string filename)
        {
            string  previewFilePath = m_cachedFilePath;
            if (string.IsNullOrEmpty(previewFilePath))
            {
                Debug.LogError("No preview recording available for saving!");
                return;
            }

            replaykit_savePreview(previewFilePath);
        }

        public override void SharePreview(string text = null, string subject = null)
        {
            replaykit_sharePreview(text, subject);
        }

        public override void SetRecordingUIVisibility(bool visible)
        {
            replaykit_setRecordingUIVisibility(visible);
        }

        #endregion

        #region Private methods

        private IEnumerator CheckVideoAvailable(bool status)
        {
            yield return new WaitForSeconds(0.3f);

            while (status && string.IsNullOrEmpty(UnityEngine.Apple.ReplayKit.ReplayKit.lastError) && !UnityEngine.Apple.ReplayKit.ReplayKit.recordingAvailable)
            {
                yield return new WaitForSeconds(0.3f);
            }

            if (status && string.IsNullOrEmpty(UnityEngine.Apple.ReplayKit.ReplayKit.lastError))
            {
                SendOnRecordingStateChange(ReplayKitRecordingState.Stopped);
                m_cachedFilePath = replaykit_getPreviewFilePath();
                SendOnRecordingStateChange(ReplayKitRecordingState.Available); //This is required as in future we may have some video processing (Audio+Video Mux)
            }
            else
            {
                Discard();
                Debug.LogError("Recording failed : " + UnityEngine.Apple.ReplayKit.ReplayKit.lastError);
                SendOnRecordingStateChange(ReplayKitRecordingState.Failed, new Error(IsRecordingAPIAvailable() ? "START_RECORDING_FAILED"  : "API_UNAVAILABLE"));
            }
        }

        #endregion
    }
}
#endif