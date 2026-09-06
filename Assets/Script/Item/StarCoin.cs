using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarCoin : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(0, 1.5f, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) { Coin.Instance.coin += 100; Destroy(this.gameObject); Coin.Instance.AddCoin(100); }
    }
}