using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject furniturePanel;
    public Button floatingPlusBtn;
    public Button sofaBtn;

    [Header("Furniture Prefabs")]
    public GameObject sofaPrefab;

    [HideInInspector]
    public GameObject selectedFurniture;

    void Start()
    {
        furniturePanel.SetActive(false);

        floatingPlusBtn.onClick.AddListener(ToggleFurniturePanel);
        sofaBtn.onClick.AddListener(() => SelectFurniture(sofaPrefab));
    }

    void ToggleFurniturePanel()
    {
        furniturePanel.SetActive(!furniturePanel.activeSelf);
    }

    void SelectFurniture(GameObject prefab)
    {
        selectedFurniture = prefab;
        furniturePanel.SetActive(false);
    }

    public static bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject(0);
    }
}
