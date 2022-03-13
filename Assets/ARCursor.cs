using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARCursor : MonoBehaviour
{
    [SerializeField] private GameObject cursorChildObject;
    [SerializeField] private GameObject objectToPlace;
    [SerializeField] private ARRaycastManager arRaycastManager;
    [SerializeField] private bool useCursor = true;

    private void Start()
    {
        cursorChildObject.SetActive(useCursor);
    }

    private void Update()
    {
        if (useCursor)
        {
            UpdateCursor();
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (useCursor)
            {
                var transform1 = transform;
                GameObject.Instantiate(objectToPlace, transform1.position, transform1.rotation);
            }
            else
            {
                List<ARRaycastHit> hits = new List<ARRaycastHit>();
                arRaycastManager.Raycast(Input.GetTouch(0).position, hits,
                    UnityEngine.XR.ARSubsystems.TrackableType.Planes);
                if (hits.Count > 0)
                {
                    GameObject.Instantiate(objectToPlace, hits[0].pose.position, hits[0].pose.rotation);
                }
            }
        }
    }

    void UpdateCursor()
    {
        if (Camera.main is { })
        {
            Vector2 screenPosition = Camera.main.ViewportToScreenPoint(new Vector2(0.5f, 0.5f));
            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            arRaycastManager.Raycast(screenPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);

            if (hits.Count > 0)
            {
                var transform1 = transform;
                transform1.position = hits[0].pose.position;
                transform1.rotation = hits[0].pose.rotation;
            }
        }
    }
}