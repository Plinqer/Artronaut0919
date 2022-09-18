using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.ReplayKit
{
    public class PrepareRecordingStateChangeResult
    {
        #region Properties

        /// <summary>
        /// Contains the state value.
        /// </summary>
        public ReplayKitPrepareRecordingState State { get; private set; }

        #endregion

        #region Constructors

        public PrepareRecordingStateChangeResult(ReplayKitPrepareRecordingState state)
        {
            // set properties
            State       = state;
        }

        #endregion
    }
}