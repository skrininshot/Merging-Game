using UnityEngine;
using UnityEngine.UI;

public class ItemCell : MonoBehaviour
{
    [SerializeField] private GameObject ghostItemObject = null;
    [SerializeField] private GameObject shineEffect;
    public int ghostItem = -1;

    [HideInInspector] public int thisItem = -1;
    [HideInInspector] public MergeItem thisItemObject = null;
    [HideInInspector] public bool active = true;

    [HideInInspector] public GameObject currentGhostItemObject = null;
    private Animator anim;

    private void Start()
    {
        thisItem = -1;
        
        shineEffect = Instantiate(shineEffect, transform);
        anim = shineEffect.GetComponent<Animator>();
        shineEffect.SetActive(false);
    }

    public void ShowGhostItem()
    {
        if (ghostItem != -1)
        {
            Items itemsManager = FindObjectOfType<Items>();
            currentGhostItemObject = Instantiate(ghostItemObject, GameObject.FindGameObjectWithTag("Items").transform);
            currentGhostItemObject.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            currentGhostItemObject.transform.SetAsLastSibling();
            currentGhostItemObject.GetComponent<Image>().sprite = itemsManager.itemImage[ghostItem];
            currentGhostItemObject.GetComponent<Image>().SetNativeSize();

            currentGhostItemObject.GetComponent<RectTransform>().pivot = new Vector2(
            itemsManager.itemImage[ghostItem].pivot.x / itemsManager.itemImage[ghostItem].rect.width,
            itemsManager.itemImage[ghostItem].pivot.y / itemsManager.itemImage[ghostItem].rect.height);
        }
    }

    public void ClearCell()
    {
        if (ghostItem != -1)
        {
            Destroy(currentGhostItemObject);
            currentGhostItemObject = null;
            
        }
        if (thisItem != -1)
        {
            thisItem = -1;
            if (thisItemObject != null) Destroy(thisItemObject.gameObject);
        }
        active = true;
    }

    public void ShineEffect()
    {
        shineEffect.SetActive(true);
        anim.Rebind();
        anim.SetTrigger("Merge");
    }

    public void GhostItemMatch()
    {
        currentGhostItemObject.GetComponent<Image>().color = Color.HSVToRGB(0.33f, 1f, 1); //Green
        Destroy(thisItemObject.gameObject);
    }
}