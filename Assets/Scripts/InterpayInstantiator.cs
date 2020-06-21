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

    private List<BtnRebuilder> btnList = new List<BtnRebuilder>();
    private AccountingManager manager;
    private ContentItemPerson personContentItem;


    private struct BtnRebuilder
    {
        public string buttonLabel;
        public string columnLabel;
        public Person otherPerson;
    }
    public void Init(AccountingManager manager, ContentItemPerson personContentItem)
    {
        this.manager = manager;
        this.personContentItem = personContentItem;

        if (manager.PeopleAdded.Count > 1)
        {
            MakeBtnRebuilder();
            for (int i = 0; i < manager.PeopleAdded.Count - 1; i++)
            {
                MakeBtnRebuilder(manager.PeopleAdded[i]);
            }
        }
    }

    public void UpdateMenuWithNewPerson(Person newPerson)
    {
        if (btnList.Count == 0)
        {
            MakeBtnRebuilder();
        }

        MakeBtnRebuilder(newPerson);
    }

    private void MakeBtnRebuilder(Person newPerson = null)
    {
        BtnRebuilder btnRebuilder = new BtnRebuilder();
        Person thisPerson = personContentItem.ConnectedPerson;
        if (newPerson != null)
        {
            btnRebuilder.buttonLabel = string.Concat("Paid for ", newPerson.name);
            btnRebuilder.columnLabel = string.Concat(thisPerson.name, "\n paid for \n", newPerson.name);
            btnRebuilder.otherPerson = newPerson;
        }
        else
        {
            btnRebuilder.buttonLabel = "Paid for Joint";
            btnRebuilder.columnLabel = string.Concat(thisPerson.name, "\n paid for \n", "Joint");
        }

        btnList.Add(btnRebuilder);
    }
    private void MakeButtonFromRebuilder(BtnRebuilder btnRebuilder)
    {
        var btn = btnPrefab.GetInstance(transform).GetComponent<InterpayInstantiatorButton>();
        btn.Activate();
        btn.buttonText.text = btnRebuilder.buttonLabel;
        btn.thisButton.onClick.AddListener(
            () =>
            {
                manager.CreateInterpay(btnRebuilder.columnLabel, personContentItem.ConnectedPerson, btnRebuilder.otherPerson);
                btnList.Remove(btnRebuilder);
            });
    }

    public void RemoveButton(Person deletePerson)
    {
        int i = btnList.FindIndex(btn => btn.otherPerson == deletePerson);
        if(i != -1) btnList.RemoveAt(i);
    }

    public void On()
    {
        parent.SetActive(true);
        for (int i = 0; i < btnList.Count; i++)
        {
            MakeButtonFromRebuilder(btnList[i]);
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
