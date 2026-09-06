using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroup : MonoBehaviour
{
    void Update()
    {
        transform.Translate(new Vector3(-1, 0, 0) * 10 * Time.deltaTime);
    }
}
