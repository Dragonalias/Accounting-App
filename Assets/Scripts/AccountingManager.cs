using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountingManager : MonoBehaviour
{
    [SerializeField]
    private ContentLogic contentLogic;

    [SerializeField]
    private PoolObject ContentItem;

    private int column = 0;

    private void Start()
    {
        contentLogic.Init(this);
        AddColumn();
    }

    public void AddColumn()
    {
        contentLogic.AddColumn();
    }
    public void AddContentItem(int column)
    {
        var contentItem = ContentItem.GetInstance(contentLogic.transform).GetComponent<ContentItem>();
        contentLogic.AddContentItem(contentItem, column);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddContentItem(column);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            AddColumn();
            column++;
        }
    }
}
