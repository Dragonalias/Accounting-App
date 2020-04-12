using UnityEngine;
using System.Collections.Generic;


public class AccountingManager : MonoBehaviour
{
    [SerializeField]
    private ContentLogic contentLogic;

    [SerializeField]
    private PoolObject contentItem;

    [SerializeField]
    private GameObject MainMenu;
    [SerializeField]
    private GameObject AddPeopleMenu;

    private int column = 0;

    public ContentItem ContentItem { get => contentItem.GetComponent<ContentItem>(); }

    private void Awake()
    {
        SaveSystem.Init();
    }
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
    private void InsertContentItem(int column, int row, ContentItem item = null)
    {
        var contentItem = this.contentItem.GetInstance(contentLogic.transform).GetComponent<ContentItem>();
        contentItem.SetActive(true);
        if (item != null) contentItem = item;
        contentLogic.InsertContentItem(contentItem, column, row);
    }

    public void AddPeople()
    {

    }

    public void Save()
    {
        string json = JsonUtility.ToJson(contentLogic.contentItems);
        SaveSystem.Save("save.json", json);
        Debug.Log("SAving: " + json);
    }

    public void Load(string monthAndYear)
    {
        string loadedString = SaveSystem.Load("save.json");
        Debug.Log("loaded: " +loadedString);
        var loadedFile = JsonUtility.FromJson<List<Column>>(loadedString);

        PopulateUI(loadedFile);
    }
    private void PopulateUI(List<Column> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            AddColumn();
            for (int j = 0; j < list[i].Count; j++)
            {
                InsertContentItem(i, j, list[i].GetContentItem(j));
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            InsertContentItem(column, contentLogic.contentItems[column].Count);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Load("");
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Save();
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
