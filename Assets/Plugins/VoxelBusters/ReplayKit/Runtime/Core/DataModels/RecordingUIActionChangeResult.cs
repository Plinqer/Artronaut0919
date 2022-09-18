using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.ReplayKit
{
    public class RecordingUIActionChangeResult
    {
        #region Properties

        /// <summary>
        /// Contains the state value.
        /// </summary>
        public RecordingUIAction Action { get; private set; }

        #endregion

        #region Constructors

        public RecordingUIActionChangeResult(RecordingUIAction action)
        {
            // set properties
            Action      = action;
        }

        #endregion
    }
}