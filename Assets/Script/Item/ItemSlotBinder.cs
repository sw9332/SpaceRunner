using UnityEngine;

public class ItemSlotBinder : MonoBehaviour
{
    public GameObject[] sceneItemUI; // æ¿¿« ΩΩ∑‘ UI ø¿∫Í¡ß∆ÆµÈ

    private void Start()
    {
        if (ItemManager.Instance != null)
        {
            ItemManager.Instance.SetItemUI(sceneItemUI);
        }
    }
}