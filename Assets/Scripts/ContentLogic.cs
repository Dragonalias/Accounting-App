using System;
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
    [SerializeField]
    private float columnWidth = 345;
    [SerializeField]
    private float rowHeight = 150;

    private List<Column> contentItems;
    private int longestColumn = 0;
    private AccountingManager manager;
    private Vector2 dummyVector;
    private float oneColumnWidth;
    private float oneRowHeight;

    public List<Column> ContentItems { get => contentItems; set => contentItems = value; }

    public void Init(AccountingManager manager)
    {
        this.manager = manager;

        ContentItems = new List<Column>();
        dummyVector = new Vector2();

        oneColumnWidth = columnWidth + paddingBetweenColumns;
        oneRowHeight = rowHeight + paddingBetweenRows;
    }

    public void InsertColumn(int column)
    {
        if (column != ContentItems.Count)
        {
            for (int i = column; i < ContentItems.Count; i++)
            {
                MoveEntireColumnOneRight(i);
            }
        }

        ContentItems.Insert(column, new Column());
        ChangeWidth(RowWidth());
    }

    //Todo: make
    public void RemoveColumn(int column)
    {
        if(column != ContentItems.Count -1)
        {
            for (int i = column +1; i < ContentItems.Count; i++)
            {
                MoveEntireColumnOneLeft(i);
            }
        }

        RemoveEntireColumn(column);
        ContentItems.RemoveAt(column);
        if(longestColumn == column) //The biggest column is gone. find new one
        {
            FindHeighestColumn();
        }
        else if(longestColumn > column)//Every column was moved one to the left
        {
            longestColumn--;
        }
        
    }
    private void RemoveEntireColumn(int column)
    {
        for (int i = 0; i < ContentItems[column].Count; i++)
        {
            ContentItems[column].GetContentItem(i).SetActive(false);
        }
    }
    private void MoveEntireColumnOneLeft(int columnToBeMoved)
    {
        for (int i = 0; i < ContentItems[columnToBeMoved].Count; i++)
        {
            SetContentItemPos(ContentItems[columnToBeMoved].GetContentItem(i), columnToBeMoved - 1, i);
        }
    }
    private void MoveEntireColumnOneRight(int columnToBeMoved)
    {
        for (int i = 0; i < ContentItems[columnToBeMoved].Count; i++)
        {
            SetContentItemPos(ContentItems[columnToBeMoved].GetContentItem(i), columnToBeMoved +1, i);
        }
    }

    public void InsertContentItem(ContentItem item, int column, int row)
    {
        ItemSizeCheck(item);
        
        if (row < ContentItems[column].Count) //Make space for the item
        {
            for (int i = row; i < ContentItems[column].Count; i++)
            {
                SetContentItemPos(ContentItems[column].GetContentItem(row), column, row + 1);
            }
        }
        SetContentItemPos(item, column, row);
        ContentItems[column].Insert(row, item);

        ChangeHeightLogic(column);
    }

    private void ItemSizeCheck(ContentItem item)
    {
        if (item.Width < columnWidth && item.Height < rowHeight)
        {
            return;
        }

        dummyVector.Set(item.Width, item.Height);

        if(item.Width > columnWidth)
        {
            dummyVector.x = columnWidth;
        }
        if(item.Height > rowHeight)
        {
            dummyVector.y = rowHeight;
        }
        item.RectTransform.sizeDelta = dummyVector;
    }

    public void RemoveContentItem(int column, int row)
    {
        if (ContentItems[column].Count == 0)
        {
            Debug.LogError("How did it get to this?");
            return;
        }
        //Move every item under it up
        if (row < ContentItems[column].Count)
        {
            for (int i = row + 1; i < ContentItems[column].Count; i++)
            {
                SetContentItemPos(ContentItems[column].GetContentItem(i), column, i-1);
            }
        }
        ContentItems[column].GetContentItem(row).SetActive(false);
        ContentItems[column].RemoveAt(row);

        ChangeHeightLogic(column);
    }

    private void SetContentItemPos(ContentItem item, int column, int row)
    {
        dummyVector.Set(oneColumnWidth * column, (oneRowHeight * row) * -1); //down is negative, so gotta flip
        item.RectTransform.anchoredPosition = dummyVector;
        item.Column = column;
        item.Row = row;
    }

    private float ColumnHeight(int column)
    {
        return ContentItems[column].Count * oneRowHeight;
    }
    private float RowWidth()
    {
        return ContentItems.Count * oneColumnWidth;
    }

    private bool CheckAllColumnHeights(float heightToCompare)
    {
        for (int i = 1; i < ContentItems.Count; i++)
        {
            if (ContentItems[i].columnHeight > heightToCompare)
            {
                if (i == longestColumn) break;

                longestColumn = i;
                return true;
            }
        }
        return false;
    }
    private void FindHeighestColumn()
    {
        float highestNumber = 0;
        int highestColumnNumber = 0;
        for (int i = 1; i < ContentItems.Count; i++)
        {
            if (ContentItems[i].columnHeight > highestNumber)
            {
                highestNumber = ContentItems[i].columnHeight;
                highestColumnNumber = i;
            }
        }
        longestColumn = highestColumnNumber;
        ChangeHeight(highestNumber);
    }
    private void ChangeHeightLogic(int column)
    {
        var newHeight = ColumnHeight(column);

        if (column == longestColumn)
        {
            if (newHeight < ContentItems[longestColumn].columnHeight && !CheckAllColumnHeights(newHeight))
            {
                ChangeHeight(newHeight);
            }
        }

        //update on remove
        if (newHeight > ContentItems[longestColumn].columnHeight)
        {
            longestColumn = column;
            ChangeHeight(newHeight);
        }

        ContentItems[column].columnHeight = newHeight;
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
    public void Insert(int i, ContentItem item)
    {
        contentItems.Insert(i, item);
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
