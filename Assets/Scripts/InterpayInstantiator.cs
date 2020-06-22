using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Networking.PlayerConnection;
using UnityEngine;
using UnityEngine.EventSystems;

public class InterpayInstantiator : MonoBehaviour, IDeselectHandler
{
    [SerializeField] private PoolObject btnPrefab;
    [SerializeField] private GameObject parent;

    private List<Person> btnList = new List<Person>();
    private AccountingManager manager;
    private ContentItemPerson personContentItem;

    public void Init(AccountingManager manager, ContentItemPerson personContentItem)
    {
        this.manager = manager;
        this.personContentItem = personContentItem;

        if (manager.PeopleAdded.Count > 1)
        {
            AddPerson();
            for (int i = 0; i < manager.PeopleAdded.Count - 1; i++)
            {
                AddPerson(manager.PeopleAdded[i]);
            }
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

    public void OnDeselect(BaseEventData eventData)
    {
        StartCoroutine(j());
    }

    IEnumerator j()
    {
        yield return new WaitForSeconds(1);
        Off();
    }
}
