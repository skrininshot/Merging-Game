using UnityEngine;

public class Items : MonoBehaviour
{
    [SerializeField] private MergeItem mergeItem;
    [SerializeField] private AudioClip[] sounds;
    public int actionsCount;

    [HideInInspector] public ItemCell[] cells;
    [HideInInspector] public Sprite[] itemImage;

    private Transform parentObject;
    private MergeItem takenItem = null;
    private AudioSource audioData;


    private void Start()
    {
        audioData = GetComponent<AudioSource>();
        itemImage = FindObjectOfType<Level>().itemsOnLevel;
        parentObject = GameObject.FindGameObjectWithTag("Items").GetComponent<Transform>();

        UpdateCells();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) OnDown();
        if (Input.GetMouseButtonUp(0)) OnUp();
    }

    private void OnDown()
    {
        if (takenItem is not null) return;

        MergeItem nearest = NearestItem();
        if (Vector2.Distance(Input.mousePosition, nearest.transform.position) < 100f)
        {
            takenItem = nearest;
            takenItem.TakeItem();
        }
    }

    public void OnUp()
    {
        if (takenItem is null) return;
        takenItem.PutItem();
        takenItem = null;
    }

    public void UpdateCells()
    {
        cells = FindObjectsOfType<ItemCell>();
    }

    public void SpawnItem(int count = 1)
    {
        if (count < 1) return;

        MergeItem newItem;
        foreach (ItemCell cell in cells)
        {
            if (cell.active == true && 
                cell.thisItem == -1 && 
                cell.ghostItem == -1)
            {
                if (count == 0) return;
                
                newItem = Instantiate(mergeItem, parentObject);
                newItem.transform.position = cell.transform.position;
                count--;
            }
        }  
    }

    public void SpawnItem(ItemCell[] targets)
    {
        if (targets is null) return;

        MergeItem newItem;
        foreach (ItemCell cell in targets)
        {
            newItem = Instantiate(mergeItem, parentObject);
            newItem.transform.position = cell.transform.position;
        }
    }

    private MergeItem NearestItem()
    {
        MergeItem closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = Input.mousePosition;
        MergeItem[] items = FindObjectsOfType<MergeItem>();

        foreach (MergeItem item in items)
        {
            float curDistance = Vector2.Distance(position, item.transform.position);
            if (curDistance < distance)
            {
                closest = item;
                distance = curDistance;
            }
        }
        return closest;
    }

    public void PlaySound(int sound)
    {
        audioData.clip = sounds[sound];
        audioData.Play();
    }
}