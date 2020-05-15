using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public class AccountingManager : MonoBehaviour
{
    [SerializeField] private ContentLogic contentLogic;

    [SerializeField] private ScrollRect mainScroll;

    [SerializeField] private TMPro.TMP_Dropdown monthDropDown;

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
        List<string> dropdownList = new List<string>();
        dropdownList.AddRange(Directory.GetFiles(SaveSystem.SAVE_FOLDER, "*.json"));
        for (int i = 0; i < dropdownList.Count; i++)
        {
            dropdownList[i] = dropdownList[i].Replace(".json", "");
            dropdownList[i] = Path.GetFileName(dropdownList[i]);
        }
        monthDropDown.AddOptions(dropdownList);
        monthDropDown.onValueChanged.AddListener(delegate { LoadMonth(monthDropDown.captionText.text); });
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
        contentItem.GetComponent<FixScroll>().MainScroll = mainScroll;
        contentItem.SetActive(true);
        contentItem.SetType(type);
        contentItem.SetText(text);
        contentItem.SetInteractable(interactible);
        return contentItem;
    }
    private ContentItemInputField CreateContentItem(string jsonData)
    {
        var contentItem = contentInputItem.GetInstance(contentLogic.transform).GetComponent<ContentItemInputField>();
        contentItem.GetComponent<FixScroll>().MainScroll = mainScroll;
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
        AddPerson("Person " + peopleAdded.Count);
    }
    public void AddPerson(string name)
    {
        peopleAdded.Add(name);

        var item = CreateContentItemInput(TMPro.TMP_InputField.ContentType.Name, name, true);
        item.Savable = false;
        item.InputField.onEndEdit.AddListener(delegate { UpdatePersonName(item.InputField.text); });
        if (peopleAdded.Count >= contentLogic.ContentItems.Count) AddColumn(peopleAdded.Count);
        item.MakeDeleteable(DeletePerson);
        InsertContentItem(item, peopleAdded.Count, 1);
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
        item.Savable = false;
        item.SetActive(true);
        item.MakeClickable(AddFinance);
        InsertContentItem(item, column, row);
    }

    public void AddFinance(int column, int row)
    {
        AddFinance(column, row, "0");
    }
    public void AddFinance(int column, int row, string text)
    {
        var item = CreateContentItemInput(TMPro.TMP_InputField.ContentType.DecimalNumber, text, true);
        item.Savable = true;
        item.InputField.onEndEdit.AddListener(delegate { UpdateFinance(item.InputField.text); });
        item.MakeDeleteable(RemoveContentItem);
        if (contentLogic.ContentItems[column].Count >= contentLogic.ContentItems[0].Count) AddRow(contentLogic.ContentItems[column].Count);
        InsertContentItem(item, column, row);
    }
    public void UpdateFinance(string number)
    {
        Debug.Log(number);
    }
    

    public void SaveMonth(string monthAndYear)
    {
        SaveObject saveobj = new SaveObject(contentLogic.ContentItems, peopleAdded);
        string json = JsonUtility.ToJson(saveobj);
        SaveSystem.Save(monthAndYear + ".json", json);
        Debug.Log("SAving: " + json);
    }

    public void LoadMonth(string monthAndYear)
    {
        Debug.Log(monthAndYear);
        string loadedString = SaveSystem.Load(monthAndYear + ".json");
        Debug.Log("loaded: " +loadedString);
        var loadedFile = JsonUtility.FromJson<SaveObject>(loadedString);

        PopulateUI(loadedFile);
    }
    private void PopulateUI(SaveObject saveobj)
    {
        ClearUIExceptCounters();

        for (int i = 0; i < saveobj.peopleAdded.Count; i++)
        {
            AddPerson(saveobj.peopleAdded[i]);
            for (int j = 1; j <= saveobj.columnRowAmount[i+1]; j++)
            {
                AddFinance(i+1, j +1, JsonUtility.FromJson<ContentInputFieldSaveData>(saveobj.contentItemData[j]).textData);
            }
            
        }
    }
    private void ClearUIExceptCounters()
    {
        peopleAdded.Clear();
        for (int i = contentLogic.ContentItems.Count - 1; i >= 1; i--)
        {
            RemoveColumn(i);
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
                    Debug.Log("column: "+ i + "row: " +j+ "type: "+ contentLogic.ContentItems[i].GetContentItem(j));
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            SaveMonth("April");
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            ClearUIExceptCounters();
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
        columnRowAmount.Add(0); //because we aren't counting the rows and column counters. would be a waste since they are made automatically anyway

        for (int i = 1; i < list.Count; i++)
        {
            columnRowAmount.Add(0);
            for (int j = 1; j < list[i].Count; j++)
            {
                ContentItem item = list[i].GetContentItem(j);
                Debug.Log(item);
                if (!item.Savable) continue;
                contentItemData.Add(item.GetData());
                columnRowAmount[i]++;
            }
        }
        
    }
}
