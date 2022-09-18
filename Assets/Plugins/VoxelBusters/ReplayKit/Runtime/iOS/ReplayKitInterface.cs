#if UNITY_IOS
using UnityEngine;
using System.IO;

namespace VoxelBusters.ReplayKit.iOS
{
    public class ReplayKitInterface : ReplayKitInterfaceBase
    {
        #region Base class implementation

        public override bool IsRecordingAPIAvailable()
        {
            return replaykit_isAPIAvailable();
        }

        public override bool IsRecording()
        {
            return replaykit_isRecording();
        }

        public override bool IsPreviewAvailable()
        {
            return replaykit_isPreviewAvailable();
        }

        public override void PrepareRecording()
        {
            replaykit_prepareRecording();
        }
        
        public override void StartRecording()
        {
            replaykit_startRecording();
        }

        public override void StopRecording()
        {
            replaykit_stopRecording();
        }

        public override bool Preview()
        {
            return replaykit_previewRecording();
        }

        public override bool Discard()
        {
            return replaykit_discardRecording();
        }

        public override string GetPreviewFilePath()
        {
            return replaykit_getPreviewFilePath();
        }

        protected override void SavePreviewInternal(string filename)
        {
            string  previewFilePath = GetPreviewFilePath();
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

        public override void SetMicrophoneStatus(bool enable)
        {
            base.SetMicrophoneStatus(enable);

            replaykit_setMicrophoneStatus(enable);
        }

        public override void SetRecordingUIVisibility(bool visible)
        {
            replaykit_setRecordingUIVisibility(visible);
        }

        #endregion
    }
}
#endif