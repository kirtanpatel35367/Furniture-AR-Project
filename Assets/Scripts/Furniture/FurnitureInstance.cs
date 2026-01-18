using UnityEngine;

public class FurnitureInstance : MonoBehaviour
{
    [Header("Hierarchy")]
    [SerializeField] private Transform scaleRoot;   // PINCH
    [SerializeField] private Transform visualRoot;  // HIGHLIGHT

    private Vector3 visualBaseScale;
    private bool isSelected;

    private void Awake()
    {
        if (scaleRoot == null || visualRoot == null)
        {
            Debug.LogError($"FurnitureInstance setup missing on {name}");
            return;
        }

        visualBaseScale = visualRoot.localScale;
    }

    // ---------------- HIGHLIGHT ----------------
    public void SetSelected(bool selected)
    {
        if (isSelected == selected)
            return;

        isSelected = selected;

        visualRoot.localScale = selected
            ? visualBaseScale * 1.2f
            : visualBaseScale;
    }

    // ---------------- ROTATION ----------------
    public void RotateLocal(float angle)
    {
        transform.Rotate(Vector3.up, angle, Space.Self);
    }

    // ---------------- PINCH SCALE ----------------
    public void ScaleModel(float factor, float min, float max)
    {
        Vector3 target = scaleRoot.localScale * factor;
        float clamped = Mathf.Clamp(target.x, min, max);
        scaleRoot.localScale = Vector3.one * clamped;
    }

    // ---------------- REMOVE ----------------
    public void Remove()
    {
        Destroy(gameObject);
    }
}
