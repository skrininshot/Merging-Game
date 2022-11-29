using UnityEngine;
using UnityEngine.UI;

public class TextProperties : MonoBehaviour
{
    [HideInInspector] public string text;
    void Start()
    {
        text = GetComponent<Text>().text;
    }

    public void ChangeText(string newText)
    {
        text = newText;
        GetComponent<Text>().text = newText;

        if (transform.childCount > 0)
        {
            transform.GetChild(0).GetComponent<Text>().text = newText;
        }
    }
}