using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DilmerGames.Core.Singletons;

[RequireComponent(typeof(ARAnchorManager))]
public class ARDrawManager : Singleton<ARDrawManager>
{
    [SerializeField]
    private LineSettings lineSettings = null;

    [SerializeField]
    private UnityEvent OnDraw = null;

    [SerializeField]
    private ARAnchorManager anchorManager;

    [SerializeField]
    private Camera arCamera = null;

    [SerializeField] Button colorbtn;
    private List<ARAnchor> anchors = new List<ARAnchor>();
    private Pose pose;
    private Dictionary<int, ARLine> Lines = new Dictionary<int, ARLine>();

    public bool CanDraw { get; set; }
    private List<RaycastResult> raycastResults = new List<RaycastResult>();
    void Update()
    {
#if !UNITY_EDITOR
        DrawOnTouch();
#else
        DrawOnMouse();
#endif
    }

    public void AllowDraw(bool isAllow)
    {
        CanDraw = isAllow;
    }


    void DrawOnTouch()
    {
        if (!CanDraw) return;

        int tapCount = Input.touchCount > 1 && lineSettings.allowMultiTouch ? Input.touchCount : 1;

        for (int i = 0; i < tapCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            Vector3 touchPosition = arCamera.ScreenToWorldPoint(new Vector3(Input.GetTouch(i).position.x, Input.GetTouch(i).position.y, lineSettings.distanceFromCamera));

            ARDebugManager.Instance.LogInfo($"{touch.fingerId}");

            if (touch.phase == TouchPhase.Began && !IsPointOverUI(Input.mousePosition) && !IsPointOverUI(touchPosition))
            {
                OnDraw?.Invoke();

                ARAnchor anchor = anchorManager.AddAnchor(new Pose(touchPosition, Quaternion.identity));
                if (anchor == null)
                    Debug.LogError("Error creating reference point");
                else
                {
                    anchors.Add(anchor);
                    ARDebugManager.Instance.LogInfo($"Anchor created & total of {anchors.Count} anchor(s)");
                }

                ARLine line = new ARLine(lineSettings);
                Lines.Add(0, line);
                line.AddNewLineRenderer(transform, null, touchPosition);
                colorbtn.image.color = lineSettings.endColor;

            }
            else if (touch.phase == TouchPhase.Moved && !IsPointOverUI(Input.mousePosition) || touch.phase == TouchPhase.Stationary && !IsPointOverUI(Input.mousePosition))
            {
                Lines[touch.fingerId].AddPoint(touchPosition);
            }
            else if (IsPointOverUI(Input.mousePosition))
            {
                Debug.Log("nothing");
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                Lines.Remove(touch.fingerId);
            }
        }
    }

    void DrawOnMouse()
    {
        if (!CanDraw) return;

        Vector3 mousePosition = arCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, lineSettings.distanceFromCamera));

        if (Input.GetMouseButton(0))
        {
            OnDraw?.Invoke();

            if (Lines.Keys.Count == 0 && !IsPointOverUI(Input.mousePosition))
            {
                ARLine line = new ARLine(lineSettings);
                Lines.Add(0, line);
                line.AddNewLineRenderer(transform, null, mousePosition);
                colorbtn.image.color = lineSettings.endColor;
            }

            else if (IsPointOverUI(Input.mousePosition))
            {
                Debug.Log("nothing");
            }

            else
            {
                Lines[0].AddPoint(mousePosition);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Lines.Remove(0);
        }
    }

    GameObject[] GetAllLinesInScene()
    {
        return GameObject.FindGameObjectsWithTag("Line");
    }

    public void ClearLines()
    {
        GameObject[] lines = GetAllLinesInScene();
        foreach (GameObject currentLine in lines)
        {
            LineRenderer line = currentLine.GetComponent<LineRenderer>();
            Destroy(currentLine);
        }
    }
    private bool IsPointOverUI(Vector2 fingerPosition)
    {
        PointerEventData eventDataPosition = new PointerEventData(EventSystem.current);
        eventDataPosition.position = fingerPosition;
        EventSystem.current.RaycastAll(eventDataPosition, raycastResults);
        return raycastResults.Count > 0;
    }
}