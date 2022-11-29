using UnityEngine;

public class BackgroundColor : MonoBehaviour
{
    [SerializeField] private float hue = 0;
    [SerializeField] private float sat = 0.43f;
    [SerializeField] private float val = 0.62f;

    private void Start()
    {
        hue = PlayerPrefs.GetFloat("HueBG", 0);
    }
    void Update()
    {
        ChangingColor();
    }

    private void ChangingColor()
    {
        hue += 0.005f * Time.deltaTime;
        if (hue > 1) hue = 0;
        GetComponent<Camera>().backgroundColor = Color.HSVToRGB(hue, sat, val);
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetFloat("HueBG",hue);
    }
}