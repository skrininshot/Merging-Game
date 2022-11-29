using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private ItemCell[] firstLaunchCells;
    [SerializeField] private GameObject tutorialCursor;
    [SerializeField] private TutorialTip tutorialTip;
    private Items itemsManager;
    private int itemOnSwitch = 0;

    private void Start()
    {
        GameUI gameUI = FindObjectOfType<GameUI>();

        itemsManager = FindObjectOfType<Items>();
        tutorialCursor = Instantiate(tutorialCursor, gameUI.transform);
        tutorialTip = Instantiate(tutorialTip, gameUI.transform);
        tutorialTip.transform.SetAsLastSibling();

        itemsManager.SpawnItem(firstLaunchCells);
    }

    private void Update()
    {
        ChangingItemsOnTutorial();
    }

    public void ChangingItemsOnTutorial()
    {
        if (itemOnSwitch == itemsManager.actionsCount) return;

        switch (itemsManager.actionsCount)
        {
            case 1:
                tutorialCursor.GetComponent<Animator>().Rebind();
                tutorialCursor.GetComponent<Animator>().Play("CursorAnimation2");
                tutorialTip.StartCoroutine("AddTipAnimation");
                break;
            case 2:
                tutorialCursor.GetComponent<CursorTutorial>().StartCoroutine("CursorDisappear");
                break;
            case 6:
                itemOnSwitch = itemsManager.actionsCount;
                tutorialTip.StartCoroutine("TipDisappear");
                FindObjectOfType<GameManager>().isTutorial = false;
                PlayerPrefs.SetInt("IsTutorial", 0);
                break;
        }

        itemOnSwitch = itemsManager.actionsCount;
    }
}
