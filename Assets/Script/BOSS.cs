using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BOSS : MonoBehaviour
{
    public Slider hp;
    public GameObject laserPrefab;
    public GameObject meteorPrefab;
    public Transform[] meteorSpawnPoints;
    public Transform player;

    public float gravityFieldForce = 5f;
    public float appearSpeed = 15f;

    private float initialFollowOffset = 65f;
    private float battleFollowOffset = 25f;
    private float currentFollowOffset;

    private bool isAppearing = false;
    private bool patternStarted = false;

    private GravitySystem gravitySystem;
    private MissionManager missionManager;
    private EarthStageManager earthStageManager;

    public Camera mainCamera;

    public float cameraTargetYRotation = 26f;
    public float cameraRotateSpeed = 2f;

    private void Start()
    {
        gravitySystem = FindFirstObjectByType<GravitySystem>();
        missionManager = FindObjectOfType<MissionManager>();
        earthStageManager = FindFirstObjectByType<EarthStageManager>();

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            else
            {
                Debug.LogError("BOSS: Player not found in scene!");
            }
        }

        hp.value = 100;
        hp.gameObject.SetActive(false);
        currentFollowOffset = initialFollowOffset;
    }

    private void Update()
    {
        if (player == null || isAppearing) return;

        Vector3 targetPos = new Vector3(player.position.x + currentFollowOffset, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 2f);

        if (!patternStarted && Mathf.Abs(transform.position.x - (player.position.x + battleFollowOffset)) < 0.2f)
        {
            patternStarted = true;
            StartCoroutine(BossPatternLoop());
        }
    }

    public void TriggerBossAppearance()
    {
        if (patternStarted) return;

        earthStageManager.BGM.Stop();
        earthStageManager.bossBGM.Play();
        hp.gameObject.SetActive(true);
        gravitySystem.gravity = false;
        currentFollowOffset = battleFollowOffset;

        EarthStageManager.Instance.upUI.SetActive(true);
        EarthStageManager.Instance.downUI.SetActive(true);

        StartCoroutine(HandleBossAppearance());
    }

    IEnumerator HandleBossAppearance()
    {
        yield return null;
        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(RotateCameraY(cameraTargetYRotation));
    }

    IEnumerator RotateCameraY(float targetYAngle)
    {
        Quaternion startRotation = mainCamera.transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(mainCamera.transform.eulerAngles.x, targetYAngle, mainCamera.transform.eulerAngles.z);

        float t = 0f;
        while (Quaternion.Angle(mainCamera.transform.rotation, targetRotation) > 0.1f)
        {
            t += Time.deltaTime * cameraRotateSpeed;
            mainCamera.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            yield return null;
        }

        mainCamera.transform.rotation = targetRotation;
    }

    private IEnumerator BossPatternLoop()
    {
        while (hp.value > 0)
        {
            int pattern = Random.Range(0, 11);
            switch (pattern)
            {
                case 0: yield return StartCoroutine(LaserSpreadVertical()); break;
                case 1: yield return StartCoroutine(LaserSpreadVertical()); break;
                case 2: yield return StartCoroutine(LaserSpreadVertical()); break;
                case 3: yield return StartCoroutine(LaserSpreadVertical()); break;
                case 4: yield return StartCoroutine(LaserSpreadVertical()); break;
                case 5: yield return StartCoroutine(MeteorRain()); break;
                case 6: yield return StartCoroutine(MeteorRain()); break;
                case 7: yield return StartCoroutine(MeteorRain()); break;
                case 8: yield return StartCoroutine(MeteorRain()); break;
                case 9: yield return StartCoroutine(MeteorRain()); break;
                case 10: yield return StartCoroutine(GravityField()); break;
            }

            yield return new WaitForSeconds(0.05f);
        }

        Die();
    }

    IEnumerator LaserSpreadVertical()
    {
        int bulletCount = Random.Range(3, 5);
        Vector3 spawnPos1 = transform.position + new Vector3(-1f, -4.5f, 0);
        Vector3 spawnPos2 = transform.position + new Vector3(-1f, 0.5f, 0);
        Vector3 spawnPos3 = transform.position + new Vector3(-1f, 4.5f, 0);

        for (int i = 0; i < bulletCount; i++)
        {
            Instantiate(laserPrefab, spawnPos1, Quaternion.LookRotation(Vector3.left));
            Instantiate(laserPrefab, spawnPos2, Quaternion.LookRotation(Vector3.left));
            Instantiate(laserPrefab, spawnPos3, Quaternion.LookRotation(Vector3.left));
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(2f);
    }

    IEnumerator MeteorRain()
    {
        for (int i = 0; i < 15; i++)
        {
            int randomIndex = Random.Range(0, meteorSpawnPoints.Length);
            Transform spawn = meteorSpawnPoints[randomIndex];
            Instantiate(meteorPrefab, spawn.position, Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.2f);
    }

    IEnumerator GravityField()
    {
        float pullTime = 2f;
        float timer = 0f;

        while (timer < pullTime)
        {
            timer += Time.deltaTime;
            Vector3 direction = (transform.position - player.position).normalized;
            player.position += direction * gravityFieldForce * Time.deltaTime;
            yield return null;
        }
    }

    public void TakeDamage(int amount)
    {
        hp.value -= amount;
        if (hp.value <= 0)
        {
            StopAllCoroutines();
            Die();
        }
    }

    void Die()
    {
        if (missionManager != null)
        {
            missionManager.AddProgress(Mission.MissionType.KillBoss, 1);
            StartCoroutine(DelayedClear());
        }
    }

    IEnumerator DelayedClear()
    {
        earthStageManager = FindFirstObjectByType<EarthStageManager>();
        yield return new WaitForSeconds(0.1f);
        earthStageManager.GameClear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet")) TakeDamage(2);
    }
}