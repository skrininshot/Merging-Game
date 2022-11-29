using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TextProperties logoText;
    [SerializeField] private GameObject tip;
    [SerializeField] private GameObject levelChanger;
    [SerializeField] private GameObject resetButton;
    [SerializeField] private GameObject tapToStartButton;
    
    private bool itemsManagerActive;
    private Image BG;
    private Color BG_Alpha;

    private void Start()
    {
        BG = GetComponent<Image>();
        BG_Alpha = GetComponent<Image>().color;
    }

    public void ChangeLevelUI()
    {
        levelChanger.transform.GetChild(0).gameObject.SetActive(gameManager.currentLevel != 0);
        levelChanger.transform.GetChild(1).gameObject.SetActive(gameManager.currentLevel != gameManager.unlockedLevels);
        levelChanger.transform.GetChild(2).gameObject.GetComponent<TextProperties>().ChangeText("Óðîâåíü " + (gameManager.currentLevel + 1));
    }

    public void ResetMenu()
    {
        BG_Alpha.a = 0f;
        BG.color = BG_Alpha;
        logoText.ChangeText("ÁÀÐÄÀÊ");
        itemsManagerActive = false;
        tip.SetActive(true);
        StartCoroutine(LogoTextAppear());
        StartCoroutine(BackgroundAppear());
        resetButton.SetActive(false); 
    }

    public void StartGame()
    {
        if (itemsManagerActive == false)
        {
            tip.SetActive(false);
            levelChanger.SetActive(false);
            StartCoroutine(LogoTextDisappear());
            StartCoroutine(BackgroundDisappear());
            itemsManagerActive = true;
            resetButton.SetActive(false);
            gameManager.PlaySound(0);
            tapToStartButton.SetActive(false);
        }
    }

    private IEnumerator BackgroundAppear()
    {
        logoText.gameObject.GetComponent<Animator>().enabled = false;
        logoText.gameObject.GetComponent<RectTransform>().localScale = new Vector2(1, 1);
        logoText.gameObject.GetComponent<RectTransform>().rotation = Quaternion.Euler(0f, 0f, 0f);
        while (BG_Alpha.a < 0.5f)
        {
            BG_Alpha.a += 1f * Time.deltaTime;
            BG.color = BG_Alpha;
            yield return new WaitForEndOfFrame();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator BackgroundDisappear()
    {
        while (BG_Alpha.a > 0)
        {
            BG_Alpha.a -= 1f * Time.deltaTime;
            BG.color = BG_Alpha;
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator LogoTextAppear()
    {
        while (logoText.text.Length < 6)
        {
            logoText.ChangeText(logoText.text.Substring(0, logoText.text.Length + 1));
            yield return new WaitForSeconds(0.05f);
        }
    }

    private IEnumerator LogoTextDisappear()
    {
        while(logoText.text.Length > 0)
        {
            logoText.ChangeText(logoText.text.Substring(0, logoText.text.Length - 1));
            yield return new WaitForSeconds(0.05f);
        }
        gameManager.StartGame();
    } 
}