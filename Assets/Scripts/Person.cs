using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person
{
    public string name;
    public ContentItemPerson connectedContentItem;
    public List<Interpay> connectedColumns = new List<Interpay>();

    private Dictionary<string, float> debtBook = new Dictionary<string, float>();

    public Person(string name)
    {
        this.name = name;
    }

    public void WriteDebt(string otherPerson, float amount)
    {
        if (!debtBook.ContainsKey(otherPerson))
        {
            debtBook.Add(otherPerson, amount);
        }
        else
        {
            debtBook[otherPerson] += amount;
        }
    }

    public int GetDebtAmount()
    {
        return debtBook.Count;
    }

    public float GetAmount(string otherPerson)
    {
        if (debtBook.ContainsKey(otherPerson))
        {
            return debtBook[otherPerson];
        }
        return 0;
    }

    public void ClearAllDebt()
    {
        debtBook.Clear();
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
