using UnityEngine;
using System;
using System.Collections;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.ReplayKit
{
    [IncludeInDocs]
    public class ReplayKitManager
    {
        #region Constants

        private     const   string      kMainAssembly                   = "VoxelBusters.ReplayKit";

        private     const   string      kRootNamespace                  = "VoxelBusters.ReplayKit";

        #endregion

        #region Static fields

        private     static  NativeFeatureRuntimeConfiguration   s_runtimeConfiguration      = new NativeFeatureRuntimeConfiguration(
            packages: new NativeFeatureRuntimePackage[]
            {
                new NativeFeatureRuntimePackage(platform: RuntimePlatform.IPhonePlayer, assembly: $"{kMainAssembly}.iOSModule",     ns: $"{kRootNamespace}.iOS",        nativeInterfaceType: "ReplayKitInterface"),
                new NativeFeatureRuntimePackage(platform: RuntimePlatform.tvOS,         assembly: $"{kMainAssembly}.iOSModule",     ns: $"{kRootNamespace}.iOS",        nativeInterfaceType: "ReplayKitInterface"),
                new NativeFeatureRuntimePackage(platform: RuntimePlatform.Android,      assembly: $"{kMainAssembly}.AndroidModule", ns: $"{kRootNamespace}.Android",    nativeInterfaceType: "ReplayKitInterface"),
            },
            simulatorPackage: new NativeFeatureRuntimePackage(assembly: $"{kMainAssembly}.SimulatorModule", ns: $"{kRootNamespace}.Simulator",  nativeInterfaceType: "ReplayKitInterface"),
            fallbackPackage: new NativeFeatureRuntimePackage(assembly: kMainAssembly,                       ns: kRootNamespace,                 nativeInterfaceType: "NullReplayKitInterface" ));

        #endregion

        #region Static properties

        public static bool IsInitialised { get; private set; }

        private static INativeReplayKitInterface NativeInterface { get; set; }

        #endregion

        #region Static events

        public static event EventCallback<InitialiseCompleteResult> DidInitialise;

        public static event EventCallback<RecordingStateChangeResult> DidRecordingStateChange;

        public static event EventCallback<PrepareRecordingStateChangeResult> DidPrepareRecordingStateChange;

        private static EventCallback<RecordingPreviewAvailableResult> m_onRecordingPreviewAvailable;

        private static EventCallback<PrepareRecordingCompleteResult> m_onPrepareRecordingFinished;

        #endregion

        #region Constructors

        static ReplayKitManager()
        { }

        #endregion

        #region Public Methods

        /// <summary>
        /// Initialise Replay Kit
        /// </summary>
        public static void Initialise()
        {
            if (!EnsureInterfaceIsSet()) return;

            if (IsInitialised)
            {
                var     result  = new InitialiseCompleteResult(state: ReplayKitInitialisationState.Success);
                DidInitialise?.Invoke(result, null);
                return;
            }
                
            NativeInterface.Initialise();
        }

        /// <summary>
        /// Check if Recording API is available on this platform
        /// </summary>
        /// <returns><c>true</c>, if recording API is available, <c>false</c> otherwise.</returns>
        public static bool IsRecordingAPIAvailable()
        {
            if (!EnsureInterfaceIsSet()) return false;

            return NativeInterface.IsRecordingAPIAvailable();
        }

        public static bool IsRecording()
        {
            if (!EnsureInterfaceIsSet()) return false;

            return NativeInterface.IsRecording();
        }

        public static bool IsPreviewAvailable()
        {
            if (!EnsureInterfaceIsSet()) return false;

            return NativeInterface.IsPreviewAvailable();
        }

        public static void SetMicrophoneStatus(bool enable)
        {
            if (!EnsureInterfaceIsSet()) return;

            NativeInterface.SetMicrophoneStatus(enable);
        }

        public static void PrepareRecording(EventCallback<PrepareRecordingCompleteResult> callback = null)
        {
            if (!EnsureInterfaceIsSet()) return;

            // store callback
            m_onPrepareRecordingFinished = callback;

            NativeInterface.PrepareRecording();
        }

        public static void StartRecording()
        {
            if (!EnsureInterfaceIsSet()) return;

            if (!IsInitialised)
            {
                Initialise();
            }

            NativeInterface.StartRecording();
        }

        public static void StopRecording(EventCallback<RecordingPreviewAvailableResult> callback = null)
        {
            if (!EnsureInterfaceIsSet()) return;

            // store callback
            m_onRecordingPreviewAvailable   = callback;

            // make the request
            NativeInterface.StopRecording();
        }

        public static bool Preview()
        {
            if (!EnsureInterfaceIsSet()) return false;

            return NativeInterface.Preview();
        }

        public static string GetPreviewFilePath()
        {
            if (!EnsureInterfaceIsSet()) return null;

            return NativeInterface.GetPreviewFilePath();
        }

        public static bool Discard()
        {
            if (!EnsureInterfaceIsSet()) return false;

            return NativeInterface.Discard();
        }

        public static void SavePreview(EventCallback<SavePreviewCompleteResult> callback)
        {
            if (!EnsureInterfaceIsSet()) return;

            NativeInterface.SavePreview(
                filename: null,
                onCompletion: (error) =>
                {
                    var     result  = (error != null) ? null : new SavePreviewCompleteResult();
                    CallbackDispatcher.InvokeOnMainThread(callback, result, error);
                });
        }

        public static void SharePreview()
        {
            if (!EnsureInterfaceIsSet()) return;

            NativeInterface.SharePreview(null);
        }

        #endregion

        #region Private Methods

        private static void ShowRecordingUI()
        {
            if (!EnsureInterfaceIsSet()) return;

            NativeInterface.SetRecordingUIVisibility(true);
        }

        private static void HideRecordingUI()
        {
            if (!EnsureInterfaceIsSet()) return;

            NativeInterface.SetRecordingUIVisibility(false);
        }

        private static bool IsCameraEnabled()
        {
            if (!EnsureInterfaceIsSet()) return false;

            return NativeInterface.IsCameraEnabled();
        }

        #endregion

        #region Private static methods

        private static bool EnsureInterfaceIsSet()
        {
            if (NativeInterface == null)
            {
                // check whether we can create interface object
                var     settings        = ReplayKitSettings.Instance;
                if (settings == null)
                {
                    DebugLogger.LogWarning("[ReplayKit] The requested operation could not be completed. And the reason is that ReplayKitSettings file is not found. Please configure the plugin in order to use the functionalities.");
                    return false;
                }
                else if (!settings.IsEnabled)
                {
                    DebugLogger.LogWarning("[ReplayKit] The requested operation could not be completed. And the reason is that the plugin is marked disabled. Please turn it ON in order to use the functionalities.");
                    return false;
                }

                // create interface object
                var     targetPackage   = s_runtimeConfiguration.GetPackageForPlatform(Application.platform);
                var     targetType      = ReflectionUtility.GetType(assemblyName: targetPackage.Assembly, typeName: targetPackage.NativeInterfaceType);
                var     interfaceGO     = new GameObject("ReplayKit");
                var     interfaceObj    = interfaceGO.AddComponent(targetType) as NativeReplayKitInterface;
                interfaceObj.Settings   = settings;
                UnityEngine.Object.DontDestroyOnLoad(interfaceGO);

                // store reference
                NativeInterface         = interfaceObj;

                // register for events
                NativeInterface.OnInitialiseComplete            += OnInitialiseComplete;
                NativeInterface.OnPrepareRecordingStateChange   += OnPrepareRecordingStateChange;
                NativeInterface.OnRecordingStateChange          += OnDidRecordingStateChange;
                NativeInterface.OnPreviewStateChange            += OnPreviewStateChange;
			    NativeInterface.OnRecordingUIActionChange       += OnDidRecordingUIActionChange;
            }
            return true;
        }

        private static void SendOnRecordingPreviewAvailable(RecordingPreviewAvailableResult result, Error error)
        {
            CallbackDispatcher.InvokeOnMainThread(m_onRecordingPreviewAvailable, result, error);
            m_onRecordingPreviewAvailable   = null;
        }

        private static void SendOnPrepareRecordingFinished(PrepareRecordingCompleteResult result, Error error)
        {
            CallbackDispatcher.InvokeOnMainThread(m_onPrepareRecordingFinished, result, error);
            m_onPrepareRecordingFinished    = null;
        }

        #endregion

        #region Event callback methods

        private static void OnInitialiseComplete(ReplayKitInitialisationState state, Error error)
        {
            // update state
            IsInitialised   = (state == ReplayKitInitialisationState.Success);

            // send event
            var     result  = new InitialiseCompleteResult(state: state);
            CallbackDispatcher.InvokeOnMainThread(DidInitialise, result, error);
        }

        private static void OnDidRecordingStateChange(ReplayKitRecordingState state, Error error)
        {
            // send event
            var     recordingResult     = new RecordingStateChangeResult(state: state);
            CallbackDispatcher.InvokeOnMainThread(DidRecordingStateChange, recordingResult, error);

            // additional actions specific to states
            if (state == ReplayKitRecordingState.Available)
            {
                var     previewResult   = new RecordingPreviewAvailableResult(path: GetPreviewFilePath());
                SendOnRecordingPreviewAvailable(previewResult, null);
            }
            else if (state == ReplayKitRecordingState.Failed)
            {
                SendOnRecordingPreviewAvailable(null, new Error("Failed while stopping video"));
            }
        }

        private static void OnPreviewStateChange(ReplayKitPreviewState state, Error error)
        { }

        private static void OnPrepareRecordingStateChange(ReplayKitPrepareRecordingState state, Error error)
        {
            var     stateChangeResult   = new PrepareRecordingStateChangeResult(state: state);
            DidPrepareRecordingStateChange?.Invoke(stateChangeResult, null);

            if (state == ReplayKitPrepareRecordingState.Failed)
            {
                SendOnPrepareRecordingFinished(result: null, error: error);
            }
            else if (state == ReplayKitPrepareRecordingState.Finished)
            {
                var     recordingResult = new PrepareRecordingCompleteResult();
                SendOnPrepareRecordingFinished(result: recordingResult, error: null);
            }
        }

        private static void OnDidRecordingUIActionChange(RecordingUIAction action, Error error)
		{
            switch(action)
			{
				case RecordingUIAction.Started:
					StartRecording();
					break;

				case RecordingUIAction.Stopped:
					StopRecording();
					break;

				default:
					Debug.LogError("Not implemented");
					break;
			}
		}

        #endregion
    }
}