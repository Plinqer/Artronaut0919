using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.ReplayKit
{
    public class RecordingPreviewAvailableResult
    {
        #region Properties

        /// <summary>
        /// String that describes the reason for the state, if any.
        /// </summary>
        public string Path { get; private set; }

        #endregion

        #region Constructors

        public RecordingPreviewAvailableResult(string path)
        {
            // set properties
            Path    = path;
        }

        #endregion
    }
}