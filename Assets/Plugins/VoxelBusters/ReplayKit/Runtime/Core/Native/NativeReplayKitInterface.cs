using UnityEngine;
using System;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.ReplayKit
{
    public abstract class NativeReplayKitInterface : MonoBehaviour, INativeReplayKitInterface
    {
        #region Properties

        private CompletionCallback m_onSavePreviewComplete;    

        #endregion

        #region IDispose implementation

        public virtual void Dispose()
        { }

        #endregion

        #region INativeObject implementation

        public NativeObjectRef NativeObjectRef => GetNativeObjectRefInternal();

        public virtual IntPtr AddrOfNativeObject() => IntPtr.Zero;

        protected virtual NativeObjectRef GetNativeObjectRefInternal() => null;

        #endregion

        #region INativeFeatureInterface implementation

        public bool IsAvailable => GetIsAvailableInternal();

        protected abstract bool GetIsAvailableInternal();

        #endregion

        #region INativeReportingKitInterface implementation

        public ReplayKitSettings Settings { get; internal set; }

        public event OnInitialiseCompleteInternalDelegate OnInitialiseComplete;

        public event OnPrepareRecordingStateChangeInternalDelegate OnPrepareRecordingStateChange;

        public event OnRecordingStateChangeInternalDelegate OnRecordingStateChange;

        public event OnPreviewStateChangeInternalDelegate OnPreviewStateChange;

        public event OnRecordingUIActionChangeInternalDelegate OnRecordingUIActionChange;

        public abstract void Initialise();

        public abstract bool IsRecordingAPIAvailable();

        public abstract bool IsRecording();

        public abstract bool IsPreviewAvailable();

        public abstract bool IsCameraEnabled();

        public abstract void SetMicrophoneStatus(bool enable);

        public abstract void SetRecordingUIVisibility(bool show);

        public abstract void PrepareRecording();

        public abstract void StartRecording();

        public abstract void StopRecording();

        public abstract bool Preview();

        public abstract bool Discard();

        public abstract string GetPreviewFilePath();

        public void SavePreview(string filename, CompletionCallback onCompletion)
        {
            // cache callback
            m_onSavePreviewComplete = onCompletion;

            SavePreviewInternal(filename);
        }

        protected abstract void SavePreviewInternal(string filename);

        public abstract void SharePreview(string text = null, string subject = null);

        #endregion

        #region Event dispatch methods

        protected void SendOnInitialiseComplete(ReplayKitInitialisationState state, Error error = null)
        {
            OnInitialiseComplete?.Invoke(state, error);
        }

        protected void SendOnPrepareRecordingStateChange(ReplayKitPrepareRecordingState state, Error error = null)
        {
            OnPrepareRecordingStateChange?.Invoke(state, error);
        }

        protected void SendOnRecordingStateChange(ReplayKitRecordingState state, Error error = null)
        {
            OnRecordingStateChange?.Invoke(state, error);
        }

        protected void SendOnPreviewStateChange(ReplayKitPreviewState state, Error error = null)
        {
            OnPreviewStateChange?.Invoke(state, error);
        }

        protected void SendOnRecordingUIActionChange(RecordingUIAction action, Error error = null)
        {
            OnRecordingUIActionChange?.Invoke(action, error);
        }

        protected void SendOnSavePreviewCompleteCallback(Error error)
        {
            m_onSavePreviewComplete?.Invoke(error);
        }

        #endregion
    }

    /*
    {
		INativeService m_service;
        private bool m_enableMicrophone;
        private ReplayKitDelegates.SavePreviewCompleteCallback m_savePreviewCallback;

        private bool m_audioListenerStatus;
        private bool m_isInitialised;

        #region Query Methods

        public void Initialise()
        {
            m_isInitialised = true;
            m_service.Initialise(this);
        }

        public bool IsInitialised()
        {
            return m_isInitialised;
        }

        public bool IsRecordingAPIAvailable()
        {
			return m_service.IsRecordingAPIAvailable();
        }

        public bool IsCameraEnabled()
        {
			return m_service.IsCameraEnabled();
        }

        public bool IsRecording()
        {
            return m_service.IsRecording();
        }

        public bool IsMicrophoneEnabled()
        {
            return m_enableMicrophone;
        }

        public bool IsPreviewAvailable()
        {
            return m_service.IsPreviewAvailable();
        }

#endregion

#region Recording Operations

        public void SetMicrophoneStatus(bool enable)
        {
            m_enableMicrophone = enable;
            m_service.SetMicrophoneStatus(enable);
        }

        public void SetRecordingUIVisibility(bool visible)
        {
            m_service.SetRecordingUIVisibility(visible);
        }

        public void PrepareRecording()
        {
            m_service.PrepareRecording();
        }

        public void StartRecording()
        {
            m_service.StartRecording();
        }

        public void StopRecording()
        {
            m_service.StopRecording();
        }

        public bool Preview()
        {
            return m_service.Preview();
        }

        public string GetPreviewFilePath()
        {
            return m_service.GetPreviewFilePath();
        }

        public bool Discard()
        {
            return m_service.Discard();
        }

#endregion

#region Utility

        public void SavePreview(ReplayKitDelegates.SavePreviewCompleteCallback callback)
        {
            m_savePreviewCallback = callback;
            m_service.SavePreview();
        }

        public void SharePreview(string text = null, string subject = null)
        {
            m_service.SharePreview(text, subject);
        }

#endregion


#region Overriden Methods

        protected override void Init()
        {
            base.Init();

			// Not interested in non singleton instance
			if (instance != this)
                return;

#if (UNITY_ANDROID && !UNITY_EDITOR)
			m_service = this.gameObject.AddComponent<ReplayKitAndroid>();
#elif (UNITY_IOS && !UNITY_EDITOR)
            if(UnityShouldAutorotate() == 1)
            {
                m_service = this.gameObject.AddComponent<ReplayKitIOSNormalRecorder>();
            }
            else
            {
                m_service = this.gameObject.AddComponent<ReplayKitIOSCustomRecorder>();
            }
#else
			m_service = this.gameObject.AddComponent<ReplayKitDefaultPlatform>();
#endif
        }

#endregion

    } 
    */
}