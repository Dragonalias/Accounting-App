using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PopupSaveMonth : PopupElement
{
    public TMPro.TMP_Dropdown dropdownYears;
    public TMPro.TMP_Dropdown dropdownMonths;
    public override void Init()
    {
        base.Init();
        if (dropdownMonths.options.Count == 0)
        {
            populateMonths();
        }
        if (dropdownYears.options.Count == 0)
        {
            populateYears();
        }
    }

    private void populateMonths()
    {
        var list = Enum.GetValues(typeof(AccountingManager.Months)).Cast<AccountingManager.Months>().Select(x => x.ToString("g")).ToList();
        dropdownMonths.AddOptions(list);
    }
    private void populateYears()
    {
        int earliestYear = 2010;
        var list = Enumerable.Range(earliestYear, DateTime.Today.Year - earliestYear+1).OrderByDescending(x => x).Select(x => x.ToString()).ToList();
        dropdownYears.AddOptions(list);
    }
}
