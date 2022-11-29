using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CursorTutorial : MonoBehaviour
{
    private Color alpha;

    private void Start()
    {
        alpha = GetComponent<Image>().color;
    }
    private IEnumerator CursorDisappear()
    {
        while(alpha.a > 0)
        {
            alpha.a -= 4f * Time.deltaTime;
            GetComponent<Image>().color = alpha;
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }
}
