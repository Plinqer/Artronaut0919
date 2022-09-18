using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.ReplayKit
{
    public class RecordingStateChangeResult
    {
        #region Properties

        /// <summary>
        /// Contains the state value.
        /// </summary>
        public ReplayKitRecordingState State { get; private set; }

        #endregion

        #region Constructors

        public RecordingStateChangeResult(ReplayKitRecordingState state)
        {
            // set properties
            State       = state;
        }

        #endregion
    }
}