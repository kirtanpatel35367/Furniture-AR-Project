using UnityEngine;

public class FurnitureSelectionManager : MonoBehaviour
{
    [SerializeField] private ARPlacementController placementController;
    [SerializeField] private FurnitureUIManager uiManager;

    [Header("Furniture Prefabs (AR Roots)")]
    [SerializeField] private GameObject sofaPrefab;
    [SerializeField] private GameObject tablePrefab;
    [SerializeField] private GameObject bedPrefab;
    [SerializeField] private GameObject cupboardPrefab;

    public void SelectSofa() => Select(sofaPrefab);
    public void SelectTable() => Select(tablePrefab);
    public void SelectBed() => Select(bedPrefab);
    public void SelectCupboard() => Select(cupboardPrefab);

    private void Select(GameObject prefab)
    {
        if (prefab == null)
            return;

        placementController.SetSelectedPrefab(prefab);
        uiManager.HidePanel();
    }
}
