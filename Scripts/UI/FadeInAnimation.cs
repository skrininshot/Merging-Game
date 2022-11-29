using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeInAnimation : MonoBehaviour
{
    private Color textColor;
    private Text text;
    private void OnEnable()
    {
        text = GetComponent<Text>();
        textColor = text.color;
        textColor.a = 0f;
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        while (textColor.a < 1)
        {
            textColor.a += 0.1f;
            text.color = textColor;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
