using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.ReplayKit
{
    public class RecordingPreviewStateChangeResult
    {
        #region Properties

        /// <summary>
        /// Contains the state value.
        /// </summary>
        public ReplayKitPreviewState State { get; private set; }

        /// <summary>
        /// String that describes the reason for the state, if any.
        /// </summary>
        public string Message { get; private set; }

        #endregion

        #region Constructors

        public RecordingPreviewStateChangeResult(ReplayKitPreviewState state, string message)
        {
            // set properties
            State       = state;
            Message     = message;
        }

        #endregion
    }
}