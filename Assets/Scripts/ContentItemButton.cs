using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class ContentItemButton : ContentItem
{
    private ContentInputFieldSaveData saveData = null;

    [SerializeField] private Button button;
    public Button Button { get => button; }

    public void MakeClickable(UnityAction<int, int> action)
    {
        Button.onClick.AddListener(()=>action(Column, Row));
    }

    public override string GetData()
    {
        return "";
    }

    public override void SetContentType()
    {
        contentType = ContentItemType.Button;
    }
    public override void ResetItem()
    {
        base.ResetItem();
        Button.onClick.RemoveAllListeners();
    }
}
