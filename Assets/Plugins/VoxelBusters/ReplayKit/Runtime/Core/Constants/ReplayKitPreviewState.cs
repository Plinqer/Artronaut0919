using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.ReplayKit
{
    public enum ReplayKitPreviewState
    {
        /// <summary>
        /// State when preview opens successfully
        /// </summary>
        Opened = 0,

        /// <summary>
        /// State when preview is closed
        /// </summary>
        Closed,

        /// <summary>
        /// State when preview failes to open
        /// </summary>
        Failed,

        /// <summary>
        /// State when share button is clicked in the preview
        /// </summary>
        Shared,

        /// <summary>
        /// State when user started playing preview
        /// </summary>
        Played
    }
}