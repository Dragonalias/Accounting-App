using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;

public class DropdownMonths : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Dropdown dropdown = null;
    private List<string> dropDownSortedList;
    private AccountingManager manager;

    public void Init(AccountingManager manager)
    {
        this.manager = manager;
        ClearOptions();
        dropdown.onValueChanged.AddListener(delegate { manager.LoadMonth(dropdown.captionText.text); });

        List<string> dropdownList = new List<string>();
        dropdownList.AddRange(Directory.GetFiles(SaveSystem.SAVE_FOLDER, "*.json"));
        for (int i = 0; i < dropdownList.Count; i++)
        {
            dropdownList[i] = dropdownList[i].Replace(".json", "");
            dropdownList[i] = Path.GetFileName(dropdownList[i]);
        }

        dropDownSortedList = SortDropdown(dropdownList);
        dropdown.AddOptions(dropDownSortedList);
    }

    public void AddMonth(string name)
    {
        dropDownSortedList.Add(name);
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

        return sortedList;
    }
}