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

    private List<string> peopleAdded = new List<string>();

    private void Awake()
    {
        SaveSystem.Init();
    }
    private void Start()
    {
        contentLogic.Init(this);
        AddColumn(0);

        for (int i = 1; i < 5; i++)
        {
            AddColumn(i);
            AddRow(i);
        }
    }

    public void AddColumn(int column)
    {
        InsertColumn(column);
        InsertContentItem(CreateContentItem(TMPro.TMP_InputField.ContentType.IntegerNumber, column.ToString(), false), column, 0);
    }
    private void AddRow(int row)
    {
        InsertContentItem(CreateContentItem(TMPro.TMP_InputField.ContentType.IntegerNumber, row.ToString(), false), 0, row);
    }
    public void InsertColumn(int column)
    {
        contentLogic.InsertColumn(column);
    }
    private void RemoveColumn(int column)
    {
        contentLogic.RemoveColumn(column);
    }
    private void InsertContentItem(ContentItem contentItem, int column, int row)
    {
        contentLogic.InsertContentItem(contentItem, column, row);
    }
    private ContentItem CreateContentItem(TMPro.TMP_InputField.ContentType type, string text, bool interactible)
    {
        var contentItem = this.contentItem.GetInstance(contentLogic.transform).GetComponent<ContentItem>();
        contentItem.SetActive(true);
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
        peopleAdded.Add("Person " + peopleAdded.Count);

        var item = CreateContentItem(TMPro.TMP_InputField.ContentType.Name, "Person " + peopleAdded.Count, true);
        item.InputField.onEndEdit.AddListener( delegate { UpdatePersonName(item.InputField.text); });
        Debug.Log(peopleAdded.Count + " " + contentLogic.ContentItems[1].Count);
        InsertContentItem(item, peopleAdded.Count, contentLogic.ContentItems[peopleAdded.Count].Count);
    }
    public void UpdatePersonName(string name)
    {
        Debug.Log(name);
    }
    public void DeletePerson(int column)
    {
        RemoveColumn(column);
        peopleAdded.RemoveAt(column - 1);
        //Something more here to update all names
    }

    public void Save(string saveName)
    {
        SaveObject saveobj = new SaveObject(contentLogic.ContentItems, peopleAdded);
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
        this.peopleAdded = saveobj.peopleAdded;

        for (int i = 0; i < saveobj.columnRowAmount.Count; i++)
        {
            InsertColumn(i);
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
    }
}

[System.Serializable]
public class SaveObject
{
    public List<int> columnRowAmount;
    public List<string> contentItemData;
    public List<string> peopleAdded;

    public SaveObject(List<Column> list, List<string> peopleAdded)
    {
        this.peopleAdded = peopleAdded;
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
