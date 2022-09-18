using UnityEngine;

namespace VoxelBusters.ReplayKit
{
    [System.Serializable]
    public class InitialisationSettings
    {
        #region Fields

        [SerializeField]
        private     bool    m_requestScreenRecordPermissionOnInitialise = false;

        [SerializeField]
        private     bool    m_requestMicrophonePermissionOnInitialise   = false;

        #endregion

        #region Properties

        public bool RequestScreenRecordPermissionOnInitialise => m_requestScreenRecordPermissionOnInitialise;

        public bool RequestMicrophonePermissionOnInitialise => m_requestMicrophonePermissionOnInitialise;

        #endregion

        #region Constructors

        public InitialisationSettings(bool requestScreenRecordPermissionOnInitialise = false, bool requestMicrophonePermissionOnInitialise = false)
        {
            // set properties
            m_requestScreenRecordPermissionOnInitialise = requestScreenRecordPermissionOnInitialise;
            m_requestMicrophonePermissionOnInitialise   = requestMicrophonePermissionOnInitialise;
        }

        #endregion
    }
}