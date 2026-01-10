using UnityEngine;

public class FurnitureUIManager : MonoBehaviour
{
    [SerializeField] private GameObject furniturePanel;

    public void TogglePanel()
    {
        furniturePanel.SetActive(!furniturePanel.activeSelf);
    }

    public void HidePanel()
    {
        furniturePanel.SetActive(false);
    }
}
