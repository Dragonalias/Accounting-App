using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentLogic : MonoBehaviour
{
    [SerializeField]
    private RectTransform rt;
    [SerializeField]
    private float paddingBetweenRows;
    [SerializeField]
    private float paddingBetweenColumns;

    private List<List<ContentItem>> contentItems;
    private AccountingManager manager;
    private Vector2 contentPos;

    public void Init(AccountingManager manager)
    {
        this.manager = manager;
        contentItems = new List<List<ContentItem>>();
        contentPos = new Vector2();
    }

    public void AddColumn()
    {
        contentItems.Add(new List<ContentItem>());
    }

    public void AddContentItem(ContentItem item, int column)
    {
        
        contentItems[column].Add(item);
        SetContentItemPos(item, column, contentItems[column].Count);
    }

    public void RemoveContentItem(ContentItem item, Vector2Int listPos)
    {
        //Move every item under it up
        if(listPos.y < contentItems[listPos.x].Count)
        {
            for (int i = listPos.y +1; i < contentItems[listPos.x].Count; i++)
            {
                SetContentItemPos(contentItems[listPos.x][i], listPos.x, listPos.y);
            }
        }
        item.gameObject.SetActive(false);
        contentItems[listPos.x].RemoveAt(listPos.y);

    }

    private void SetContentItemPos(ContentItem item, int column, int row)
    {
        contentPos.Set(item.Width * column + paddingBetweenColumns * column, item.Height * row + paddingBetweenRows * row);
        item.RectTransform.anchoredPosition = contentPos;
    }

    private void ChangeHeight(float heightAdded)
    {
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, rt.sizeDelta.y + heightAdded);
    }
    private void ChangeWidth(float widthAdded)
    {
        rt.sizeDelta = new Vector2(rt.sizeDelta.x + widthAdded, rt.sizeDelta.y);
    }
    private void ChangeHeightWidth(float widthAdded, float heightAdded)
    {
        rt.sizeDelta = new Vector2(rt.sizeDelta.x + widthAdded, rt.sizeDelta.y + heightAdded);
    }
}
