using UnityEngine;
using UnityEngine.UI;

public class MainSceneUIConnector : MonoBehaviour
{
    public GameObject[] itemUISlots;
    public GameObject itemBuyUI_Background;
    public GameObject itemBuyUI;
    public GameObject coinChortageUI;
    public Text confirmText;

    public Button yesButton;  // Yes 버튼
    public Button noButton;   // No 버튼
    public Button okButton;

    void Start()
    {
        if (ItemManager.Instance != null)
        {
            // UI 연결
            ItemManager.Instance.SetItemUI(itemUISlots);
            ItemManager.Instance.itemBuyUI_Background = itemBuyUI_Background;
            ItemManager.Instance.itemBuyUI = itemBuyUI;
            ItemManager.Instance.coinChortageUI = coinChortageUI;
            ItemManager.Instance.confirmText = confirmText;

            // 슬롯 버튼 연결
            for (int i = 0; i < itemUISlots.Length; i++)
            {
                int index = i;
                Button btn = itemUISlots[i]?.GetComponent<Button>();
                if (btn != null)
                {
                    btn.onClick.RemoveAllListeners();
                    btn.onClick.AddListener(() => ItemManager.Instance.UseItem(index));
                }
            }

            // Yes/No/Ok 버튼 이벤트 연결
            if (yesButton != null)
            {
                yesButton.onClick.RemoveAllListeners();
                yesButton.onClick.AddListener(ItemManager.Instance.OnConfirmYes);
            }

            if (noButton != null)
            {
                noButton.onClick.RemoveAllListeners();
                noButton.onClick.AddListener(ItemManager.Instance.OnConfirmNo);
            }

            if (okButton != null)
            {
                okButton.onClick.RemoveAllListeners();
                okButton.onClick.AddListener(ItemManager.Instance.OnConfirmOk);
            }
        }
    }
}