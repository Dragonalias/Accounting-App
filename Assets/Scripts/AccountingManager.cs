using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountingManager : MonoBehaviour
{
    [SerializeField]
    private ContentLogic contentLogic;

    [SerializeField]
    private PoolObject contentItem;

    private int column = 0;

    public ContentItem ContentItem { get => contentItem.GetComponent<ContentItem>(); }

    private void Start()
    {
        contentLogic.Init(this);
        AddColumn();
    }

    public void AddColumn()
    {
        contentLogic.AddColumn();
    }
    private void RemoveColumn(int column)
    {
        contentLogic.RemoveColumn(column);
    }
    public void AddContentItem(int column)
    {
        var contentItem = this.contentItem.GetInstance(contentLogic.transform).GetComponent<ContentItem>();
        contentItem.SetActive(true);
        contentLogic.AddContentItem(contentItem, column);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddContentItem(column);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            column++;
            Debug.Log(Time.deltaTime + " At column: " + column);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            column--;
            Debug.Log(Time.deltaTime + " At column: " + column);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            RemoveColumn(column);
            Debug.Log(Time.deltaTime + " REmoving column " + column);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            AddColumn();
            Debug.Log(Time.deltaTime + " Adding colomn!");
        }
    }

   
}
