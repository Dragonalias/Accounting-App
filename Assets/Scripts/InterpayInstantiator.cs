using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InterpayInstantiator : MonoBehaviour
{
    [SerializeField] private PoolObject btnPrefab;
    [SerializeField] private GameObject parent;

    private List<Person> btnList = new List<Person>();
    private AccountingManager manager;
    private ContentItemPerson personContentItem;
    private GameObject thisGameobject;
    private EventSystem currentES;

    public void Init(AccountingManager manager, ContentItemPerson personContentItem)
    {
        this.manager = manager;
        this.personContentItem = personContentItem;
        thisGameobject = gameObject;
        currentES = EventSystem.current;

        AddPerson();
        for (int i = 0; i < manager.PeopleAdded.Count - 1; i++)
        {
            AddPerson(manager.PeopleAdded[i]);
        }
        
    }

    //Wonky af, but its the best way i could find to hide this when clicking away while also being able to click button
    private void Update()
    {
        if(currentES.currentSelectedGameObject == null || (currentES.currentSelectedGameObject != thisGameobject && !currentES.currentSelectedGameObject.TryGetComponent(out InterpayInstantiatorButton dummy)))
        {
            Off();
        }
    }

    public void UpdateMenuWithNewPerson(Person newPerson = null)
    {
        AddPerson(newPerson);
    }

    private void AddPerson(Person newPerson = null)
    {
        if (newPerson != null)
        {
            btnList.Add(newPerson);
        }
        else btnList.Insert(0, newPerson);
    }

    private void MakeButton(Person otherPerson = null)
    {
        var btn = btnPrefab.GetInstance(transform).GetComponent<InterpayInstantiatorButton>();
        btn.Init();
        string buttonLabel;
        string columnLabel;
        GetLabels(out buttonLabel, out columnLabel, otherPerson);
        btn.buttonText.text = buttonLabel;
        btn.thisButton.onClick.AddListener(
            () =>
            {
                OnClick(columnLabel, otherPerson);
                Off();
            });
    }
    private void OnClick(string columnLabel, Person otherPerson = null)
    {
        manager.CreateInterpay(columnLabel, personContentItem.ConnectedPerson, otherPerson);
        btnList.Remove(otherPerson);
    }

    public void SimulatedClick(Person otherPerson = null)
    {
        string columnLabel;
        GetLabels(out string buttonLabel, out columnLabel, otherPerson);
        OnClick(columnLabel, otherPerson);
    }

    public void RemoveButton(Person deletePerson)
    {
        btnList.Remove(deletePerson);
    }

    public void On()
    {
        parent.SetActive(true);

        if (manager.PeopleAdded.Count > 1)
        {
            for (int i = 0; i < btnList.Count; i++)
            {
                MakeButton(btnList[i]);
            }
        }
        
    }
    public void Off()
    {
        parent.SetActive(false);
        transform.DetachChildren();
    }

    public void ResetMenu()
    {
        btnList.Clear();
    }

    private void GetLabels(out string buttonLabel, out string columnLabel, Person otherPerson = null)
    {
        if (otherPerson != null)
        {
            buttonLabel = string.Concat("Paid for ", otherPerson.name);
            columnLabel = string.Concat(personContentItem.ConnectedPerson.name, "\n paid for \n", otherPerson.name);
        }
        else
        {
            buttonLabel = "Paid for Joint";
            columnLabel = string.Concat(personContentItem.ConnectedPerson.name, "\n paid for \n", "Joint");
        }
    }
}
