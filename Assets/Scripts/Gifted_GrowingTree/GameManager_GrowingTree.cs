using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager_GrowingTree : MonoBehaviour
{
    [SerializeField] AudioSource bg;
    public enum Stage
    { one, two, three, four, five, six, seven };
    private bool spawned = false;
    public GameObject instantiatedObj;
    public GameObject spawnedPos;
    public Camera ARcamera;
    public Stage stage;

    private List<RaycastResult> raycastResults = new List<RaycastResult>();
    // Start is called before the first frame update
    void Start()
    {
        bg.Play();
        stage= Stage.one;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("works!");

            Ray ray = ARcamera.ScreenPointToRay(Input.mousePosition);
            Debug.Log(ray);

            if (IsPointOverUI(Input.mousePosition))
            {
                Debug.Log("nothing");
            }
            else if(!spawned)
            {
                spawnedPos = Instantiate(instantiatedObj, ray.origin, Quaternion.identity);
                spawned = true;
            }
            else if (spawned)
            {
                Debug.Log("Already Spawned");
            }
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
