using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private static PlayerControl instance;
    public static PlayerControl Instance => instance;

    [Header("가속도")]
    public float ACCELERATION = 15.0f;

    [Header("점프 높이")]
    public float JUMP_HEIGHT_MAX = 3.0f;

    [Header("점프 힘")]
    public float jumpPower = 8.0f;

    [Header("Level Control")]
    public float current_speed = 0.0f;
    public LevelControl level_control = null;

    public Rigidbody rb;
    public int jumpCount = 1;

    public Vector3 velocity;
    public Animator ani;
    public AudioSource effectSound;
    [SerializeField] private AudioSource coinEffectSound;
    public GameObject Bullet;

    public static float NARAKU_HEIGHT = -5.0f;

    private float bulletCooldown = 0.5f;
    private float lastBulletTime = -999f;

    private GravitySystem gravitySystem;
    public float floatControlSpeed = 5.5f; // 중력 OFF 시 위/아래 수동 이동 속도

    // === 새로 추가된 자력 및 MP 관련 변수 ===
    [Header("MP & 자력")]
    public float maxMP = 100f;
    public float currentMP = 0f;
    public float mpDecreaseSpeed = 20f;  // 자력 사용 시 MP 감소량 (초당)
    public float mpIncreaseSpeed = 10f;  // 자력 미사용 시 MP 충전량 (초당)

    public bool isAttractActive = false;
    public bool isBoosting = false;

    private List<SpaceTrash> attractedTrashList = new List<SpaceTrash>();

    // === 기존 함수들 그대로 ===

    public void MoveControl()
    {
        transform.Translate(Vector3.forward * ACCELERATION * Time.deltaTime);
    }

    public void AirControl()
    {
        if (isBoosting) return;

        if (gravitySystem != null && gravitySystem.gravity)
        {
            // 기존 점프
            if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 2 && ACCELERATION != 0)
            {
                velocity.y = Mathf.Sqrt(20f * JUMP_HEIGHT_MAX);
                jumpCount++;

                MissionManager missionManager = FindAnyObjectByType<MissionManager>();
                if (missionManager != null)
                    missionManager.AddProgress(Mission.MissionType.JumpCount, 1);

                if (jumpCount == 1) ani.Play("Jump1");
                if (jumpCount == 2) ani.Play("Jump2");
                effectSound.Play();
            }

            if (velocity.y < -0.001f) { ani.Play("Down"); velocity.y += -80 * Time.deltaTime; }
            if (Mathf.Abs(velocity.y) < 0.01f) { jumpCount = 0; ani.Play("Run"); }
        }
        else
        {
            // 중력 OFF일 때 Z = 위, X = 아래
            if (Input.GetKey(KeyCode.UpArrow)) transform.Translate(Vector3.up * floatControlSpeed * Time.deltaTime);
            if (Input.GetKey(KeyCode.DownArrow)) transform.Translate(Vector3.down * floatControlSpeed * Time.deltaTime);
        }
    }

    public void BulletControl()
    {
        if (Time.time - lastBulletTime >= bulletCooldown && !isBoosting)
        {
            Instantiate(Bullet, transform.position, Quaternion.identity);
            lastBulletTime = Time.time;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Colider Object")
        {
            if (gravitySystem != null && gravitySystem.gravity == false)
            {
                EarthStageManager.Instance.GameOver();
            }

            else
            {
                ACCELERATION = 0;
                velocity.y = 0f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("StarCoin")) coinEffectSound.Play();
    }

    private float runTimeUpdateInterval = 1.0f;
    private float lastRunTimeUpdate = 0f;

    private void Update()
    {
        velocity = rb.velocity;
        current_speed = level_control.getPlayerSpeed();

        if (transform.position.y <= -10)
        {
            EarthStageManager.Instance.GameOver();
            ACCELERATION *= Time.deltaTime;
            if (velocity.x < 0.0f) velocity.x = 0.0f;
        }

        if (Time.time - lastRunTimeUpdate >= runTimeUpdateInterval)
        {
            MissionManager missionManager = FindAnyObjectByType<MissionManager>();
            if (missionManager != null)
                missionManager.AddProgress(Mission.MissionType.RunTime, 1);

            lastRunTimeUpdate = Time.time;
        }

        if (gravitySystem.gravity == false) velocity.y = 0f;

        // ======= MP 및 자력 처리 =======
        HandleMPAndAttract();

        MoveControl();
        AirControl(); // 기존 JumpControl 대체
        BulletControl();

        rb.velocity = velocity;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        effectSound = GetComponent<AudioSource>();
        ACCELERATION = 15.0f;

        gravitySystem = FindFirstObjectByType<GravitySystem>();

        coinEffectSound.volume = GameSetting.Instance.SFX.value;
    }

    // ======= 새로 추가된 MP 및 자력 처리 함수 =======
    void HandleMPAndAttract()
    {
        if (!isAttractActive)
        {
            currentMP += mpIncreaseSpeed * Time.deltaTime;
            if (currentMP > maxMP) currentMP = maxMP;

            if (currentMP >= maxMP)
            {
                ActivateAttract();
            }
        }
        else
        {
            currentMP -= mpDecreaseSpeed * Time.deltaTime;
            if (currentMP <= 0)
            {
                currentMP = 0;
                DeactivateAttract();
            }
            else
            {
                AttractTrash();
            }
        }
    }

    void ActivateAttract()
    {
        isAttractActive = true;
        attractedTrashList.Clear();

        GameObject[] trashObjects = GameObject.FindGameObjectsWithTag("SpaceTrash");
        foreach (var obj in trashObjects)
        {
            var trash = obj.GetComponent<SpaceTrash>();
            if (trash != null)
            {
                trash.StartAttract();
                attractedTrashList.Add(trash);
            }
        }
    }

    void DeactivateAttract()
    {
        isAttractActive = false;
        foreach (var trash in attractedTrashList)
        {
            if (trash != null) trash.StopAttract();
        }
        attractedTrashList.Clear();
    }

    void AttractTrash()
    {
        // 실제 끌어당기는 동작은 SpaceTrash 쪽에서 Rigidbody.MovePosition 등으로 구현
        // 여기서는 그냥 존재만 확인해도 됩니다.
    }
}