using UnityEngine;
using UnityEngine.UI;

public class MergeItem : MonoBehaviour
{
    public int item = 0;
    private enum itemState {cell, mouse}
    private itemState state = itemState.cell;
    private Vector3 addDistance;
    private bool movingToCell;
    private ItemCell currentCell;
    private Items itemsManager;

    private void Start()
    {
        itemsManager = FindObjectOfType<Items>();
        currentCell = NearestCell();
        currentCell.thisItem = item;
        currentCell.thisItemObject = GetComponent<MergeItem>();
        transform.position = currentCell.transform.position;

        Jump();
        SetItem(item);
    }

    public void TakeItem()
    {
        if (state == itemState.cell)
        {
            state = itemState.mouse;
            transform.SetAsLastSibling();
            addDistance = transform.position - Input.mousePosition;
            movingToCell = false;
        }
    }

    public void PutItem()
    {
        ItemCell nearestCell = NearestCell(true);
        if (state == itemState.mouse)
        {
            if (nearestCell != currentCell)
            {
                if (nearestCell.ghostItem != -1 && nearestCell.ghostItem != item && nearestCell.active == true) 
                {
                    itemsManager.PlaySound(1);
                }
                
                if ((nearestCell.ghostItem == -1 || nearestCell.ghostItem == item) && nearestCell.thisItem == -1) 
                {
                    ChangeCell(nearestCell); 
                    
                }
                else if (nearestCell.thisItem == item && nearestCell.active == true) Merge(nearestCell);
                else movingToCell = true;
            }
            else movingToCell = true;

            FindObjectOfType<Level>().SetDepth();
            state = itemState.cell;
        }
    }

    private void Update()
    {
        switch (state)
        {
            case itemState.cell: if (movingToCell) MoveToCell(); break;
            case itemState.mouse: FollowPointer(); break;
        }
    }

    private void Merge(ItemCell cell)
    {
        Destroy(cell.thisItemObject.gameObject);
        LevelUp();
        ChangeCell(cell);
        
        if (!MergePartner())
        {
            itemsManager.SpawnItem(2);
        }
        itemsManager.PlaySound(4);
        FindObjectOfType<Level>().MergeEffect();
    }

    private void LevelUp()
    {
        if (item + 1 == -1) return;
        SetItem(item + 1);
        itemsManager.actionsCount++;
    }

    private void ChangeCell(ItemCell cell)
    {
        currentCell.thisItem = -1;
        currentCell.thisItemObject = null;

        currentCell = cell;

        currentCell.thisItem = item;
        currentCell.thisItemObject = GetComponent<MergeItem>();

        movingToCell = true;

        if (currentCell.ghostItem == item)
        {
            currentCell.active = false;
            currentCell.GhostItemMatch();
            itemsManager.PlaySound(2);
            if (GhostTargets())
            {
                FindObjectOfType<Items>().SpawnItem(2);
                FindObjectOfType<Congratulations>().Congratulation("Предмет поставлен!");
                
            }
            else
            {
                FindObjectOfType<Congratulations>().Congratulation("Уровень пройден!");
                FindObjectOfType<GameManager>().StopGame(false);
            }
            currentCell.ShineEffect();
            FindObjectOfType<GameUI>().AddProgressBar();
            itemsManager.actionsCount++;
            FindObjectOfType<Level>().MergeEffect();
        }
        if (currentCell.thisItem == item && currentCell.ghostItem == -1)
        {
            itemsManager.PlaySound(0);
        }
    }

    private void Jump(float strength = 100f)
    {
        transform.position =
            new Vector2(transform.position.x,
            transform.position.y+strength);
        movingToCell = true;
    }

    private void MoveToCell()
    {
        if (Vector2.Distance(transform.position, currentCell.transform.position) > 0.1f)
        {
            transform.position =
                Vector2.Lerp(currentCell.transform.position, 
                new Vector3(transform.position.x, transform.position.y, transform.position.z),
                Time.fixedDeltaTime * 15f);
        }
        else
        {
            transform.position = currentCell.transform.position;
            movingToCell = false;
        }
    }

    private void FollowPointer()
    {
        transform.position = Input.mousePosition + addDistance;
    }

    private ItemCell NearestCell(bool mouse = false)
    {
        ItemCell closestCell = null;
        float distance = Mathf.Infinity;
        Vector3 position;

        if (mouse) position = Input.mousePosition;
        else position = transform.position;
        
        foreach (ItemCell cell in itemsManager.cells)
        {
            float curDistance = Vector2.Distance(position, cell.transform.position);
            if (curDistance < distance)
            {
                closestCell = cell;
                distance = curDistance;
            }
        }
        return closestCell;
    }

    private void SetItem(int newItem)
    {
        if (newItem == -1) return;
        item = newItem;

        GetComponent<RectTransform>().pivot = new Vector2(
            itemsManager.itemImage[item].pivot.x / itemsManager.itemImage[item].rect.width,
            itemsManager.itemImage[item].pivot.y / itemsManager.itemImage[item].rect.height);

        GetComponent<Image>().sprite = itemsManager.itemImage[item];
        GetComponent<Image>().SetNativeSize();
    }

    private bool MergePartner()
    {
        foreach (ItemCell cell in itemsManager.cells)
        {
            if (cell.thisItemObject != GetComponent<MergeItem>() &&
                cell.active == true &&
                (cell.thisItem == item || cell.ghostItem == item))
                return true;
        }
        return false;
    }

    private bool GhostTargets()
    {
        foreach (ItemCell cell in itemsManager.cells)
        {
            if (cell.active == true &&
                cell.ghostItem != -1)
                return true;
        }
        return false;
    }
}