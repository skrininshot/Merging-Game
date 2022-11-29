using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private Image progressBar;
    private RectTransform progressBarParent;
    private float proportion;
    private float finalProgress = 0;
    private int ghostItemsCount;

    private void Start()
    {
        progressBar.fillAmount = 0;
        progressBarParent = progressBar.transform.parent.GetComponent<RectTransform>();
        progressBarParent.gameObject.SetActive(false);
        StartCoroutine(TimeAfterSpawn());
    }
    IEnumerator TimeAfterSpawn()
    {
        yield return new WaitForSeconds(0.1f);
        ghostItemsCount = GameObject.FindGameObjectsWithTag("GhostItem").Length;
        proportion = 1f / ghostItemsCount;
    }
    public void AddProgressBar()
    {
        if (progressBarParent.gameObject.activeSelf == false)
        {
            progressBarParent.gameObject.SetActive(true);
        }
        
        StopCoroutine(AddProgressBarAnimation());

        progressBar.fillAmount = finalProgress;
        finalProgress = 0;
        
        StartCoroutine(AddProgressBarAnimation());
    }
    
    IEnumerator AddProgressBarAnimation()
    {
        finalProgress = progressBar.fillAmount + proportion;
        while (progressBar.fillAmount < finalProgress)
        {
            progressBar.fillAmount += (finalProgress - progressBar.fillAmount) * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        finalProgress = 0;
    }
}