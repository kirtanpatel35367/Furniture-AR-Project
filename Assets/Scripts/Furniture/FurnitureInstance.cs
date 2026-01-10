using UnityEngine;

public class FurnitureInstance : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform modelRoot;

    private Vector3 originalScale;

    private void Awake()
    {
        if (modelRoot != null)
            originalScale = modelRoot.localScale;
    }

    // ---------------- SELECTION ----------------
    public void SetSelected(bool selected)
    {
        if (modelRoot == null)
            return;

        modelRoot.localScale = selected
            ? originalScale * 1.05f
            : originalScale;
    }

    // ---------------- ROTATION (LOCAL) ----------------
    public void RotateLocal(float angle)
    {
        transform.Rotate(Vector3.up, angle, Space.Self);
    }

    // ---------------- SCALE ----------------
    public void ScaleModel(float factor, float min, float max)
    {
        if (modelRoot == null)
            return;

        Vector3 target = modelRoot.localScale * factor;
        float clamped = Mathf.Clamp(target.x, min, max);
        modelRoot.localScale = Vector3.one * clamped;
    }

    // ---------------- REMOVE ----------------
    public void Remove()
    {
        Destroy(gameObject);
    }
}
