using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DropdownMonths : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Dropdown dropdown;
    private AccountingManager manager;

    public void Init(AccountingManager manager)
    {
        this.manager = manager;

        List<string> dropdownList = new List<string>();
        dropdownList.AddRange(Directory.GetFiles(SaveSystem.SAVE_FOLDER, "*.json"));
        for (int i = 0; i < dropdownList.Count; i++)
        {
            dropdownList[i] = dropdownList[i].Replace(".json", "");
            dropdownList[i] = Path.GetFileName(dropdownList[i]);
        }
        dropdown.AddOptions(dropdownList);
        dropdown.onValueChanged.AddListener(delegate { manager.LoadMonth(dropdown.captionText.text); });
    }
    public void AddMonth(AccountingManager.Months month, int year, string name)
    {
        TMPro.TMP_Dropdown.OptionData data = new TMPro.TMP_Dropdown.OptionData();
        data.text = name;
        //dropdown.options.Insert()
    }
}
