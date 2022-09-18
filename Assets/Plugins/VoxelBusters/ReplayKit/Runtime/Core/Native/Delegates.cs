using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.ReplayKit
{
    public delegate void OnInitialiseCompleteInternalDelegate(ReplayKitInitialisationState state, Error error);

    public delegate void OnPrepareRecordingStateChangeInternalDelegate(ReplayKitPrepareRecordingState state, Error error);

    public delegate void OnRecordingStateChangeInternalDelegate(ReplayKitRecordingState state, Error error);

    public delegate void OnPreviewStateChangeInternalDelegate(ReplayKitPreviewState state, Error error);

    public delegate void OnRecordingUIActionChangeInternalDelegate(RecordingUIAction action, Error error);
}