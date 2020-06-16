using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

public class ContentItemInputField : ContentItem
{
    [SerializeField] private Color defaultColor;

    [SerializeField] private Color calculationColor;

    [SerializeField] private Button deleteButton = null;

    [SerializeField] private TMP_InputField inputField = null;
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

    public void SetDefaultColor()
    {
        InputField.image.color = defaultColor;
    }
    public void SetCalculationColor()
    {
        InputField.image.color = calculationColor;
    }

    public override void ResetItem()
    {
        base.ResetItem();
        Column = -1;
        SetDefaultColor();
        InputField.onEndEdit.RemoveAllListeners();
        InputField.readOnly = false;
        InputField.onValidateInput = null;
        deleteButton.onClick.RemoveAllListeners();
        deleteButton.gameObject.SetActive(false);
    }
}
