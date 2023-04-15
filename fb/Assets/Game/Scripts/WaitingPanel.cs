using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitingPanel : MonoBehaviour
{
	[SerializeField] Text playTxt;
    string textToBlink;
    public float BlinkTime;

    void Awake()
    {
        textToBlink = playTxt.text;
    }
    void OnEnable()
    {
        StartCoroutine(BlinkText());
    }
    IEnumerator BlinkText()
    {
        while (true)
        {
            playTxt.text = textToBlink;
            yield return new WaitForSeconds(BlinkTime);
            playTxt.text = string.Empty;
            yield return new WaitForSeconds(BlinkTime);
        }
    }
}
