﻿using System.Collections;
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
}
public class Interpay
{
    public string name;
    public ContentItemInputField connectedContentItem;

    public Interpay()
    {
    }

    public Interpay(string name)
    {
        this.name = name;
    }
    public void UpdateName(string oldName, string newName)
    {
        name = name.Replace(oldName, newName);
        Debug.Log(string.Concat("newname: ",newName, " oldname: ", oldName, " name: ", name));
        connectedContentItem.SetText(name);
    }
}
