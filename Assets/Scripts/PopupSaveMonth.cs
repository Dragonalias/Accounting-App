using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PopupSaveMonth : PopupElement
{
    public TMPro.TMP_Dropdown dropdownYears;
    public TMPro.TMP_Dropdown dropdownMonths;
    public override void Init(PopupSystem popupSystem)
    {
        base.Init(popupSystem);

        PopulateMonths();
        PopulateYears();
        PrepareEvents(popupSystem);
    }

    public override void MischiefManaged()
    {
        gameObject.SetActive(false);
    }

    private void PopulateMonths()
    {
        var list = Enum.GetValues(typeof(AccountingManager.Months)).Cast<AccountingManager.Months>().Select(x => x.ToString("g")).ToList();
        dropdownMonths.AddOptions(list);
    }
    private void PopulateYears()
    {
        int earliestYear = 2010;
        var list = Enumerable.Range(earliestYear, DateTime.Today.Year - earliestYear+1).OrderByDescending(x => x).Select(x => x.ToString()).ToList();
        dropdownYears.AddOptions(list);
    }
    private void PrepareEvents(PopupSystem popupSystem)
    {
        confirmButton.onClick.AddListener(()=> popupSystem.manager.SaveMonth(dropdownMonths.captionText.text, dropdownYears.captionText.text));
    }
}
