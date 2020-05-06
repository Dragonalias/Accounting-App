using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

public class ContentItemInputField : ContentItem
{
    private ContentInputFieldSaveData saveData = null;

    [SerializeField] private Button deleteButton;

    [SerializeField] private TMP_InputField inputField;
    public TMP_InputField InputField { get => inputField; }

    public override void SetActive(bool active)
    {
        if(saveData == null) saveData = new ContentInputFieldSaveData();
        base.SetActive(active);
    }

    public void MakeDeleteable(AccountingManager manager)
    {
        deleteButton.gameObject.SetActive(true);
        deleteButton.onClick.AddListener(()=>manager.RemoveContentItem(Column, Row));
    }
    public override string GetData()
    {
        return JsonUtility.ToJson(saveData);
    }
    public void SetType(TMP_InputField.ContentType type)
    {
        InputField.contentType = type;
        saveData.type = type;
    }
    public void SetInteractable(bool activity)
    {
        InputField.interactable = activity;
        saveData.isInteractable = activity;
    }

    public void SetText(string text)
    {
        InputField.text = text;
        saveData.textData = text;
    }
    public void SetData(string data)
    {
        var getData = JsonUtility.FromJson<ContentInputFieldSaveData>(data);
        SetData(getData);
    }
    public void SetData(ContentInputFieldSaveData data)
    {
        SetText(data.textData);
        SetType(data.type);
        SetInteractable(data.isInteractable);
    }

    public override void ResetItem()
    {
        base.ResetItem();
        InputField.onEndEdit.RemoveAllListeners();
        deleteButton.onClick.RemoveAllListeners();
        deleteButton.gameObject.SetActive(false);
    }

    public override void SetContentType()
    {
        contentType = ContentItemType.InputField;
    }
}

public class ContentInputFieldSaveData
{
    public string textData;
    public TMP_InputField.ContentType type;
    public bool isInteractable;

    public ContentInputFieldSaveData()
    {

    }
    public ContentInputFieldSaveData(string textData, TMP_InputField.ContentType type, bool isInteractable)
    {
        this.textData = textData;
        this.type = type;
        this.isInteractable = isInteractable;
    }
}
