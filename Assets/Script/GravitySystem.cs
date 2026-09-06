using System.Collections;
using UnityEngine;

public class GravitySystem : MonoBehaviour
{
    private PlayerControl playerControl;
    public Camera mainCamera;
    public bool gravity = true;

    private bool prevGravityState = true;

    void Start()
    {
        playerControl = FindFirstObjectByType<PlayerControl>();
    }

    void Update()
    {
        transform.position = new Vector3(playerControl.transform.position.x, transform.position.y, transform.position.z);

        playerControl.rb.useGravity = gravity;

        if (prevGravityState && gravity == false) StartCoroutine(ShakeAndDisable(1f, 0.5f));
        if (gravity) gameObject.SetActive(true);

        prevGravityState = gravity;
    }

    IEnumerator ShakeAndDisable(float duration, float magnitude)
    {
        Vector3 originalPos = mainCamera.transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;

            mainCamera.transform.localPosition = originalPos + new Vector3(offsetX, offsetY, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.localPosition = originalPos;
        gameObject.SetActive(false);
    }
}