using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scroller : MonoBehaviour
{
    private RawImage image;
    [SerializeField] private float x, y;
    void Start()
    {
        image = GetComponent<RawImage>();
        image.uvRect = new Rect(new Vector2(
            PlayerPrefs.GetFloat(gameObject.name + "_x", 0), 
            PlayerPrefs.GetFloat(gameObject.name + "_y", 0)), 
            image.uvRect.size);
    }

    
    void Update()
    {
        image.uvRect = new Rect(image.uvRect.position + new Vector2(x,y) * Time.deltaTime, image.uvRect.size);
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetFloat(gameObject.name + "_x", image.uvRect.position.x);
        PlayerPrefs.SetFloat(gameObject.name + "_y", image.uvRect.position.y);
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat(gameObject.name + "_x", 0);
        PlayerPrefs.SetFloat(gameObject.name + "_y", 0);
    }
}
