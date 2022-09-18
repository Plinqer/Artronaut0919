using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using VoxelBusters.CoreLibrary;
using VoxelBusters.ReplayKit;

namespace VoxelBusters.ReplayKit.Demo
{
    public class ReplayKitDemo : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private     Text        m_statusText    = null;

        #endregion

        #region Unity Life Cycle

        private void OnEnable()
        {
            SetStatus("Registered for ReplayKit Callbacks");
            ReplayKitManager.DidInitialise             += DidInitialiseEvent;
            ReplayKitManager.DidRecordingStateChange   += DidRecordingStateChangeEvent;
        }

        private void OnDisable()
        {
            ReplayKitManager.DidInitialise             -= DidInitialiseEvent;
            ReplayKitManager.DidRecordingStateChange   -= DidRecordingStateChangeEvent;
        }

        #endregion

        #region Initialise

        public void Initialise()
        {
			ReplayKitManager.Initialise();
        }

        #endregion

        #region Query

        public void IsRecordingAPIAvailable()
        {
            bool isAvailable = ReplayKitManager.IsRecordingAPIAvailable();
            SetStatus($"Recording API Available: {isAvailable}");
        }


        public void IsRecording()
        {
            bool isRecording = ReplayKitManager.IsRecording();
            SetStatus($"Is currently recording: {isRecording}");
        }

        public void IsPreviewAvailable()
        {
            bool isPreviewAvailable = ReplayKitManager.IsPreviewAvailable();
            SetStatus($"Is preview available: {isPreviewAvailable}");
        }

        public void GetPreviewFilePath()
        {
            string filePath = ReplayKitManager.GetPreviewFilePath();
            SetStatus($"Recorded video preview file path: {filePath}");
        }

        #endregion

        #region Utilities

        public void SavePreview() //Saves preview to gallery
        {
            if (ReplayKitManager.IsPreviewAvailable())
            {
                ReplayKitManager.SavePreview((result, error) =>
                {
                    if (error == null)
                    {
                        SetStatus($"Saved preview to gallery.");
                    }
                    else
                    {
                        SetStatus($"Saved preview to gallery failed with error: {error}");
                    }
                });
            }
            else
            {
                SetStatus("Preview recording not available. If you have already recoded, wait for ReplayKitRecordingState.Available status event and try saving again.");
            }
        }

        public void SharePreview()
        {
            if(ReplayKitManager.IsPreviewAvailable())
            {
                ReplayKitManager.SharePreview();
                SetStatus("Shared video preview");
            }
        }

        #endregion

        #region Recording

        public void SetMicrophoneStatus()
        {
            ReplayKitManager.SetMicrophoneStatus(true);
        }

        public void PrepareRecording()
        {
            ReplayKitManager.PrepareRecording((result, error) =>
            {
                if (error == null)
                {
                    SetStatus("Prepare recording successfull.");
                }
                else
                {
                    SetStatus($"Prepare recording failed with error : {error}");
                }
            });
        }

        public void StartRecording()
        {
            ReplayKitManager.StartRecording();
        }

        public void StopRecording()
        {
            ReplayKitManager.StopRecording((result, error) => {
                if (error == null)
                {
                    Debug.Log($"File path: {result.Path}");
                }
                else
                {
                    Debug.Log($"Failed with error: {error}");
                }
            });
        }

        public void Preview()
        {
            bool    success = ReplayKitManager.Preview();
            SetStatus($"Preview recording: {(success ? "Success" : "Failed")}");
        }

        public void Discard()
        {
            bool    success = ReplayKitManager.Discard();
            SetStatus($"Discard  recording: {(success ? "Success" : "Failed")}");
        }

        #endregion

        #region Event Callbacks

        private void DidInitialiseEvent(InitialiseCompleteResult result, Error error)
        {
            Debug.Log("Received Event Callback : DidInitialiseEvent [State:" + result.State.ToString() + " " + "Error:" + error);

            switch (result.State)
            {
                case ReplayKitInitialisationState.Success:
                    SetStatus("ReplayKitManager.DidInitialiseEvent : Initialisation Success");
                    break;

                case ReplayKitInitialisationState.Failed:
                    SetStatus("ReplayKitManager.DidInitialiseEvent : Initialisation Failed with message["+error+"]");
                    break;

                default:
                    SetStatus("Unknown State");
                    break;
            }
        }

        private void DidRecordingStateChangeEvent(RecordingStateChangeResult result, Error error)
        {
            Debug.Log("Received Event Callback : DidRecordingStateChangeEvent [State:" + result.State.ToString() + " " + "Error:" + error);

            switch(result.State)
            {
                case ReplayKitRecordingState.Started:
                    SetStatus("ReplayKitManager.DidRecordingStateChangeEvent : Video Recording Started");
                    break;

                case ReplayKitRecordingState.Stopped:
                    SetStatus("ReplayKitManager.DidRecordingStateChangeEvent : Video Recording Stopped");
                    break;

                case ReplayKitRecordingState.Failed:
                    SetStatus("ReplayKitManager.DidRecordingStateChangeEvent : Video Recording Failed with message [" + error +"]");
                    break;

                case ReplayKitRecordingState.Available:
                    SetStatus("ReplayKitManager.DidRecordingStateChangeEvent : Video Recording available for preview");
                    break;

                default:
                    SetStatus("Unknown State");
                    break;
            }
        }

        #endregion


        #region UI

        private void SetStatus(string message)
        {
            Debug.Log("[ReplayKit] " + message);
			Debug.Log("[m_statusText] " + m_statusText);

			if (m_statusText != null)
            {
                m_statusText.text = message;
            }
        }

        #endregion
    }
}