using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagnetCleaner : MonoBehaviour
{
    [Header("MP 관련")]
    public Slider mpSlider;
    public float mpMax = 100f;
    public float mpGainSpeed = 10f;     // MP 증가 속도
    public float mpDrainSpeed = 20f;    // 자력 발동 시 MP 감소 속도
    private float currentMp = 0f;
    private bool magnetActive = false;

    [Header("자력 관련")]
    public float magnetRadius = 10f;
    public float pullStrength = 50f;

    private void Start()
    {
        currentMp = 1f;
        mpSlider.maxValue = mpMax;
        mpSlider.value = currentMp;
    }

    private void Update()
    {
        if (!magnetActive)
        {
            currentMp += mpGainSpeed * Time.deltaTime;
            if (currentMp >= mpMax)
            {
                currentMp = mpMax;
                ActivateMagnet();
            }
        }
        else
        {
            currentMp -= mpDrainSpeed * Time.deltaTime;
            if (currentMp <= 0f)
            {
                currentMp = 0f;
                DeactivateMagnet();
            }
            else
            {
                AttractTrash(); // 자력으로 쓰레기 끌어당기기
            }
        }

        mpSlider.value = currentMp;
    }

    void ActivateMagnet()
    {
        magnetActive = true;
        Debug.Log("자력 청소 시작!");
    }

    void DeactivateMagnet()
    {
        magnetActive = false;
        Debug.Log("자력 청소 종료.");
    }

    void AttractTrash()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, magnetRadius);
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("SpaceTrash"))
            {
                Vector3 dir = (transform.position - col.transform.position).normalized;
                Rigidbody rb = col.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddForce(dir * pullStrength);
                }

                // 플레이어에 닿으면 삭제
                if (Vector3.Distance(transform.position, col.transform.position) < 1f)
                {
                    Destroy(col.gameObject);
                }
            }
        }
    }
}