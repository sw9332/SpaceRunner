using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet")) { Destroy(this.gameObject); Destroy(other.gameObject); }
    }

    void Update()
    {
        
    }
}
