using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
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
    [SerializeField] private PoolObject contentPersonItem;
    [SerializeField] private PoolObject contentButtonItem;

    private List<Person> peopleAdded = new List<Person>();
    private List<Interpay> interPayColumns = new List<Interpay>();

    public List<Person> PeopleAdded { get => peopleAdded; }

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
        if (InsertColumn(column))
        {
            var item = contentInputItem.GetInstance(contentLogic.transform).GetComponent<ContentItemInputField>();
            item.Instantiate(TMP_InputField.ContentType.IntegerNumber, column.ToString(), false, mainScroll);
            item.Savable = false;
            contentLogic.InsertContentCountItem(item, column, 0);
        }
    }
    private void AddRow(int row)
    {
        var item = contentInputItem.GetInstance(contentLogic.transform).GetComponent<ContentItemInputField>();
        item.Instantiate(TMP_InputField.ContentType.IntegerNumber, row.ToString(), false, mainScroll);
        item.Savable = false;
        contentLogic.InsertContentCountItem(item, 0, row);
    }
    public bool InsertColumn(int column)
    {
        return contentLogic.InsertColumn(column);
    }

    public void RemoveContentItem(ContentItem item)
    {
        contentLogic.RemoveContentItem(item.Column, item.Row);
    }

    //PersonCode start
    private void AddColumnBase(ContentItem item, int newColumn)
    {
        AddColumn(newColumn);
        contentLogic.InsertContentParentItem(item, newColumn, 1);
        AddRowButton(newColumn, contentLogic.ContentItems[newColumn].Count);
        AddCalculationResultButton(newColumn, contentLogic.ContentItems[newColumn].Count);
    }
    public void AddPerson()
    {
        AddPerson("Person " + peopleAdded.Count);
    }
    public void AddPerson(string name)
    {
        var person = new Person(name);
        peopleAdded.Add(person);

        var item = contentPersonItem.GetInstance(contentLogic.transform).GetComponent<ContentItemPerson>();
        item.Instantiate(TMP_InputField.ContentType.Standard, name, true, mainScroll);
        item.Savable = false;
        item.Init(this, person);
        item.InputField.onEndEdit.AddListener(delegate { person.UpdateName(item.GetData()); });
        item.MakeDeleteable(DeletePerson);

        AddColumnBase(item, peopleAdded.Count);

        person.connectedContentItem = item;
        if (PeopleAdded.Count > 1) UpdateNewPersonInterpayMenu(person);

        
        //contentLogic.CalculateBarPositions(item);
    }

    private void UpdateNewPersonInterpayMenu(Person newPerson)
    {
        for (int i = 0; i < PeopleAdded.Count -1; i++)
        {
            PeopleAdded[i].UpdateMenuWithNewPerson(newPerson);
        }
    }
    private void DeletePersonInterpayMenu(Person deletePerson)
    {
        for (int i = 0; i < PeopleAdded.Count; i++) // -1 to not update the newly created person
        {
            Person person = PeopleAdded[i];
            if (person == deletePerson) continue;
            PeopleAdded[i].DeleteButtonFromMenu(deletePerson);
        }
    }
    private void AddRestColumns()
    {
        if (peopleAdded.Count <= 1) return; //Cant make interpay columns for only 1 person
        //PersonA paid for joint payment
        //PersonA paid for PersonB
        //PersonB paid for PersonA
        string label;
        List<Person> people = new List<Person>();
        for (int i = peopleAdded.Count - 2; i >= 0; i--)
        {
            people.Add(peopleAdded[peopleAdded.Count - 1]); //This person
            people.Add(peopleAdded[i]); //Other person

            label = string.Concat(people[0].name, "\n paid for \n", people[1].name);
            CreateInterpay(label, people[0], people[1]);

            label = string.Concat(people[1].name, "\n paid for \n", people[0].name);
            CreateInterpay(label, people[1], people[0]);

            people.Clear();
        }
        people.Add(peopleAdded[peopleAdded.Count - 1]);
        label = string.Concat(people[0].name, "\n paid for \n Joint");

        CreateInterpay(label, people[0]);
    }
    public void CreateInterpay(string label, Person thisPerson, Person otherPerson = null)
    {
        var interpay = new Interpay(label, thisPerson, otherPerson);
        thisPerson.connectedColumns.Add(interpay);
        if(otherPerson != null) otherPerson.connectedColumns.Add(interpay);
        AddInterpayItem(interpay);
    }
    private void AddInterpayItem(Interpay interpay)
    {
        interPayColumns.Add(interpay);

        var item = contentInputItem.GetInstance(contentLogic.transform).GetComponent<ContentItemInputField>();
        item.Instantiate(TMP_InputField.ContentType.Standard, interpay.name, false, mainScroll);
        item.Savable = false;
        interpay.connectedContentItem = item;
        AddColumnBase(item, peopleAdded.Count+interPayColumns.Count);
    }
    public void DeletePerson(ContentItem item)
    {
        int column = item.Column;
        Person person = peopleAdded[column - 1];
        if (person.connectedColumns.Count > 0)
        {
            DeleteInterpayItems(person);
        }
        DeletePersonInterpayMenu(person);
        contentLogic.RemoveColumn(column);
        peopleAdded.RemoveAt(column - 1);

        if (peopleAdded.Count == 1) //if theres only 1 person left, it shouldn't have any connectedcolumns
        {
            person = peopleAdded[0];
            person.ClearMenu();
            if (person.connectedColumns.Count > 0)
            {
                DeleteInterpayItems(person);
            }
        }
    }
    private void DeleteInterpayItems(Person person)
    {
        for (int i = person.connectedColumns.Count - 1; i >= 0; i--)
        {
            var cc = person.connectedColumns[i];
            var interPayColumn = cc.connectedContentItem.Column;
            if (cc.isBeingPaidFor != null)
            {
                cc.isBeingPaidFor.RemoveConnectedColumn(cc);
            }
            cc.paidFor.RemoveConnectedColumn(cc);

            interPayColumns.RemoveAt(interPayColumn - 1 - peopleAdded.Count);
            contentLogic.RemoveColumn(interPayColumn);
        }
    }

    private void AddRowButton(int column, int row)
    {
        var item = contentButtonItem.GetInstance(contentLogic.transform).GetComponent<ContentItemButton>();
        item.Savable = false;
        item.SetActive(true);
        item.MakeClickable(AddFinance);
        contentLogic.InsertContentChildItem(item, column, row);
    }
    private void AddCalculationResultButton(int column, int row)
    {
        var item = contentInputItem.GetInstance(contentLogic.transform).GetComponent<ContentItemInputField>();
        item.Instantiate(TMP_InputField.ContentType.DecimalNumber, "0", true, mainScroll);
        item.InputField.readOnly = true;
        item.SetCalculationColor();
        item.Savable = false;
        item.SetActive(true);

        contentLogic.InsertContentChildItem(item, column, row);
    }
    //PersonCode end
    public void AddFinance(int column, int row)
    {
        AddFinance(column, row, "0");
    }
    public void AddFinance(int column, int row, string text)
    {
        var item = contentInputItem.GetInstance(contentLogic.transform).GetComponent<ContentItemInputField>();
        item.Instantiate(TMP_InputField.ContentType.Custom, text, true, mainScroll);
        item.Savable = true;
        item.InputField.onValidateInput = delegate (string input, int charIndex, char addedChar) { return FinanceValidation(addedChar); };
        item.InputField.onEndEdit.AddListener((x)=>UpdateFinance(item));
        item.MakeDeleteable((x)=> { RemoveContentItem(x); UpdateFinance(x);  });
        if (contentLogic.ContentItems[column].Count >= contentLogic.ContentItems[0].Count) AddRow(contentLogic.ContentItems[column].Count);
        contentLogic.InsertContentChildItem(item, column, row);
        //contentLogic.CalculateBarPositions(item);
    }
    private char FinanceValidation(char charToValidate)
    {
        if (!char.IsNumber(charToValidate) && charToValidate != ',')
        {
            // change it to an empty character.
            charToValidate = '\0';
        }
        return charToValidate;
    }
    public void UpdateFinance(ContentItemInputField item)
    {
        if (!float.TryParse(item.GetData(), out float fallback))
        {
            item.SetText("0");
        }
        CalculateAndSetFinance(item.Column);
    }
    private void CalculateAndSetFinance(int column)
    {
        float result = contentLogic.ContentItems[column].contentItems.Where(x => x.Savable).Sum(x => float.Parse(x.GetData()));
        ContentItemInputField calcItem = (ContentItemInputField)contentLogic.ContentItems[column].GetCalculationContentItem();
        calcItem.SetText(result.ToString("#.000"));
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
            contentLogic.RemoveColumn(i);
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

    public SaveObject(List<Column> list, List<Person> peopleAdded)
    {
        this.peopleAdded = peopleAdded.Select(x=>x.name).ToList();
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
