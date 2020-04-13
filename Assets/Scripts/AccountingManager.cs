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
    private uint amountOfPeople = 0;

    public ContentItem ContentItem { get => contentItem.GetComponent<ContentItem>(); }

    private void Awake()
    {
        SaveSystem.Init();
    }
    private void Start()
    {
        contentLogic.Init(this);
        contentLogic.HeightChangedHandler += RowCountHandler;
        AddColumn();
    }

    private void RowCountHandler(int rowCount)
    {
        var rowCountAmount = contentLogic.ContentItems[0].Count;
        if (rowCount > rowCountAmount)
        {
            AddRow(rowCount);
        }
        else if (rowCount < rowCountAmount)
        {
            RemoveContentItem(0, rowCountAmount);
        }
    }

    private void AddRow(int row)
    {
        InsertContentItem(CreateContentItem(TMPro.TMP_InputField.ContentType.IntegerNumber, row.ToString(), false), 0, row);
    }
    public void AddColumn()
    {
        contentLogic.AddColumn();
    }
    private void RemoveColumn(int column)
    {
        contentLogic.RemoveColumn(column);
    }
    private void InsertContentItem(ContentItem contentItem, int column, int row, string itemJsonData = null)
    {
        if (itemJsonData != null) contentItem.SetData(itemJsonData);

        contentLogic.InsertContentItem(contentItem, column, row);
    }
    private ContentItem CreateContentItem(TMPro.TMP_InputField.ContentType type, string text, bool interactible)
    {
        var contentItem = this.contentItem.GetInstance(contentLogic.transform).GetComponent<ContentItem>();
        contentItem.SetType(type);
        contentItem.SetText(text);
        contentItem.SetInteractable(interactible);
        return contentItem;
    }
    private ContentItem CreateContentItem(string jsonData)
    {
        var contentItem = this.contentItem.GetInstance(contentLogic.transform).GetComponent<ContentItem>();
        contentItem.SetData(jsonData);
        return contentItem;
    }
    private void RemoveContentItem(int column, int row)
    {
        contentLogic.RemoveContentItem(new Vector2Int(column, row));
    }

    public void AddPerson()
    {
        amountOfPeople++;
        AddColumn();
    }
    public void DeletePerson()
    {
        amountOfPeople--;
    }

    public void Save(string saveName)
    {
        SaveObject saveobj = new SaveObject(contentLogic.ContentItems, amountOfPeople);
        string json = JsonUtility.ToJson(saveobj);
        SaveSystem.Save(saveName + ".json", json);
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
        this.amountOfPeople = saveobj.amountOfPeople;

        for (int i = 0; i < saveobj.columnRowAmount.Count; i++)
        {
            AddColumn();
            for (int j = 0; j < saveobj.columnRowAmount[i]; j++)
            {
                InsertContentItem(CreateContentItem(saveobj.contentItemData[j]), i, j);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Load("");
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
    public uint amountOfPeople;

    public SaveObject(List<Column> list, uint amountOfPeople)
    {
        this.amountOfPeople = amountOfPeople;
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
