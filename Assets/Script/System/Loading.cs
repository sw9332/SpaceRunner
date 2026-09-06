using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    public Slider loading;

    private void Start()
    {
        loading.value = 0;
    }

    private void Update()
    {
        loading.value += 100 * Time.deltaTime;
        if (loading.value >= 100) SceneManager.LoadScene("Earth_Stage_Scene");
    }
}