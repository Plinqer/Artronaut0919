using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.ReplayKit;

namespace VoxelBusters.ReplayKit.Demo
{
    public class ReplayKitUtility : MonoBehaviour 
    {
        private void OnEnable()
        {
            ReplayKitManager.DidInitialise              += DidInitialise;
            ReplayKitManager.DidRecordingStateChange    += DidRecordingStateChange;
        }

        private void OnDisable()
        {
            ReplayKitManager.DidInitialise              -= DidInitialise;
            ReplayKitManager.DidRecordingStateChange    -= DidRecordingStateChange;
        }

        public void Initialise()
        {
            if (ReplayKitManager.IsRecordingAPIAvailable())
            {
                ReplayKitManager.Initialise();
            }
            else
            {
                Debug.LogError("Recording API not available!");
            }
        }

        public void StartRecording()
        {
            if (ReplayKitManager.IsRecordingAPIAvailable())
            {
                ReplayKitManager.SetMicrophoneStatus(true);
                ReplayKitManager.StartRecording();
            }
            else
            {
                Debug.LogError("Recording API not available!");
            }
        }

        public void StopRecording()
        {
            ReplayKitManager.StopRecording();
        }

        public void PreviewRecording()
        {
            if (!ReplayKitManager.IsPreviewAvailable())
            {
                Debug.Log("Either nothing recorded or Not yet ReplayKitRecordingState.Available received!");
            }
            ReplayKitManager.Preview();
        }

        public void DiscardRecording()
        {
            ReplayKitManager.Discard();
        }

        private void DidInitialise(InitialiseCompleteResult result, Error error)
        {
            switch(result.State)
            {
                case ReplayKitInitialisationState.Success:
                    Debug.Log("Initialised Successfully");
                    break;

                case ReplayKitInitialisationState.Failed:
                    Debug.Log("Failed Initialisation");
                    break;
            }
        }

        void DidRecordingStateChange(RecordingStateChangeResult result, Error error)
        {
            Debug.Log("Received Event Callback : DidRecordingStateChangeEvent [State:" + result.ToString() + " " + "Error:" + error);

            switch (result.State)
            {
                case ReplayKitRecordingState.Started:
                    Debug.Log("ReplayKitManager.DidRecordingStateChangeEvent : Video Recording Started");
                    break;

                case ReplayKitRecordingState.Stopped:
                    Debug.Log("ReplayKitManager.DidRecordingStateChangeEvent : Video Recording Stopped");
                    break;

                case ReplayKitRecordingState.Failed:
                    Debug.Log("ReplayKitManager.DidRecordingStateChangeEvent : Video Recording Failed with message [" + error + "]");
                    break;

                case ReplayKitRecordingState.Available:
                    Debug.Log("ReplayKitManager.DidRecordingStateChangeEvent : Video Recording available for preview");
                    break;

                default:
                    Debug.Log("Unknown State");
                    break;
            }
        }

    }
}
