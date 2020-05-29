using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using System;
using System.Linq;
using TMPro;

public class AccountingManager : MonoBehaviour
{
    public enum Months { January, February, March, April, May, June, July, August, September, October, November, December }

    [SerializeField] private ContentLogic contentLogic;

    [SerializeField] private PopupSystem popupSystem;

    [SerializeField] private InformationResultManager informationResultManager;

    [SerializeField] private ScrollRect mainScroll;

    [SerializeField] private DropdownMonths monthDropDown;

    [SerializeField] private PoolObject contentInputItem;

    [SerializeField] private PoolObject contentButtonItem;

    [SerializeField] private TMP_InputValidator customValidateDoubleWithComma;

    private List<string> peopleAdded = new List<string>();

    private void Awake()
    {
        SaveSystem.Init();
    }
    private void Start()
    {
        contentLogic.Init(this);
        monthDropDown.Init(this);
        popupSystem.Init(this);

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
        var item = CreateContentItemInput(TMPro.TMP_InputField.ContentType.IntegerNumber, column.ToString(), false);
        item.Savable = false;
        InsertContentItem(item, column, 0);
    }
    private void AddRow(int row)
    {
        var item = CreateContentItemInput(TMPro.TMP_InputField.ContentType.IntegerNumber, row.ToString(), false);
        item.Savable = false;
        InsertContentItem(item, 0, row);
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

    public void RemoveContentItem(ContentItem item)
    {
        contentLogic.RemoveContentItem(item.Column, item.Row);
    }

    //PersonCode start
    public void AddPerson()
    {
        AddPerson("Person " + peopleAdded.Count);
    }
    public void AddPerson(string name)
    {
        peopleAdded.Add(name);

        var item = CreateContentItemInput(TMP_InputField.ContentType.Standard, name, true);
        item.Savable = false;
        item.InputField.onEndEdit.AddListener(delegate { UpdatePersonName(item); });
        if (peopleAdded.Count >= contentLogic.ContentItems.Count) AddColumn(peopleAdded.Count);
        item.MakeDeleteable(DeletePerson);
        InsertContentItem(item, peopleAdded.Count, 1);
        AddRowButton(peopleAdded.Count, contentLogic.ContentItems[peopleAdded.Count].Count);
        AddCalculationResultButton(peopleAdded.Count, contentLogic.ContentItems[peopleAdded.Count].Count);
        //contentLogic.CalculateBarPositions(item);
    }
    public void UpdatePersonName(ContentItem item)
    {
    }
    public void DeletePerson(ContentItem item)
    {
        RemoveColumn(item.Column);
        peopleAdded.RemoveAt(item.Column - 1);
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
    private void AddCalculationResultButton(int column, int row)
    {
        var item = CreateContentItemInput(TMP_InputField.ContentType.DecimalNumber, "0", true);
        item.InputField.readOnly = true;
        item.InputField.image.color += Color.blue;
        item.Savable = false;
        item.SetActive(true);

        InsertContentItem(item, column, row);
    }
    //PersonCode end
    public void AddFinance(int column, int row)
    {
        AddFinance(column, row, "0");
    }
    public void AddFinance(int column, int row, string text)
    {
        var item = CreateContentItemInput(TMP_InputField.ContentType.Custom, text, true);
        item.Savable = true;
        item.InputField.onValidateInput = delegate (string input, int charIndex, char addedChar) { return FinanceValidation(addedChar); };
        item.InputField.onEndEdit.AddListener((x)=>UpdateFinance(item));
        item.MakeDeleteable((x)=> { RemoveContentItem(x); UpdateFinance(x);  });
        if (contentLogic.ContentItems[column].Count >= contentLogic.ContentItems[0].Count) AddRow(contentLogic.ContentItems[column].Count);
        InsertContentItem(item, column, row);
        //contentLogic.CalculateBarPositions(item);
    }
    private char FinanceValidation(char charToValidate)
    {
        if (!char.IsNumber(charToValidate) && charToValidate != ',')
        {
            // ... if it is change it to an empty character.
            charToValidate = '\0';
        }
        return charToValidate;
    }
    public void UpdateFinance(ContentItemInputField item)
    {
        double fallback = 0;
        if (!double.TryParse(item.GetData(), out fallback))
        {
            item.SetText("0");
        }
        CalculateAndSetFinance(item.Column);
    }
    private void CalculateAndSetFinance(int column)
    {
        double result = contentLogic.ContentItems[column].contentItems.Where(x => x.Savable).Sum(x => double.Parse(x.GetData()));
        ContentItemInputField calcItem = (ContentItemInputField)contentLogic.ContentItems[column].GetLastContentItem();
        calcItem.SetText(result.ToString());
    }
    

    public void SaveMonth(string month, string year)
    {
        SaveObject saveobj = new SaveObject(contentLogic.ContentItems, peopleAdded);
        string json = JsonUtility.ToJson(saveobj);
        string name = string.Concat(year, " ", month);
        if (!SaveSystem.Save(name + ".json", json))//Save does not override anything, meaning that month must be added
        {
            monthDropDown.AddMonth(name); 
        }
        monthDropDown.ShowMonth(name);
    }

    public void SaveMonthOnClick()
    {
        if (monthDropDown.IndexValue == 0)
        {
            popupSystem.PopupSaveMonth();
        }
        else
        {
            popupSystem.PopupConfirmation
            ("you want to override save?",
            () =>
            {
                var seperatedYearAndMonth = monthDropDown.DropdownCurrentText.Split(' ');
                SaveMonth(seperatedYearAndMonth[1], seperatedYearAndMonth[0]);
            });
        }
        
    }

    public void LoadMonth(string yearAndMonth)
    {
        Debug.Log("loading: " +yearAndMonth);
        string loadedString = SaveSystem.Load(yearAndMonth + ".json");
        Debug.Log("loaded: " +loadedString);
        if (loadedString == null)
        {
            ClearUIExceptCounters();
        }
        else
        {
            var loadedFile = JsonUtility.FromJson<SaveObject>(loadedString);
            PopulateUI(loadedFile);
        }
        
    }
    public void DeleteMonth(string yearAndMonth)
    {
        SaveSystem.DeleteSave(yearAndMonth + ".json");
        ClearUIExceptCounters();
    }
    public void DeleteCurrentMonth()
    {
        popupSystem.PopupConfirmation
            ("you want to delete month?",
            ()=>
            {
                DeleteMonth(monthDropDown.DropdownCurrentText);
                monthDropDown.DeleteCurrentMonth();
            });
        
    }

    private void PopulateUI(SaveObject saveobj)
    {
        ClearUIExceptCounters();
        int contentCounter = 0;
        for (int i = 0; i < saveobj.peopleAdded.Count; i++)
        {
            AddPerson(saveobj.peopleAdded[i]);
            for (int j = 1; j <= saveobj.columnRowAmount[i+1]; j++)
            {
                AddFinance(i+1, j +1, saveobj.contentItemData[contentCounter]);
                contentCounter++;
            }
            CalculateAndSetFinance(i + 1);
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

    public void Calculate()
    {
        float result = 2530;
        informationResultManager.AddResult("person1", "person2", result);
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

        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    SaveMonth("August", "2020");
        //}
        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    ClearUIExceptCounters();
        //}
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
            columnRowAmount.Add(0); //because we aren't counting the rows and column counters. would be a waste since they are made automatically anyway
            for (int j = 1; j < list[i].Count; j++)
            {
                ContentItem item = list[i].GetContentItem(j);
                if (!item.Savable) continue;
                contentItemData.Add(item.GetData());
                columnRowAmount[i]++;
            }
        }
        
    }
}
