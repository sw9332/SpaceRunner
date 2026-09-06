using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EarthStageManager : MonoBehaviour
{
    private static EarthStageManager instance = null;

    public static EarthStageManager Instance { get { return instance; } }

    void Awake()
    {
        if (instance == null) { instance = this; }
        else Destroy(this.gameObject);
    }

    private SpaceTrashSpawner spaceTrashSpawner;

    public GameObject GameOverUI;
    public GameObject GameOverBackground;
    public AudioSource BGM;
    public AudioSource bossBGM;
    
    public Text coinText;

    public float step_timer = 0.0f; // 경과 시간을 유지

    public bool gameOver = false;

    public GameObject jumpUI;

    public GameObject upUI;
    public GameObject downUI;

    public GameObject q;
    public GameObject w;
    public GameObject e;

    bool ui = false;

    public void RePlayButton()
    {
        if (!string.IsNullOrEmpty(ItemManager.Instance.itemSlot[0]) &&
            !string.IsNullOrEmpty(ItemManager.Instance.itemSlot[1]) &&
            !string.IsNullOrEmpty(ItemManager.Instance.itemSlot[2]))
        {
            SceneManager.LoadScene("Earth_Stage_Scene");
        }

        else StartCoroutine(RePlay());
    }

    public IEnumerator RePlay()
    {
        MainManager.autoReadyAfterLoad = true;
        SceneManager.LoadScene("MainScene");
        yield return null;
    }

    public void ExitButton()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void GameClear()
    {
        Coin.Instance.coinSave = Coin.Instance.coin;
        PlayerPrefs.SetInt("Coin", Coin.Instance.coinSave);
        PlayerPrefs.SetInt("Space Trash", SpaceTrashSpawner.SpaceTrash);
        SceneManager.LoadScene("GameClearScene");
    }

    public float getPlayTime()
    {
        float time;
        time = this.step_timer;
        return (time); // 호출한 곳에 경과 시간을 알려줌
    }

    public void GameOver()
    {
        gameOver = true;
        Coin.Instance.coinSave = Coin.Instance.coin;
        PlayerPrefs.SetInt("Coin", Coin.Instance.coinSave);
        PlayerPrefs.SetInt("Space Trash", SpaceTrashSpawner.SpaceTrash);
        Time.timeScale = 0;
        if (!ui) { GameOverUI.GetComponent<UIAnimator>().Show(); GameOverBackground.SetActive(true); ui = true; }
    }

    private void KeyColorBlack(GameObject ui)
    {
        ui.GetComponent<Image>().color = Color.black;
    }

    private void KeyColorWhite(GameObject ui)
    {
        ui.GetComponent<Image>().color = Color.blue;
    }

    void Update()
    {
        FindFirstObjectByType<PlayerControl>().effectSound.volume = GameSetting.Instance.SFX.value;
        this.step_timer += Time.deltaTime; // 경과 시간을 더함
        coinText.text = "Star Coin : " + Coin.Instance.coin.ToString("N0");

        if (Input.GetKey(KeyCode.R) && gameOver) RePlayButton();

        if (Input.GetKeyDown(KeyCode.Escape)) { Time.timeScale = 0; GameSetting.Instance.GameSettingOpenOrClose(); }

        if (Input.GetKeyDown(KeyCode.Space)) KeyColorWhite(jumpUI);
        if (Input.GetKeyUp(KeyCode.Space)) KeyColorBlack(jumpUI);

        if (Input.GetKeyDown(KeyCode.UpArrow)) KeyColorWhite(upUI);
        if (Input.GetKeyUp(KeyCode.UpArrow)) KeyColorBlack(upUI);

        if (Input.GetKeyDown(KeyCode.DownArrow)) KeyColorWhite(downUI);
        if (Input.GetKeyUp(KeyCode.DownArrow)) KeyColorBlack(downUI);

        if (Input.GetKeyDown(KeyCode.Q)) KeyColorWhite(q);
        if(Input.GetKeyUp(KeyCode.Q)) KeyColorBlack(q);

        if (Input.GetKeyDown(KeyCode.W)) KeyColorWhite(w);
        if (Input.GetKeyUp(KeyCode.W)) KeyColorBlack(w);

        if (Input.GetKeyDown(KeyCode.E)) KeyColorWhite(e);
        if (Input.GetKeyUp(KeyCode.E)) KeyColorBlack(e);
    }

    void Start()
    {
        spaceTrashSpawner = FindFirstObjectByType<SpaceTrashSpawner>();

        Time.timeScale = 1;
        BGM.Play();
        bossBGM.volume = GameSetting.Instance.BGM.value;
        gameOver = false;
        GameManager.Instance.gameStep = "Earth Stage";
        SceneManager.LoadScene("GameSetting", LoadSceneMode.Additive);
    }
}