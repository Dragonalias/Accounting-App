using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Networking.PlayerConnection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

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

        if (manager.PeopleAdded.Count > 1)
        {
            AddPerson();
            for (int i = 0; i < manager.PeopleAdded.Count - 1; i++)
            {
                AddPerson(manager.PeopleAdded[i]);
            }
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

    public void UpdateMenuWithNewPerson(Person newPerson)
    {
        if (btnList.Count == 0)
        {
            AddPerson();
        }

        AddPerson(newPerson);
    }

    private void AddPerson(Person newPerson = null)
    {
        btnList.Add(newPerson);
    }

    private void MakeButton(Person otherPerson = null)
    {
        var btn = btnPrefab.GetInstance(transform).GetComponent<InterpayInstantiatorButton>();
        btn.Activate();
        string buttonLabel;
        string columnLabel;
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
        btn.buttonText.text = buttonLabel;
        btn.thisButton.onClick.AddListener(
            () =>
            {
                manager.CreateInterpay(columnLabel, personContentItem.ConnectedPerson, otherPerson);
                btnList.Remove(otherPerson);
                Off();
            });
    }

    public void RemoveButton(Person deletePerson)
    {
        btnList.Remove(deletePerson);
    }

    public void On()
    {
        parent.SetActive(true);
        for (int i = 0; i < btnList.Count; i++)
        {
            MakeButton(btnList[i]);
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
}
