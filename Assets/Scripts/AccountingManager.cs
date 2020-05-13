using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class AccountingManager : MonoBehaviour
{
    [SerializeField] private ContentLogic contentLogic;

    [SerializeField] private ScrollRect MainScroll;

    [SerializeField] private PoolObject contentInputItem;

    [SerializeField] private PoolObject contentButtonItem;

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
        InsertContentItem(CreateContentItemInput(TMPro.TMP_InputField.ContentType.IntegerNumber, column.ToString(), false), column, 0);
    }
    private void AddRow(int row)
    {
        InsertContentItem(CreateContentItemInput(TMPro.TMP_InputField.ContentType.IntegerNumber, row.ToString(), false), 0, row);
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
    private ContentItemInputField CreateContentItemInput(TMPro.TMP_InputField.ContentType type, string text, bool interactible)
    {
        var contentItem = contentInputItem.GetInstance(contentLogic.transform).GetComponent<ContentItemInputField>();
        contentItem.GetComponent<FixScroll>().MainScroll = MainScroll;
        contentItem.SetActive(true);
        contentItem.SetType(type);
        contentItem.SetText(text);
        contentItem.SetInteractable(interactible);
        return contentItem;
    }
    private ContentItemInputField CreateContentItem(string jsonData)
    {
        var contentItem = contentInputItem.GetInstance(contentLogic.transform).GetComponent<ContentItemInputField>();
        contentItem.GetComponent<FixScroll>().MainScroll = MainScroll;
        contentItem.SetActive(true);
        contentItem.SetData(jsonData);
        return contentItem;
    }
    public void RemoveContentItem(int column, int row)
    {
        contentLogic.RemoveContentItem(column, row);
    }

    public void AddPerson()
    {
        peopleAdded.Add("Person " + peopleAdded.Count);

        var item = CreateContentItemInput(TMPro.TMP_InputField.ContentType.Name, "Person " + peopleAdded.Count, true);
        item.InputField.onEndEdit.AddListener( delegate { UpdatePersonName(item.InputField.text); });
        if (peopleAdded.Count >= contentLogic.ContentItems.Count) AddColumn(peopleAdded.Count);
        item.MakeDeleteable(DeletePerson);
        InsertContentItem(item, peopleAdded.Count, contentLogic.ContentItems[peopleAdded.Count].Count);
        AddRowButton(peopleAdded.Count, contentLogic.ContentItems[peopleAdded.Count].Count);
    }
    public void UpdatePersonName(string name)
    {
        Debug.Log(name);
    }
    public void DeletePerson(int column, int row)
    {
        RemoveColumn(column);
        peopleAdded.RemoveAt(column - 1);
        //Something more here to update all names
    }

    private void AddRowButton(int column, int row)
    {
        var item = contentButtonItem.GetInstance(contentLogic.transform).GetComponent<ContentItemButton>();
        item.SetActive(true);
        item.MakeClickable(AddFinance);
        InsertContentItem(item, column, row);
    }

    public void AddFinance(int column, int row)
    {
        var item = CreateContentItemInput(TMPro.TMP_InputField.ContentType.DecimalNumber, "0", true);
        item.InputField.onEndEdit.AddListener(delegate { UpdateFinance(item.InputField.text); });
        item.MakeDeleteable(RemoveContentItem);
        if (contentLogic.ContentItems[column].Count >= contentLogic.ContentItems[0].Count) AddRow(contentLogic.ContentItems[column].Count);
        InsertContentItem(item, column, row);
    }
    public void UpdateFinance(string number)
    {
        Debug.Log(number);
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
            for (int i = 0; i < contentLogic.ContentItems.Count; i++)
            {
                for (int j = 0; j < contentLogic.ContentItems[i].Count; j++)
                {
                    Debug.Log("column: "+ i + "row: " +j+ "type: "+ contentLogic.ContentItems[i].GetContentItem(j).GetContentType());
                }
            }
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
                contentItemData.Add(list[i].GetContentItem(j).GetData()); //TODO: accomadate buttons
            }
        }
        
    }
}
