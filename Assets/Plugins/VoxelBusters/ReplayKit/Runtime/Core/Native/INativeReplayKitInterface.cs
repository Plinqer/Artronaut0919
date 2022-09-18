using UnityEngine;
using System.Collections;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.ReplayKit
{
    public interface INativeReplayKitInterface : INativeFeatureInterface
    {
        #region Properties

        ReplayKitSettings Settings { get; }

        #endregion

        #region Events

        event OnInitialiseCompleteInternalDelegate OnInitialiseComplete;

        event OnPrepareRecordingStateChangeInternalDelegate OnPrepareRecordingStateChange;

        event OnRecordingStateChangeInternalDelegate OnRecordingStateChange;

        event OnPreviewStateChangeInternalDelegate OnPreviewStateChange;

        event OnRecordingUIActionChangeInternalDelegate OnRecordingUIActionChange;

        #endregion

        #region Methods

        // Init
        void Initialise();

        // Query
        bool IsRecordingAPIAvailable();
        bool IsRecording();
        bool IsPreviewAvailable();
        bool IsCameraEnabled();

        // Actions
        void SetMicrophoneStatus(bool enable);
        void SetRecordingUIVisibility(bool show);

        void PrepareRecording();
        void StartRecording();
        void StopRecording();
        bool Preview();
        bool Discard();
        string GetPreviewFilePath();

        void SavePreview(string filename, CompletionCallback onCompletion);
        void SharePreview(string text = null, string subject = null);

        #endregion
    }
}