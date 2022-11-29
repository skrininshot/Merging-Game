using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool isTutorial;
    public int unlockedLevels = 0;
    [SerializeField] public Level[] levels;
    public int currentLevel = 0;
    private Level currentLevelObj;
    private AudioSource audioData;
    [SerializeField] private GameUI gameUI;
    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private Items itemsManager;
    [SerializeField] private AudioClip[] sounds;

    private void Start()
    {
        audioData = GetComponent<AudioSource>();

        if (PlayerPrefs.GetInt("FirstLaunch", 1) == 1)
        {
            PlayerPrefs.SetInt("FirstLaunch",0);
            PlayerPrefs.SetInt("UnlockedLevels", 0);
            PlayerPrefs.SetInt("CurrentLevel", 0);
            PlayerPrefs.SetInt("IsTutorial", 1);
        }

        unlockedLevels = PlayerPrefs.GetInt("UnlockedLevels",0);
        currentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);
        isTutorial = PlayerPrefs.GetInt("IsTutorial",1) == 1 ? true : false;

        ChangeLevel(currentLevel); 
    }

    public void ChangeLevel(int newLevel)
    {
        if (newLevel < 0 || newLevel > levels.Length - 1) return;
        currentLevel = newLevel;
        if (currentLevelObj != null) { Destroy(currentLevelObj.gameObject); PlaySound(3);}

        currentLevelObj = Instantiate(levels[newLevel], transform.parent);
        currentLevelObj.transform.SetSiblingIndex(1);
        mainMenu.ChangeLevelUI();
        itemsManager.UpdateCells();
        PlayerPrefs.SetInt("CurrentLevel", currentLevel);
    }
    public void ChangeLevel(bool right)
    {
        int newLevel = right ? currentLevel + 1 : currentLevel - 1;
        if (newLevel < 0 || newLevel > levels.Length - 1) return;
        ChangeLevel(newLevel);
    }

    public void StartGame()
    {
        mainMenu.gameObject.SetActive(false);
        gameUI.gameObject.SetActive(true);
        itemsManager.gameObject.SetActive(true);
        StartCoroutine(StartGameTimer());
    }

    public void PlaySound(int sound)
    {
        audioData.clip = sounds[sound];
        audioData.Play();
    }

    IEnumerator StartGameTimer()
    {
        yield return new WaitForSeconds(0.1f);

        if (isTutorial && currentLevel == 0)
        {
            FindObjectOfType<TutorialManager>().enabled = true;
        }
        else
        {
            itemsManager.SpawnItem(2);
        }
        currentLevelObj.SetLevel();
    }

    public void StopGame(bool immediately)
    {
        if (immediately)
        {
            StopGameEvent();
        }
        else
        {
            if (currentLevel < levels.Length - 1 && currentLevel == unlockedLevels)
            {
                PlayerPrefs.SetInt("UnlockedLevels", unlockedLevels + 1);
                PlayerPrefs.SetInt("CurrentLevel", currentLevel + 1);
            }
            else if (currentLevel != unlockedLevels)
            {
                PlayerPrefs.SetInt("CurrentLevel", currentLevel);
            }
            PlaySound(4);
            StartCoroutine(StopGameTimer());
        }
    }

    public void StopGameEvent()
    {
        currentLevelObj.ClearLevel();
        mainMenu.gameObject.SetActive(true);
        mainMenu.ResetMenu();
        gameUI.gameObject.SetActive(false);
        itemsManager.gameObject.SetActive(false);
    }

    IEnumerator StopGameTimer()
    {
        yield return new WaitForSeconds(4f);
        StopGameEvent();
    }

    public void ResetSettings()
    {
        StartCoroutine(ResetSettingsTimer());
    }

    private IEnumerator ResetSettingsTimer()
    {
        yield return new WaitForSeconds(0.25f);
        PlayerPrefs.SetInt("FirstLaunch", 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}