using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

public class ContentItemInputField : ContentItem
{

    [SerializeField] private Button deleteButton;

    [SerializeField] private TMP_InputField inputField;
    public TMP_InputField InputField { get => inputField; }

    public override void SetActive(bool active)
    {
        base.SetActive(active);
    }

    public void MakeDeleteable(UnityAction<ContentItemInputField> action)
    {
        deleteButton.gameObject.SetActive(true);
        deleteButton.onClick.AddListener(()=> action(this));
    }
    public override string GetData()
    {
        return InputField.text;
    }
    public void SetType(TMP_InputField.ContentType type)
    {
        InputField.contentType = type;
    }
    public void SetInteractable(bool activity)
    {
        InputField.interactable = activity;
    }

    public void SetText(string text)
    {
        InputField.text = text;
    }
    public void SetData(string data)
    {
        InputField.text = data;
    }

    public override void ResetItem()
    {
        base.ResetItem();
        InputField.onEndEdit.RemoveAllListeners();
        InputField.readOnly = false;
        InputField.onValidateInput = null;
        deleteButton.onClick.RemoveAllListeners();
        deleteButton.gameObject.SetActive(false);
    }
}
