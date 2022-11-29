using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTip : MonoBehaviour
{
    private RectTransform rectTransform;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    IEnumerator AddTipAnimation()
    {
        string addText = "\n и располагай их в комнате";
        string myText = GetComponent<Text>().text;
        int length = addText.Length + myText.Length;
        int counter = 0;
        while (GetComponent<Text>().text.Length < length)
        {
            counter++;
            string newText = myText + addText.Substring(0, counter);
            GetComponent<Text>().text = newText;
            transform.GetChild(0).GetComponent<Text>().text = newText;
            yield return new WaitForSeconds(0.02f);
        }
    }

    IEnumerator TipDisappear()
    {
        while (rectTransform.GetComponent<RectTransform>().transform.position.x > -345f)
        {
            rectTransform.transform.position = new Vector2(rectTransform.transform.position.x + (-350f - rectTransform.transform.position.x) / 10, rectTransform.transform.position.y);
            yield return new WaitForSeconds(0.01f);
        }
        Destroy(rectTransform.gameObject);
    }
}