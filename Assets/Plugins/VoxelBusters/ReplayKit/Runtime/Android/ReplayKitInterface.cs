#if UNITY_ANDROID
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.ReplayKit.Android
{
	public partial class ReplayKitInterface : NativeReplayKitInterface
	{
        #region Fields

        private bool                            m_allowControllingAudio;

        private Dictionary<AudioSource, float> collection = new Dictionary<AudioSource, float>();
        
        #endregion

        #region Constructors

        public ReplayKitInterface()
		{
			var     classObj    = new AndroidJavaClass(Native.Class.NAME);
			Plugin              = classObj.CallStatic<AndroidJavaObject>("getInstance", GetUnityActivity());
		}

        #endregion

        #region Base class implementation

        protected override bool GetIsAvailableInternal() => true;

        public override void Initialise()
		{
            if (IsRecordingAPIAvailable())
            {
                bool isAppAudioPriority         = ReplayKitSettings.Instance.Android.PrioritiseAppAudioWhenUsingMicrophone;
                m_allowControllingAudio         = ReplayKitSettings.Instance.Android.AllowControllingAudio;

                Plugin.Call(Native.Methods.INITIALISE);
                Plugin.Call(Native.Methods.SET_APP_AUDIO_PRIORITY, isAppAudioPriority);
            }
            else
            {
                SendOnInitialiseComplete(ReplayKitInitialisationState.Failed, new Error("Replay Kit API not available"));
            }
		}

        public override bool IsRecordingAPIAvailable()
        {
            return Plugin.Call<bool>(Native.Methods.IS_RECORDING_API_AVAILABLE);
        }

        public override bool IsRecording()
        {
            return Plugin.Call<bool>(Native.Methods.IS_RECORDING);
        }

        public override bool IsPreviewAvailable()
        {
            return Plugin.Call<bool>(Native.Methods.IS_PREVIEW_AVAILABLE);
        }

        public override bool IsCameraEnabled()
        {
            return Plugin.Call<bool>(Native.Methods.IS_CAMERA_ENABLED);
        }

        public override void SetMicrophoneStatus(bool enable)
        {
            Plugin.Call(Native.Methods.SET_MICROPHONE_STATUS, enable);
        }

        public override void SetRecordingUIVisibility(bool show)
        {
            Plugin.Call(Native.Methods.SET_RECORDING_UI_VISIBILITY, show);
        }

        public override void PrepareRecording()
        {
            Plugin.Call(Native.Methods.PREPARE_RECORDING);
        }

        public override void StartRecording()
        {
            if (!IsRecording())
            {
                if (m_allowControllingAudio)
                    PauseAudio();

                StartCoroutine(StartRecordingInternal());
            }
        }

        private IEnumerator StartRecordingInternal()
        {
            yield return new WaitForEndOfFrame();

            var     androidSettings = Settings.Android;
            Plugin.Call(Native.Methods.START_RECORDING, (int)GetNativeVideoQualityType(androidSettings.VideoQuality), androidSettings.CustomBitrateSetting.AllowCustomBitrates ? androidSettings.CustomBitrateSetting.BitrateFactor : -1f);
        }

        public override void StopRecording()
        {
            if (IsRecording())
            {
                if (m_allowControllingAudio)
                    PauseAudio();

                StartCoroutine(StopRecordingInternal());
            }
        }

        private IEnumerator StopRecordingInternal()
        {
            yield return new WaitForEndOfFrame();

            Plugin.Call(Native.Methods.STOP_RECORDING);
        }

        public override bool Preview()
        {
            return Plugin.Call<bool>(Native.Methods.PREVIEW_RECORDING);
        }

        public override bool Discard()
        {
            return Plugin.Call<bool>(Native.Methods.DISCARD_RECORDING);
        }

        public override string GetPreviewFilePath()
        {
            return Plugin.Call<string>(Native.Methods.PREVIEW_FILE_PATH);
        }

        protected override void SavePreviewInternal(string filename)
        {
            if (!ReplayKitSettings.Instance.Android.UsesSavePreview)
            {
                string message = "Please enable UsesSavePreview in ReplayKit Settings and click on save to use this feature!";
                Debug.LogError("[ReplayKit] " + message);
                Plugin.Call(Native.Methods.SHOW_MESSAGE, message);
            }
            Plugin.Call(Native.Methods.SAVE_PREVIEW, filename);
        }

        public override void SharePreview(string text = null, string subject = null)
        {
            Plugin.Call(Native.Methods.SHARE_PREVIEW, text, subject);
        }

        #endregion

        #region Helpers

        private void PauseAudio()
        {
            AudioSource[] audioArray = FindObjectsOfType<AudioSource>();
            collection.Clear();
            foreach (AudioSource each in audioArray)
            {
                if (each.isPlaying)
                {
                    //Debug.Log("Stop audio : " + each.name + " Time : " + each.time);
                    collection.Add(each, each.time);
                    each.Stop();
                }
            }
        }

        private void ResumeAudio()
        {
            StartCoroutine(ResumeAudioInternal());
        }

        private IEnumerator ResumeAudioInternal()
        {
            yield return new WaitForSeconds(0.1f);

            foreach (AudioSource each in collection.Keys)
            {
                each.time = collection[each];
                each.Play();
                //Debug.Log("Resume audio : " + each.name + " Time : " + each.time + " Mute ? : " + each.mute + " Is Playing : " + each.isPlaying);
            }
            collection.Clear();
        }

        private NativeVideoQualityType GetNativeVideoQualityType(VideoQuality videoQuality)
        {
            switch(videoQuality)
            {
                case VideoQuality.QUALITY_MATCH_SCREEN_SIZE:
                    return NativeVideoQualityType.QUALITY_MATCH_SCREEN_SIZE;

                case VideoQuality.QUALITY_1080P:
                    return NativeVideoQualityType.QUALITY_1080P;

                case VideoQuality.QUALITY_720P:
                    return NativeVideoQualityType.QUALITY_720P;

                case VideoQuality.QUALITY_480P:
                    return NativeVideoQualityType.QUALITY_480P;

                default:
                    throw new System.Exception("Not implemented on native : " + videoQuality);

            }
        }

        private static AndroidJavaObject GetUnityActivity()
        {
            var     jc  = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            return jc.GetStatic<AndroidJavaObject>("currentActivity");
        }

        #endregion

        void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus && IsRecording())
            {
                Plugin.Call(Native.Methods.STOP_RECORDING);
            }
        }
    }
}
#endif
