using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

    public string[] itemSlot = new string[3];          // 슬롯별 아이템 이름 저장
    private GameObject[] itemUI = new GameObject[3];  // 현재 씬의 아이템 UI 슬롯

    public GameObject itemBuyUI_Background; //구매 확인 팝업 배경
    public GameObject itemBuyUI;            // 구매 확인 팝업
    public GameObject coinChortageUI;       // 코인 부족 팝업
    public Text confirmText;                // 팝업 메시지 텍스트

    private int pendingIndex = -1;          // 구매 대기 인덱스
    private string pendingItemName = "";
    private int pendingItemCost = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadItemData();

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateItemVisuals();
    }

    public void SetItemUI(GameObject[] uiSlots)
    {
        itemUI = uiSlots;
        UpdateItemVisuals();
    }

    public void AddItem(string itemName)
    {
        switch (itemName)
        {
            case "체력 회복 아이템":
                ShowItemBuyPopup(0, "체력 회복 아이템", 1000);
                break;
            case "장애물 파괴 아이템":
                ShowItemBuyPopup(1, "장애물 회피 아이템", 2500);
                break;
            case "빠른 스타트 아이템":
                ShowItemBuyPopup(2, "빠른 스타트 아이템", 4000);
                break;
            default:
                Debug.LogWarning("알 수 없는 아이템 이름입니다.");
                break;
        }
    }

    private void ShowItemBuyPopup(int index, string itemLabel, int cost)
    {
        pendingIndex = index;
        pendingItemName = itemLabel;
        pendingItemCost = cost;

        confirmText.text = $"{itemLabel} ({cost}코인) 추가하시겠습니까?";
        itemBuyUI_Background.SetActive(true);
        itemBuyUI.GetComponent<UIAnimator>().Show();
    }

    public void OnConfirmYes()
    {
        if (Coin.Instance.coin < pendingItemCost)
        {
            coinChortageUI.SetActive(true);
            itemBuyUI.GetComponent<UIAnimator>().Hide();
            itemBuyUI_Background.SetActive(true);
            return;
        }

        if (!string.IsNullOrEmpty(itemSlot[pendingIndex]))
        {
            Debug.LogWarning("이미 해당 슬롯에 아이템이 있습니다.");
            itemBuyUI.GetComponent<UIAnimator>().Hide();
            itemBuyUI_Background.SetActive(false);
            return;
        }

        Coin.Instance.coin -= pendingItemCost;
        Coin.Instance.SaveCoin();

        itemSlot[pendingIndex] = pendingItemName;
        SaveItemData();
        UpdateItemVisuals();

        Debug.Log($"{pendingItemName} 구매 완료");

        itemBuyUI.GetComponent<UIAnimator>().Hide();
        itemBuyUI_Background.SetActive(false);
    }

    public void OnConfirmNo()
    {
        itemBuyUI.GetComponent<UIAnimator>().Hide();
        itemBuyUI_Background.SetActive(false);
    }

    public void OnConfirmOk()
    {
        coinChortageUI.GetComponent<UIAnimator>().Hide();
        itemBuyUI_Background.SetActive(false);
    }

    public void UseItem(int index)
    {
        if (string.IsNullOrEmpty(itemSlot[index]))
        {
            Debug.LogWarning("사용할 아이템이 없습니다.");
            return;
        }

        Debug.Log($"{itemSlot[index]} 사용됨");
        itemSlot[index] = "";
        SaveItemData();
        UpdateItemVisuals();
    }

    public void UpdateItemVisuals()
    {
        if (itemUI == null) return;

        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (i >= itemUI.Length) continue;

            if (itemUI[i] != null)
                itemUI[i].SetActive(!string.IsNullOrEmpty(itemSlot[i]));
        }
    }

    public void SaveItemData()
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            PlayerPrefs.SetString($"ItemSlot_{i}", itemSlot[i] ?? "");
        }
        PlayerPrefs.Save();
    }

    public void LoadItemData()
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            itemSlot[i] = PlayerPrefs.GetString($"ItemSlot_{i}", "");
        }
    }

    public void ClearAllItems()
    {
        for (int i = 0; i < itemSlot.Length; i++)
            itemSlot[i] = "";

        SaveItemData();
        UpdateItemVisuals();
    }
}