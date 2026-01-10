using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class FurnitureGestureController : MonoBehaviour
{
    [Header("AR")]
    [SerializeField] private ARRaycastManager raycastManager;

    [Header("Scale Limits")]
    [SerializeField] private float minScale = 0.5f;
    [SerializeField] private float maxScale = 2.0f;

    [Header("Long Press Remove")]
    [SerializeField] private float longPressDuration = 0.7f;

    private FurnitureInstance selectedFurniture;
    private float longPressTimer;
    private bool isDragging;

    private static readonly List<ARRaycastHit> hits = new();

    void Update()
    {
        if (Input.touchCount == 0)
            return;

        if (Input.touchCount == 1)
        {
            HandleSingleTouch(Input.GetTouch(0));
        }
        else if (Input.touchCount == 2 && selectedFurniture != null)
        {
            HandleTwoFingerGesture();
        }
    }

    // -------------------------------
    // SINGLE TOUCH: SELECT / MOVE / REMOVE
    // -------------------------------
    private void HandleSingleTouch(Touch touch)
    {
        switch (touch.phase)
        {
            case TouchPhase.Began:
                isDragging = false;
                longPressTimer = 0f;
                TrySelectFurniture(touch);
                break;

            case TouchPhase.Moved:
                if (selectedFurniture == null)
                    return;

                isDragging = true;
                longPressTimer = 0f;
                MoveSelectedFurniture(touch);
                break;

            case TouchPhase.Stationary:
                if (selectedFurniture != null && !isDragging)
                {
                    longPressTimer += Time.deltaTime;
                    if (longPressTimer >= longPressDuration)
                    {
                        RemoveSelectedFurniture();
                        longPressTimer = 0f;
                    }
                }
                break;

            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                longPressTimer = 0f;
                isDragging = false;
                break;
        }
    }

    private void TrySelectFurniture(Touch touch)
    {
        Ray ray = Camera.main.ScreenPointToRay(touch.position);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            FurnitureInstance instance =
                hit.collider.GetComponentInParent<FurnitureInstance>();

            if (instance == null)
                return;

            if (selectedFurniture != null && selectedFurniture != instance)
                selectedFurniture.SetSelected(false);

            selectedFurniture = instance;
            selectedFurniture.SetSelected(true);
        }
    }

    private void MoveSelectedFurniture(Touch touch)
    {
        if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose pose = hits[0].pose;
            Vector3 newPos = selectedFurniture.transform.position;
            newPos.x = pose.position.x;
            newPos.z = pose.position.z;
            selectedFurniture.transform.position = newPos;
        }
    }

    // -------------------------------
    // TWO FINGER: ROTATE OR SCALE (INTENT BASED)
    // -------------------------------
    private void HandleTwoFingerGesture()
    {
        Touch t0 = Input.GetTouch(0);
        Touch t1 = Input.GetTouch(1);

        if (t0.phase != TouchPhase.Moved && t1.phase != TouchPhase.Moved)
            return;

        float prevDist = ((t0.position - t0.deltaPosition) -
                          (t1.position - t1.deltaPosition)).magnitude;
        float currDist = (t0.position - t1.position).magnitude;

        float distDelta = Mathf.Abs(currDist - prevDist);

        // 👉 INTENT DETECTION
        if (distDelta > 5f)
        {
            HandleScale(prevDist, currDist);
        }
        else
        {
            HandleRotation(t0, t1);
        }
    }

    private void HandleRotation(Touch t0, Touch t1)
    {
        Vector2 prevDir =
            (t0.position - t0.deltaPosition) -
            (t1.position - t1.deltaPosition);

        Vector2 currDir = t0.position - t1.position;

        float angle = Vector2.SignedAngle(prevDir, currDir);

        // ✅ LOCAL ROTATION (CRITICAL FIX)
        selectedFurniture.RotateLocal(angle);
    }

    private void HandleScale(float prevDist, float currDist)
    {
        if (Mathf.Approximately(prevDist, 0))
            return;

        float scaleFactor = currDist / prevDist;
        selectedFurniture.ScaleModel(scaleFactor, minScale, maxScale);
    }

    // -------------------------------
    // REMOVE
    // -------------------------------
    public void RemoveSelectedFurniture()
    {
        if (selectedFurniture != null)
        {
            selectedFurniture.Remove();
            selectedFurniture = null;
        }
    }
}
