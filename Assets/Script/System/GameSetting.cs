using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSetting : MonoBehaviour
{
    private static GameSetting instance = null;

    public static GameSetting Instance { get { return instance; } }

    private void Awake() { if (instance == null) instance = this; else Destroy(this.gameObject); }

    private FullScreenMode screenMode = FullScreenMode.FullScreenWindow;

    private int width = 1920;
    private int height = 1080;

    public GameObject GameSetting_Background;
    public GameObject GameSettingUI;
    public Camera Camera;
    public Slider BGM;
    public Slider SFX;

    public GameObject R1920x1080;
    public GameObject R1366x768;
    public GameObject R1280x720;

    public void GameSettingOpenOrClose()
    {
        if (GameSettingUI.activeSelf)
        {
            Time.timeScale = 1;
            GameSettingUI.GetComponent<UIAnimator>().Hide();
            GameSetting_Background.SetActive(false);
            PlayerPrefs.SetFloat("BGM", BGM.value);
            PlayerPrefs.SetFloat("SFX", SFX.value);
        }

        else
        {
            GameSettingUI.SetActive(true);
            GameSetting_Background.SetActive(true);
        }
    }

    public void Screen1920x1080()
    {
        width = 1920;
        height = 1080;

        R1920x1080.GetComponent<Image>().color = Color.white;
        R1366x768.GetComponent<Image>().color = Color.black;
        R1280x720.GetComponent<Image>().color = Color.black;
        Screen.SetResolution(width, height, screenMode);
    }

    public void Screen1366x768()
    {
        width = 1366;
        height = 768;

        R1920x1080.GetComponent<Image>().color = Color.black;
        R1366x768.GetComponent<Image>().color = Color.white;
        R1280x720.GetComponent<Image>().color = Color.black;
        Screen.SetResolution(width, height, screenMode);
    }

    public void Screen1280x720()
    {
        width = 1280;
        height = 720;

        R1920x1080.GetComponent<Image>().color = Color.black;
        R1366x768.GetComponent<Image>().color = Color.black;
        R1280x720.GetComponent<Image>().color = Color.white;
        Screen.SetResolution(width, height, screenMode);
    }

    public void Windowsed_ON()
    {
        screenMode = FullScreenMode.Windowed;
        Screen.SetResolution(width, height, screenMode);
    }

    public void Windowsed_OFF()
    {
        screenMode = FullScreenMode.FullScreenWindow;
        Screen.SetResolution(width, height, screenMode);
    }

    void Update()
    {
        switch (GameManager.Instance.gameStep)
        {
            case "Main": FindFirstObjectByType<MainManager>().BGM.volume = BGM.value; break;
            case "Earth Stage":
                FindFirstObjectByType<EarthStageManager>().BGM.volume = BGM.value;
                FindFirstObjectByType<EarthStageManager>().bossBGM.volume = BGM.value;
                break;
        }
    }

    void Start()
    {
        BGM.value = PlayerPrefs.GetFloat("BGM", 0.3f);
        SFX.value = PlayerPrefs.GetFloat("SFX", 0.3f);

        Camera.enabled = false;

        int currentWidth = Screen.currentResolution.width;
        int currentHeight = Screen.currentResolution.height;

        // 디버깅 로그 (선택 사항)
        //Debug.Log($"현재 해상도: {currentWidth}x{currentHeight}");

        if (currentWidth == 1920 && currentHeight == 1080)
        {
            R1920x1080.GetComponent<Image>().color = Color.white;
            R1366x768.GetComponent<Image>().color = Color.black;
            R1280x720.GetComponent<Image>().color = Color.black;
        }
        else if (currentWidth == 1366 && currentHeight == 768)
        {
            R1920x1080.GetComponent<Image>().color = Color.black;
            R1366x768.GetComponent<Image>().color = Color.white;
            R1280x720.GetComponent<Image>().color = Color.black;
        }
        else if (currentWidth == 1280 && currentHeight == 720)
        {
            R1920x1080.GetComponent<Image>().color = Color.black;
            R1366x768.GetComponent<Image>().color = Color.black;
            R1280x720.GetComponent<Image>().color = Color.white;
        }
        else
        {
            // 어떤 해상도도 일치하지 않는 경우 (기본 상태)
            R1920x1080.GetComponent<Image>().color = Color.black;
            R1366x768.GetComponent<Image>().color = Color.black;
            R1280x720.GetComponent<Image>().color = Color.black;
        }
    }
}