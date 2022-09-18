using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.ReplayKit
{
    public class InitialiseCompleteResult
    {
        #region Properties

        /// <summary>
        /// Contains the state value.
        /// </summary>
        public ReplayKitInitialisationState State { get; private set; }

        #endregion

        #region Constructors

        public InitialiseCompleteResult(ReplayKitInitialisationState state)
        {
            // set properties
            State       = state;
        }

        #endregion
    }
}