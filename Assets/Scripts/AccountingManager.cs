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
    private void InsertContentItem(int column, int row, string itemData = null)
    {
        var contentItem = this.contentItem.GetInstance(contentLogic.transform).GetComponent<ContentItem>();
        contentItem.SetActive(true);
        if (itemData != null) contentItem.SetData(itemData);
        contentLogic.InsertContentItem(contentItem, column, row);
    }

    public void AddPeople()
    {

    }

    public void Save()
    {
        SaveObject saveobj = new SaveObject(contentLogic.ContentItems);
        string json = JsonUtility.ToJson(saveobj);
        SaveSystem.Save("save.json", json);
        Debug.Log("SAving: " + json);
    }

    public void Load(string monthAndYear)
    {
        string loadedString = SaveSystem.Load("save.json");
        Debug.Log("loaded: " +loadedString);
        var loadedFile = JsonUtility.FromJson<SaveObject>(loadedString);

        PopulateUI(loadedFile);
    }
    private void PopulateUI(SaveObject saveobj)
    {
        for (int i = 0; i < saveobj.columnRowAmount.Count; i++)
        {
            AddColumn();
            for (int j = 0; j < saveobj.columnRowAmount[i]; j++)
            {
                InsertContentItem(i, j, saveobj.contentItemData[j]);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            InsertContentItem(column, contentLogic.ContentItems[column].Count);
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

[System.Serializable]
public class SaveObject
{
    public List<int> columnRowAmount;
    public List<string> contentItemData;

    public SaveObject(List<Column> list)
    {
        columnRowAmount = new List<int>();
        contentItemData = new List<string>();

        for (int i = 0; i < list.Count; i++)
        {
            columnRowAmount.Add(list[i].Count);
            for (int j = 0; j < list[i].Count; j++)
            {
                contentItemData.Add(list[i].GetContentItem(j).GetData());
            }
        }
        
    }
}
