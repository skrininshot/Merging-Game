using UnityEngine;

public class Level : MonoBehaviour
{
    
    [SerializeField] private GameObject[] contentManagers;
    public Sprite[] itemsOnLevel;

    public void MergeEffect()
    {
        GetComponent<Animator>().Rebind();
        GetComponent<Animator>().SetTrigger("Merge");
    }

    private void SetContentManagers()
    {
        Transform mainPanel = GameObject.FindGameObjectWithTag("MainPanel").transform;
        int count = 0;
        for (int i = 0; i < mainPanel.childCount; i++)
        {
            if (mainPanel.GetChild(i).tag == "ContentManager")
            {
                contentManagers[count] = mainPanel.GetChild(i).gameObject;
                count++;
            }
        }
    }

    public void SetDepth()
    {
        foreach (GameObject content in contentManagers)
        {
            for (int i = 0; i < content.transform.childCount; i++)
            {
                MergeItem mergeItem = content.transform.GetChild(i).gameObject.GetComponent<ItemCell>().thisItemObject;
                GameObject ghostItem = content.transform.GetChild(i).gameObject.GetComponent<ItemCell>().currentGhostItemObject;
                if (mergeItem != null) mergeItem.transform.SetAsLastSibling();
                if (ghostItem != null) ghostItem.transform.SetAsLastSibling();
            }
        }
    }

    public void SetLevel()
    {
        SetContentManagers();
        foreach (ItemCell cell in FindObjectOfType<Items>().cells)
        {
            cell.ShowGhostItem();
        }
    }

    public void ClearLevel()
    {  
        foreach (ItemCell cell in FindObjectOfType<Items>().cells)
        {
            cell.ClearCell();
        }
    }
}