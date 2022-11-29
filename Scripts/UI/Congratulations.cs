using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Congratulations : MonoBehaviour
{
    [SerializeField] private Image BG;
    [SerializeField] private Color BG_Alpha;
    [SerializeField] private Text textBG;
    [SerializeField] private Text text;
    [SerializeField] private float timerSeconds = 5f;
    private readonly float distanceToBorder = -600f;

    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.offsetMin = new Vector2(distanceToBorder, rectTransform.offsetMin.y);
        BG_Alpha = BG.color;
        BG_Alpha.a = 0f;
        BG.color = BG_Alpha;
    }

    public void Congratulation(string conText = "Предмет установлен!")
    {
        StopAllCoroutines();
        textBG.text = conText;
        text.text = conText;
        StartCoroutine(CongratulationAnim(conText)); 
    }

    IEnumerator CongratulationAnim(string conText)
    {
        rectTransform.offsetMin = new Vector2(distanceToBorder, rectTransform.offsetMin.y);
        BG_Alpha.a = 0f;
        while (Mathf.Abs(rectTransform.offsetMin.x) > 0.5f)
        {
            rectTransform.offsetMin = Vector2.Lerp(rectTransform.offsetMin, new Vector2(0, rectTransform.offsetMin.y), Time.fixedDeltaTime * 20f);
            SetAlpha();
            yield return new WaitForEndOfFrame();
        }
        rectTransform.offsetMin = new Vector2(0, rectTransform.offsetMin.y);
        BG_Alpha.a = 0.5f;
        yield return new WaitForSeconds(timerSeconds);
        while (Mathf.Abs(rectTransform.offsetMin.x) < Mathf.Abs(distanceToBorder) - 0.5f)
        {
            rectTransform.offsetMin = Vector2.Lerp(rectTransform.offsetMin, new Vector2(distanceToBorder, rectTransform.offsetMin.y), Time.fixedDeltaTime * 15f);
            SetAlpha();
            yield return new WaitForEndOfFrame();
        }
        rectTransform.offsetMin = new Vector2(distanceToBorder, rectTransform.offsetMin.y);
        BG_Alpha.a = 0f;
        BG.color = BG_Alpha;
    }

    private void SetAlpha()
    {
        BG_Alpha.a = ((Mathf.Abs(distanceToBorder) - Mathf.Abs(rectTransform.offsetMin.x)) / Mathf.Abs(distanceToBorder)) * 0.5f;
        BG.color = BG_Alpha;
    }
}