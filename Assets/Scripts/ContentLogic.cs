using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class ContentLogic : MonoBehaviour
{
    [SerializeField] private RectTransform thisRT;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private float paddingBetweenRows;
    [SerializeField] private float paddingBetweenColumns;
    [SerializeField] private float columnWidth = 345;
    [SerializeField] private float rowHeight = 150;

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

        //StartCoroutine("GiveScrollEventAfterOneFrame");
        
    }

    private IEnumerator GiveScrollEventAfterOneFrame()
    {
        yield return 0;
        //Value changes once at beginning of frame, so i made this workaround
        scrollRect.horizontalScrollbar.onValueChanged.AddListener((x) => AlwaysShowRowCount(x));
        scrollRect.verticalScrollbar.onValueChanged.AddListener((x) => AlwaysShowColumnCountAndPeopleNames(x));
    }
    public void InsertColumn(int column)
    {
        if (column != ContentItems.Count)
        {
            for (int i = column; i < ContentItems.Count; i++)
            {
                MoveEntireColumn(i, i+1);
            }
        }

        ContentItems.Insert(column, new Column());
        ChangeWidth(RowWidth());
    }

    public void RemoveColumn(int column)
    {
        RemoveEntireColumn(column);
        if (column != ContentItems.Count -1)
        {
            for (int i = column +1; i < ContentItems.Count; i++)
            {
                MoveEntireColumn(i, i-1);
            }
        }

        if(column < longestColumn)//Every column was moved one to the left
        {
            longestColumn--;
        }
        
    }
    private void RemoveEntireColumn(int column)
    {
        for (int i = 1; i < ContentItems[column].Count; i++)
        {
            ContentItems[column].GetContentItem(i).SetActive(false);
        }
        ContentItems[column].RemoveRange(1, ContentItems[column].Count -1);
        ChangeHeightLogic(column);
    }

    private void MoveEntireColumn(int columnToBeMoved, int targetColumn)
    {
        if (ContentItems[columnToBeMoved].Count <= 1) return;
        
        ContentItems[targetColumn].AddRange(ContentItems[columnToBeMoved].ReturnRange(1, ContentItems[columnToBeMoved].Count -1));
        
        SetContentItemParentPos(ContentItems[columnToBeMoved].GetContentItem(1), targetColumn, 1);
        

        ContentItems[columnToBeMoved].RemoveRange(1, ContentItems[columnToBeMoved].Count - 1);
    }

    public void InsertContentItemBase(ContentItem item, int column, int row)
    {
        ItemSizeCheck(item);
        ContentItems[column].Insert(row, item);
        ChangeHeightLogic(column);
    }
    public void InsertContentParentItem(ContentItem item, int column, int row)
    {
        SetContentItemParentPos(item, column, row);
        InsertContentItemBase(item, column, row);
    }
    public void InsertContentCountItem(ContentItem item, int column, int row)
    {
        SetContentItemParentPos(item, column, row);
        InsertContentItemBase(item, column, row);
    }
    public void InsertContentChildItem(ContentItem item, int column, int row)
    {
        if (row < ContentItems[column].Count) //Make space for the item
        {
            for (int i = row; i < ContentItems[column].Count; i++)
            {
                ContentItem underlayingItem = ContentItems[column].GetContentItem(i);
                SetContentItemChildPos(underlayingItem, underlayingItem.Row+1);
            }
        }
        ContentItem parent = ContentItems[column].GetContentItem(1);
        item.Parent = parent;
        item.transform.SetParent(parent.transform);
        SetContentItemChildPos(item, row);
        InsertContentItemBase(item, column, row);
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

    //Todo: finish some day
    public void CalculateBarPositions(ContentItem item)
    {
        dummyVector.Set((item.RectTransform.anchoredPosition.x - scrollRect.viewport.rect.width/2) *-1, (item.RectTransform.anchoredPosition.y) *-1);
        Debug.Log(dummyVector);
        thisRT.anchoredPosition = dummyVector;
        //https://stackoverflow.com/questions/30766020/how-to-scroll-to-a-specific-element-in-scrollrect-with-unity-ui
        //if (scrollRect.horizontalScrollbar.isActiveAndEnabled)
        //{
        //    float pos = item.RectTransform.anchoredPosition.x;
        //    float horizontalPos = Mathf.Clamp(pos / thisRT.sizeDelta.x, 0, 1);
        //    float newPos = pos + (scrollRect.viewport.rect.width * horizontalPos - scrollRect.viewport.rect.width * 0.5f);
        //    float newhorizontalPos = Mathf.Clamp(newPos / thisRT.sizeDelta.x, 0, 1);
        //    Debug.Log(string.Concat(" pos: ", pos, " horPos: ", horizontalPos, " newPos: ", newPos, " newhorpos: ", newhorizontalPos));
        //    scrollRect.horizontalScrollbar.value = newhorizontalPos;
        //}
        //if (scrollRect.verticalScrollbar.isActiveAndEnabled)
        //{
        //    float pos = ColumnHeight(item.Column);
        //    float verticalPos = Mathf.Clamp( pos / thisRT.sizeDelta.y, 0, 1);
        //    float newPos = pos + (scrollRect.viewport.rect.height * verticalPos - scrollRect.viewport.rect.height * 0.5f);
        //    float newverticalPos = Mathf.Clamp(newPos / thisRT.sizeDelta.x, 0, 1);

        //    scrollRect.verticalScrollbar.value = 1 - newverticalPos;
        //}
    }
    private void AlwaysShowRowCount(float barPos)
    {
        float clampedBarPos = Mathf.Clamp(barPos, 0, 1);
        for (int i = 1; i < contentItems[0].Count; i++)
        {
            dummyVector.Set((thisRT.sizeDelta.x * clampedBarPos) - scrollRect.viewport.rect.width * clampedBarPos, contentItems[0].GetContentItem(i).RectTransform.anchoredPosition.y);
            if (dummyVector.x < 0) dummyVector.x = 0;
            contentItems[0].GetContentItem(i).RectTransform.anchoredPosition = dummyVector;
        }
    }
    private void AlwaysShowColumnCountAndPeopleNames(float barPos)
    {
        float clampedBarPos = 1 - Mathf.Clamp(barPos, 0, 1);
        for (int i = 1; i < contentItems.Count; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                if (contentItems[i].GetContentItem(j) == null) break;
                float y = (oneRowHeight * j + (thisRT.sizeDelta.y * clampedBarPos) - scrollRect.viewport.rect.height * clampedBarPos) * -1;
                dummyVector.Set(contentItems[i].GetContentItem(j).RectTransform.anchoredPosition.x, y);
                if (dummyVector.y > 0) dummyVector.y = 0;
                contentItems[i].GetContentItem(j).RectTransform.anchoredPosition = dummyVector;
            }
        }
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
                SetContentItemChildPos(ContentItems[column].GetContentItem(i), i-1);
            }
        }
        ContentItems[column].GetContentItem(row).SetActive(false);
        ContentItems[column].RemoveAt(row);

        ChangeHeightLogic(column);
    }

    private void SetContentItemParentPos(ContentItem item, int column, int row)
    {
        dummyVector.Set(oneColumnWidth * column, (oneRowHeight * row) * -1); //down is negative, so gotta flip
        item.RectTransform.anchoredPosition = dummyVector;
        item.Column = column;
        item.Row = row;
    }
    private void SetContentItemChildPos(ContentItem item, int row)
    {
        dummyVector.Set(0, (oneRowHeight * (row-1)) * -1); //Row-1 because it is parented
        item.RectTransform.anchoredPosition = dummyVector;
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

        if (newHeight > ContentItems[longestColumn].columnHeight)
        {
            longestColumn = column;
            ChangeHeight(newHeight);
        }
        else
        {
            if(column == longestColumn)
            {
                ContentItems[column].columnHeight = newHeight; //Have to change height before looking for heighest column
                FindHeighestColumn();
            }
        }
        ContentItems[column].columnHeight = newHeight;

    }
    private void ChangeHeight(float newHeight)
    {
        dummyVector.Set(thisRT.sizeDelta.x, newHeight);
        thisRT.sizeDelta = dummyVector;
    }
    private void ChangeWidth(float newWidth)
    {
        dummyVector.Set(newWidth, thisRT.sizeDelta.y);
        thisRT.sizeDelta = dummyVector;
    }
    private void ChangeHeightWidth(float newWidth, float newHeight)
    {
        dummyVector.Set(newWidth, newHeight);
        thisRT.sizeDelta = dummyVector;
    }
}

public class Column
{
    public float columnHeight = 0;
    public bool isUsed = false;
    public List<ContentItem> contentItems = new List<ContentItem>();

    public int Count { get => contentItems.Count; }
    public void Add(ContentItem item)
    {
        contentItems.Add(item);
    }
    public void AddRange(IEnumerable<ContentItem> range)
    {
        contentItems.AddRange(range);
    }
    public void Insert(int i, ContentItem item)
    {
        contentItems.Insert(i, item);
    }
    public void RemoveAt(int i)
    {
        contentItems.RemoveAt(i);
    }
    public void RemoveRange(int index, int count)
    {
        contentItems.RemoveRange(index, count);
    }

    public ContentItem GetContentItem(int index)
    {
        if (index >= contentItems.Count) return null;
        return contentItems[index];
    }
    public ContentItem GetCalculationContentItem()
    {
        return contentItems[contentItems.Count-1];
    }

    public IEnumerable<ContentItem> ReturnRange(int skip, int take)
    {
        return contentItems.Skip(skip).Take(take);
    }

}


