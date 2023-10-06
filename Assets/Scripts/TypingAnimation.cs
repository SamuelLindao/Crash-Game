using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypingAnimation : MonoBehaviour
{
    public float duration;

    private void OnEnable()
    {
        StartCoroutine(SetAnimation());
    }
    public IEnumerator SetAnimation()
    {
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        char [] allChar = text.text.ToCharArray();
        text.text = "";
        int i = 0;
        while (i < allChar.Length)
        {
            text.text += allChar[i];
            yield return new WaitForSeconds(duration);
            i++;
        }
    }
}
