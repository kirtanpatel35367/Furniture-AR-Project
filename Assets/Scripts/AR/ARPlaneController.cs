using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARPlaneController : MonoBehaviour
{
    [SerializeField] private ARPlaneManager planeManager;
    [SerializeField] private float minPlaneSize = 0.5f;

    private void OnEnable()
    {
        planeManager.planesChanged += OnPlanesChanged;
    }

    private void OnDisable()
    {
        planeManager.planesChanged -= OnPlanesChanged;
    }

    private void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        foreach (var plane in args.added)
            ValidatePlane(plane);

        foreach (var plane in args.updated)
            ValidatePlane(plane);
    }

    private void ValidatePlane(ARPlane plane)
    {
        bool isValid =
            plane.size.x >= minPlaneSize &&
            plane.size.y >= minPlaneSize;

        plane.gameObject.SetActive(isValid);
    }

    public void LockPlanes()
    {
        planeManager.enabled = false;

        foreach (var plane in planeManager.trackables)
            plane.gameObject.SetActive(false);
    }
}
