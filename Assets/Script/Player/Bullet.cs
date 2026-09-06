using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private PlayerControl player;

    void Start()
    {
        player = FindFirstObjectByType<PlayerControl>();
    }

    void Update()
    {
        transform.Translate(new Vector3(1, 0, 0) * 30 * Time.deltaTime);

        if (this.transform.position.x >= player.transform.position.x + 18) Destroy(this.gameObject);
    }
}