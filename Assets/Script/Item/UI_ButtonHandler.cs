using UnityEngine;

public class UI_ButtonHandler : MonoBehaviour
{
    public void OnClickAddHealthItem()
    {
        ItemManager.Instance.AddItem("체력 회복 아이템");
    }

    public void OnClickAddBreakItem()
    {
        ItemManager.Instance.AddItem("장애물 파괴 아이템");
    }

    public void OnClickAddBoostItem()
    {
        ItemManager.Instance.AddItem("빠른 스타트 아이템");
    }
}