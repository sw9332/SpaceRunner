using System.Collections;
using UnityEngine;

public class GameItem : MonoBehaviour
{
    private PlayerControl playerControl;
    private Hp hp;
    public GameObject[] itemImageSlot;
    public GameObject boosterEffect;

    private void Start()
    {
        playerControl = FindFirstObjectByType<PlayerControl>();
        hp = FindFirstObjectByType<Hp>();

        if (ItemManager.Instance.itemSlot[0] == "체력 회복 아이템") itemImageSlot[0].SetActive(true);
        else if (ItemManager.Instance.itemSlot[0] == null || ItemManager.Instance.itemSlot[0] == "") itemImageSlot[0].SetActive(false);

        if (ItemManager.Instance.itemSlot[1] == "장애물 회피 아이템") itemImageSlot[1].SetActive(true);
        else if (ItemManager.Instance.itemSlot[1] == null || ItemManager.Instance.itemSlot[1] == "") itemImageSlot[1].SetActive(false);

        if (ItemManager.Instance.itemSlot[2] == "빠른 스타트 아이템") itemImageSlot[2].SetActive(true);
        else if (ItemManager.Instance.itemSlot[2] == null || ItemManager.Instance.itemSlot[2] == "") itemImageSlot[2].SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !EarthStageManager.Instance.gameOver) UseItem(0);
        if (Input.GetKeyDown(KeyCode.W) && !EarthStageManager.Instance.gameOver) UseItem(1);
        if (Input.GetKeyDown(KeyCode.E) && !EarthStageManager.Instance.gameOver) UseItem(2);
    }

    public void UseItem(int index)
    {
        if (!string.IsNullOrEmpty(ItemManager.Instance.itemSlot[index]))
        {
            Debug.Log($"아이템 {ItemManager.Instance.itemSlot[index]} 사용됨");
            ItemManager.Instance.UseItem(index);
            itemImageSlot[index].SetActive(false);

            switch(index)
            {
                case 0: hp.hpUI.value += 20; break;
                case 1: StartCoroutine(IgnoreObject()); break;
                case 2: StartCoroutine(SpeedBooster()); break;
            }
        }
    }

    public GameObject ignoreObject;

    IEnumerator IgnoreObject() // 장애물 회피 아이템
    {
        ignoreObject.transform.localScale = Vector3.zero;
        ignoreObject.transform.rotation = Quaternion.identity;
        ignoreObject.SetActive(true);

        float scaleDuration = 1f;
        float time = 0f;
        Vector3 targetScale = new Vector3(2.8f, 2.8f, 2.8f);

        while (time < scaleDuration)
        {
            time += Time.deltaTime;
            float t = time / scaleDuration;

            ignoreObject.transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, t);
            yield return null;
        }

        hp.invincibility = true;

        float rotateDuration = 10f;
        float rotateSpeed = 360f;
        float elapsed = 0f;

        while (elapsed < rotateDuration)
        {
            elapsed += Time.deltaTime;

            ignoreObject.transform.Rotate(Vector3.back * rotateSpeed * Time.deltaTime);

            yield return null;
        }

        hp.invincibility = false;
        ignoreObject.SetActive(false);
    }

    IEnumerator SpeedBooster() // 부스터 아이템
    {
        hp.invincibility = true;
        playerControl.isBoosting = true;
        playerControl.ACCELERATION = 35;
        playerControl.ani.Play("Boost");
        boosterEffect.SetActive(true);

        Rigidbody rb = playerControl.rb;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;

        float duration = 10f;
        float timer = 0f;

        float magnetRange = 8f;
        float magnetStrength = 20f;

        while (timer < duration)
        {
            Vector3 pos = playerControl.transform.position;

            float newY = Mathf.Lerp(pos.y, 4, 10f * Time.deltaTime);
            playerControl.transform.position = new Vector3(pos.x, newY, pos.z);

            rb.velocity = Vector3.zero;

            GameObject[] starCoins = GameObject.FindGameObjectsWithTag("StarCoin");
            foreach (GameObject coin in starCoins)
            {
                if (coin == null) continue;

                float dist = Vector3.Distance(coin.transform.position, playerControl.transform.position);
                if (dist < magnetRange)
                {
                    Vector3 direction = (playerControl.transform.position - coin.transform.position).normalized;
                    coin.transform.position += direction * magnetStrength * Time.deltaTime;
                }
            }

            timer += Time.deltaTime;
            yield return null;
        }

        playerControl.ACCELERATION = 15;
        playerControl.isBoosting = false;
        playerControl.jumpCount = 0;
        playerControl.rb.velocity = Vector3.zero;
        playerControl.velocity = Vector3.zero;
        playerControl.ani.Play("Down");
        boosterEffect.SetActive(false);
        hp.invincibility = false;
        rb.useGravity = true;
    }
}