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
    private Vector2 dummyVector;

    public void Init(AccountingManager manager)
    {
        this.manager = manager;
        contentItems = new List<List<ContentItem>>();
        dummyVector = new Vector2();
    }

    public void AddColumn()
    {
        contentItems.Add(new List<ContentItem>());

        ChangeWidth(RowWidth());
    }

    public void AddContentItem(ContentItem item, int column)
    {
        SetContentItemPos(item, column, contentItems[column].Count);
        contentItems[column].Add(item);

        ChangeHeight(ColumnHeight(column));
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
                SetContentItemPos(contentItems[listPos.x][i], listPos.x, i-1);
            }
        }
        contentItems[listPos.x][listPos.y].SetActive(false);
        contentItems[listPos.x].RemoveAt(listPos.y);

        ChangeHeight(ColumnHeight(listPos.x));
    }

    private void SetContentItemPos(ContentItem item, int column, int row)
    {
        dummyVector.Set(item.Width * column + paddingBetweenColumns * column, item.Height * row + paddingBetweenRows * row);
        item.RectTransform.anchoredPosition = dummyVector;
    }

    private float ColumnHeight(int column)
    {
        return contentItems[column].Count * contentItems[column][0].Height + paddingBetweenRows * contentItems[column].Count;
    }
    private float RowWidth()
    {
        return contentItems.Count * contentItems[0][0].Width + paddingBetweenColumns * contentItems.Count;
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
