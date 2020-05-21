using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;

public class DropdownMonths : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Dropdown dropdown = null;
    private List<string> dropDownSortedList;
    private AccountingManager manager;

    public string DropdownCurrentText => dropdown.captionText.text;
    public int IndexValue => dropdown.value;
    public void Init(AccountingManager manager)
    {
        this.manager = manager;
        
        dropdown.onValueChanged.AddListener(delegate { manager.LoadMonth(dropdown.captionText.text); });

        dropDownSortedList = new List<string>();
        dropDownSortedList.AddRange(Directory.GetFiles(SaveSystem.SAVE_FOLDER, "*.json"));
        for (int i = 0; i < dropDownSortedList.Count; i++)
        {
            dropDownSortedList[i] = dropDownSortedList[i].Replace(".json", "");
            dropDownSortedList[i] = Path.GetFileName(dropDownSortedList[i]);
        }
        ResetDropdown();
    }

    public void ShowMonth(string name)
    {
        dropdown.value = dropDownSortedList.IndexOf(name) +1; //add one because 'New Month' is the first in dropdown, doesn't exist in dropDownSortedList
    }

    public void AddMonth(string name)
    {
        dropDownSortedList.Add(name);
        ResetDropdown();
    }
    public void DeleteCurrentMonth()
    {
        dropDownSortedList.Remove(dropdown.captionText.text);
        ResetDropdown();
        dropdown.SetValueWithoutNotify(0);
    }

    private void ResetDropdown()
    {
        ClearOptions();
        dropdown.AddOptions(SortDropdown(dropDownSortedList));
    }

    private void ClearOptions()
    {
        dropdown.ClearOptions();
        dropdown.options.Add(new TMPro.TMP_Dropdown.OptionData(text: "New Month"));
    }

    private List<string> SortDropdown(List<string> optionlist)
    {
        if (optionlist.Count <= 1) return optionlist;

        var sortedList = optionlist
            .Select(x => new { Name = x, Sort = DateTime.ParseExact(x, "yyyy MMMM", CultureInfo.InvariantCulture) })
            .OrderByDescending(x => x.Sort)
            .Select(x => x.Name)
            .ToList();

        dropDownSortedList = sortedList;
        return sortedList;
    }
}