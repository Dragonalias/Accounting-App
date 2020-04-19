using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ContentItem : MonoBehaviour
{
    private ContentSaveData saveData = null;

    [SerializeField]
    private RectTransform rectTransform;
    public RectTransform RectTransform { get => rectTransform; }

    [SerializeField]
    private TMP_InputField inputField;
    public TMP_InputField InputField { get => inputField; }

    private int column;
    private int row;

    public float Width {  get => RectTransform.sizeDelta.x; }
    public float Height { get => RectTransform.sizeDelta.y; }
    public int Column { get => column; set => column = value; }
    public int Row { get => row; set => row = value; }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
        if (saveData == null) saveData = new ContentSaveData();
    }

    public string GetData()
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
        var getData = JsonUtility.FromJson<ContentSaveData>(data);
        SetData(getData);
    }
    public void SetData(ContentSaveData data)
    {
        SetText(data.textData);
        SetType(data.type);
        SetInteractable(data.isInteractable);
    }
    
}

public class ContentSaveData
{
    public string textData;
    public TMP_InputField.ContentType type;
    public bool isInteractable;

    public ContentSaveData()
    {

    }
    public ContentSaveData(string textData, TMP_InputField.ContentType type, bool isInteractable)
    {
        this.textData = textData;
        this.type = type;
        this.isInteractable = isInteractable;
    }
}