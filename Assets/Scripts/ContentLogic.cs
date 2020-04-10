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

    [SerializeField, HideInInspector]
    private List<Column> contentItems;
    [SerializeField, HideInInspector]
    private int longestColumn = 0;

    private AccountingManager manager;
    private Vector2 dummyVector;
    private float oneItemAndPaddingWidth;
    private float oneItemAndPaddingHeight;


    public void Init(AccountingManager manager)
    {
        this.manager = manager;

        contentItems = new List<Column>();
        dummyVector = new Vector2();

        oneItemAndPaddingWidth = manager.ContentItem.Width + paddingBetweenColumns;
        oneItemAndPaddingHeight = manager.ContentItem.Height + paddingBetweenRows;
    }

    public void AddColumn()
    {
        contentItems.Add(new Column());

        ChangeWidth(RowWidth());
    }

    public void RemoveColumn(int column)
    {
        if(column == contentItems.Count)
        {

        }
        contentItems.RemoveAt(column);
    }

    public void AddContentItem(ContentItem item, int column)
    {
        SetContentItemPos(item, column, contentItems[column].Count);
        contentItems[column].Add(item);

        ChangeHeightLogic(column);
    }

    public void RemoveContentItem(Vector2Int listPos)
    {
        if (contentItems[listPos.x].Count == 0)
        {
            Debug.LogError("How did it get to this?");
            return;
        }

        //Move every item under it up
        if(listPos.y < contentItems[listPos.x].Count)
        {
            for (int i = listPos.y +1; i < contentItems[listPos.x].Count; i++)
            {
                SetContentItemPos(contentItems[listPos.x].GetContentItem(i), listPos.x, i-1);
            }
        }
        contentItems[listPos.x].GetContentItem(listPos.y).SetActive(false);
        contentItems[listPos.x].RemoveAt(listPos.y);

        ChangeHeightLogic(listPos.x);
    }

    private void SetContentItemPos(ContentItem item, int column, int row)
    {
        dummyVector.Set(oneItemAndPaddingWidth * column, (oneItemAndPaddingHeight * row) * -1); //down is negative, so gotta flip
        item.RectTransform.anchoredPosition = dummyVector;
    }

    private float ColumnHeight(int column)
    {
        if (contentItems[column].Count == 0) return 0;
        return contentItems[column].Count * oneItemAndPaddingHeight;
    }
    private float RowWidth()
    {
        return contentItems.Count * oneItemAndPaddingWidth;
    }

    private bool CheckAllColumnHeights(float heightToCompare)
    {
        for (int i = 0; i < contentItems.Count; i++)
        {
            if (contentItems[i].columnHeight > heightToCompare)
            {
                if (i == longestColumn) break;

                longestColumn = i;
                return true;
            }
        }
        return false;
    }
    private void ChangeHeightLogic(int column)
    {
        var newHeight = ColumnHeight(column);

        if (column == longestColumn)
        {
            if (newHeight < contentItems[longestColumn].columnHeight && !CheckAllColumnHeights(newHeight))
            {
                ChangeHeight(newHeight);
            }
        }

        if (newHeight > contentItems[longestColumn].columnHeight)
        {
            longestColumn = column;
            ChangeHeight(newHeight);
        }

        contentItems[column].columnHeight = newHeight;
    }
    private void ChangeHeight(float newHeight)
    {
        dummyVector.Set(rt.sizeDelta.x, newHeight);
        rt.sizeDelta = dummyVector;
    }
    private void ChangeWidth(float newWidth)
    {
        dummyVector.Set(newWidth, rt.sizeDelta.y);
        rt.sizeDelta = dummyVector;
    }
    private void ChangeHeightWidth(float newWidth, float newHeight)
    {
        dummyVector.Set(newWidth, newHeight);
        rt.sizeDelta = dummyVector;
    }
}

public class Column
{
    public float columnHeight = 0;
    public List<ContentItem> contentItems = new List<ContentItem>();

    public int Count { get => contentItems.Count; }
    public void Add(ContentItem item)
    {
        contentItems.Add(item);
    }
    public void RemoveAt(int i)
    {
        contentItems.RemoveAt(i);
    }

    public ContentItem GetContentItem(int index)
    {
        return contentItems[index];
    }
    
}
