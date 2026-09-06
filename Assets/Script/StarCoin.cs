using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarCoin : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) { Coin.Instance.coin += 10; Destroy(this.gameObject); }
    }

    void Update()
    {
        transform.Rotate(0, 2f, 0);
    }
}