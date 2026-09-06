using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Hp : MonoBehaviour
{
    public bool invincibility = false;
    public Slider hpUI;
    public Text hpText;

    private bool isFlashing = false;

    void HpDecrease()
    {
        hpUI.value -= 0.5f;
    }

    IEnumerator HpBarColor()
    {
        GameObject.Find("Hp Fill").GetComponent<Image>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        GameObject.Find("Hp Fill").GetComponent<Image>().color = new Color(1, 0.3529412f, 0.3529412f, 1);
        yield return new WaitForSeconds(0.1f);
        GameObject.Find("Hp Fill").GetComponent<Image>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        GameObject.Find("Hp Fill").GetComponent<Image>().color = new Color(1, 0.3529412f, 0.3529412f, 1);
        yield return new WaitForSeconds(0.1f);
    }

    void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("thorn") || other.CompareTag("Enemy") || other.CompareTag("Laser") || other.CompareTag("Meteor")) && !invincibility && !isFlashing)
        {
            hpUI.value -= 3;
            StartCoroutine(HpBarColor());
        }
    }

    void Update()
    {
        if (hpUI.value < 1) EarthStageManager.Instance.GameOver();
        hpText.text = hpUI.value.ToString("F0")+"%";
    }

    void Start()
    {
        hpUI.value = 100;
        InvokeRepeating("HpDecrease", 1f, 1f);
    }
}