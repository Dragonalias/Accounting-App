using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebtTracking
{
    public float debtOwedTo;
    public float debtOwedFrom;
}

public class Person
{
    public string name;
    public ContentItemPerson connectedContentItem;
    public Dictionary<string, DebtTracking> debtTracking = new Dictionary<string, DebtTracking>();
    public List<Interpay> connectedColumns = new List<Interpay>();

    public Person(string name)
    {
        this.name = name;
    }

    public void UpdateName(string newName)
    {
        if (connectedColumns.Count > 0)
        {
            for (int i = 0; i < connectedColumns.Count; i++)
            {
                connectedColumns[i].UpdateName(name, newName);
            }
        }
        
        name = newName;
    }

    public void UpdateMenuWithNewPerson(Person newPerson = null)
    {
        connectedContentItem.UpdateMenuWithNewPerson(newPerson);
    }
    public void DeleteButtonFromMenu(Person deletePerson)
    {
        connectedContentItem.RemoveButton(deletePerson);
    }
    public void ClearMenu()
    {
        connectedContentItem.ClearMenu();
    }

    public void RemoveConnectedColumn(Interpay item)
    {
        connectedColumns.Remove(item);
    }
}
public class Interpay
{
    public string name;
    public Person paidFor;
    public Person isBeingPaidFor;
    public ContentItemInputField connectedContentItem;

    public Interpay()
    {
    }

    public Interpay(string name, Person paidFor, Person isBeingPaidFor = null)
    {
        this.name = name;
        this.isBeingPaidFor = isBeingPaidFor;
        this.paidFor = paidFor;
    }
    public void UpdateName(string oldName, string newName)
    {
        name = name.Replace(oldName, newName);
        connectedContentItem.SetText(name);
    }
}

[System.Serializable]
public struct InterpaySaveObject
{
    public int paidFor;
    public int isBeingPaidFor;

    public InterpaySaveObject(int paidFor, int isBeingPaidFor)
    {
        this.paidFor = paidFor;
        this.isBeingPaidFor = isBeingPaidFor;
    }
}
