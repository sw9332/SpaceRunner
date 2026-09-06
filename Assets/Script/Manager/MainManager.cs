using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public Camera cameraObject;
    public AudioSource BGM;

    public Text coinText;
    public Text spaceTrashText;

    public GameObject unito;
    public GameObject mainScreen;
    public GameObject readyScreen;

    public GameObject mainUI;

    public GameObject methodUI;
    public GameObject keyUI;

    public static bool autoReadyAfterLoad = false;

    private static MainManager instance;

    public static MainManager Instance
    {
        get { return instance; }
    }

    private void Start()
    {
        SpaceTrashSpawner.SpaceTrash = PlayerPrefs.GetInt("Space Trash", 0);

        Time.timeScale = 1;
        BGM.Play();
        mainUI.GetComponent<UIAnimator>().Show();
        GameManager.Instance.gameStep = "Main";
        SceneManager.LoadScene("GameSetting", LoadSceneMode.Additive);

        if (autoReadyAfterLoad)
        {
            autoReadyAfterLoad = false;
            ReadyButton();
        }
    }

    private void Update()
    {
        coinText.text = "Star Coin : " + Coin.Instance.coin.ToString("N0");
        spaceTrashText.text = "Space Trash : " + SpaceTrashSpawner.SpaceTrash.ToString("N0");
    }

    readonly Vector3 readyUnitoEuler = new Vector3(0, 160, 0);
    readonly Vector3 readyCameraEuler = new Vector3(0, 70, 0);
    readonly Vector3 mainUnitoEuler = new Vector3(0, 180, 0);
    readonly Vector3 mainCameraEuler = new Vector3(0, 0, 0);


    public void ReadyButton()
    {
        mainScreen.SetActive(false);
        readyScreen.SetActive(true);
        //animator.Play("Ready UI");

        if (unitoRotateCoroutine != null) StopCoroutine(unitoRotateCoroutine);
        unitoRotateCoroutine = StartCoroutine(RotateSmoothDamp(unito.transform, readyUnitoEuler, 0.3f));

        if (cameraRotateCoroutine != null) StopCoroutine(cameraRotateCoroutine);
        cameraRotateCoroutine = StartCoroutine(RotateSmoothDamp(cameraObject.transform, readyCameraEuler, 0.3f));
    }

    public void BackButton()
    {
        mainScreen.SetActive(true);
        readyScreen.SetActive(false);

        if (unitoRotateCoroutine != null) StopCoroutine(unitoRotateCoroutine);
        unitoRotateCoroutine = StartCoroutine(RotateSmoothDamp(unito.transform, mainUnitoEuler, 0.3f));

        if (cameraRotateCoroutine != null) StopCoroutine(cameraRotateCoroutine);
        cameraRotateCoroutine = StartCoroutine(RotateSmoothDamp(cameraObject.transform, mainCameraEuler, 0.3f));
    }

    public void MethodUI()
    {
        if (!methodUI.activeSelf) methodUI.GetComponent<UIAnimator>().Show();
        else methodUI.GetComponent<UIAnimator>().Hide();
    }

    public void KeyUI()
    {
        if (!keyUI.activeSelf) keyUI.GetComponent<UIAnimator>().Show();
        else keyUI.GetComponent<UIAnimator>().Hide();
    }

    public void StartButton() { SceneManager.LoadScene("Loading"); }
    public void SettingButton() { GameSetting.Instance.GameSettingOpenOrClose(); }
    public void ExitButton() { Application.Quit(); }

    Coroutine unitoRotateCoroutine;
    Coroutine cameraRotateCoroutine;

    IEnumerator RotateSmoothDamp(Transform target, Vector3 targetEulerAngles, float smoothTime)
    {
        Vector3 velocity = Vector3.zero;
        Vector3 currentEuler = target.eulerAngles;

        while (true)
        {
            //currentEuler.x = Mathf.SmoothDampAngle(currentEuler.x, targetEulerAngles.x, ref velocity.x, smoothTime);
            currentEuler.y = Mathf.SmoothDampAngle(currentEuler.y, targetEulerAngles.y, ref velocity.y, smoothTime);
            currentEuler.z = Mathf.SmoothDampAngle(currentEuler.z, targetEulerAngles.z, ref velocity.z, smoothTime);

            target.rotation = Quaternion.Euler(currentEuler);

            if (Vector3.Distance(currentEuler, targetEulerAngles) < 0.1f &&
                Mathf.Abs(velocity.x) < 0.01f &&
                Mathf.Abs(velocity.y) < 0.01f &&
                Mathf.Abs(velocity.z) < 0.01f)
            {
                break;
            }

            yield return null;
        }

        target.rotation = Quaternion.Euler(targetEulerAngles);
    }
}