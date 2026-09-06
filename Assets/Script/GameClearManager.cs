using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameClearManager : MonoBehaviour
{
    public AudioSource BGM;
    public Animator animator;
    public Text clearText;

    void Start()
    {
        SceneManager.LoadScene("GameSetting", LoadSceneMode.Additive);

        GameManager.Instance.gameStep = "Clear";
        BGM.volume = GameSetting.Instance.BGM.value;
        animator.Play("Wave Hip Hop Dance");
        StartCoroutine(ClearTextSizeEffect());
    }

    public void ExitButton()
    {
        SceneManager.LoadScene("MainScene");
    }

    IEnumerator ClearTextSizeEffect()
    {
        int startSize = 260;
        int targetSize = 130;
        float duration = 0.3f; // 줄어드는 전체 시간
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // 부드러운 이징 효과 (조금씩 빨라졌다가 느려짐)
            float eased = Mathf.SmoothStep(0, 0.5f, t);

            int currentSize = Mathf.RoundToInt(Mathf.Lerp(startSize, targetSize + 20, eased));
            clearText.fontSize = currentSize;

            yield return null;
        }

        // 마지막 쾅! 느낌으로 빠르게 줄임
        yield return new WaitForSeconds(0.05f);
        clearText.fontSize = targetSize + 15;

        yield return new WaitForSeconds(0.05f);
        clearText.fontSize = targetSize;
    }
}