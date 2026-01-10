using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class ARPlacementController : MonoBehaviour
{
    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private ARAnchorManager anchorManager;
    [SerializeField] private ARPlaneManager planeManager;

    private GameObject selectedPrefab;
    private bool waitingForPlacement;

    private static readonly List<ARRaycastHit> hits = new();

    public void SetSelectedPrefab(GameObject prefab)
    {
        selectedPrefab = prefab;
        waitingForPlacement = true;

        Debug.Log("Placing prefab: " + selectedPrefab.name);
    }

    private void Update()
    {
        if (!waitingForPlacement || selectedPrefab == null)
            return;

        if (Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);
        if (touch.phase != TouchPhase.Began)
            return;

        if (!raycastManager.Raycast(
                touch.position,
                hits,
                TrackableType.PlaneWithinPolygon))
            return;

        Pose pose = hits[0].pose;

        // ==========================================
        // FACE THE AR CAMERA ON PLACEMENT (FIX)
        // ==========================================
        Transform cam = Camera.main.transform;
        Vector3 directionToCamera = cam.position - pose.position;
        directionToCamera.y = 0f;

        if (directionToCamera.sqrMagnitude > 0.001f)
        {
            pose.rotation = Quaternion.LookRotation(directionToCamera);
        }

        // ==========================================
        // GET PLANE & ATTACH ANCHOR
        // ==========================================
        ARPlane plane = planeManager.GetPlane(hits[0].trackableId);
        if (plane == null)
        {
            Debug.LogWarning("No plane found for anchor");
            return;
        }

        ARAnchor anchor = anchorManager.AttachAnchor(plane, pose);
        if (anchor == null)
        {
            Debug.LogWarning("Anchor creation failed");
            return;
        }

        Instantiate(selectedPrefab, anchor.transform);
        waitingForPlacement = false;
    }
}
