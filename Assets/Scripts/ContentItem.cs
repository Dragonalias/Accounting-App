using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ContentItem : MonoBehaviour
{
    [SerializeField]
    private RectTransform rectTransform;
    public RectTransform RectTransform { get => rectTransform; }

    [SerializeField]
    private TMP_InputField inputField;
    public TMP_InputField InputField { get => inputField; }

    public float Width {  get => RectTransform.sizeDelta.x; }
    public float Height { get => RectTransform.sizeDelta.y; }
    

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public string GetData()
    {
        return InputField.text + "_" + InputField.contentType;
    }
    public void SetType(TMP_InputField.ContentType type)
    {
        InputField.contentType = type;
    }
    public void SetData(string data)
    {
        var dataSplit = data.Split('_');
        InputField.text = dataSplit[0]; Debug.Log(dataSplit[1]);
        SetType((TMP_InputField.ContentType)System.Enum.Parse(typeof(TMP_InputField.ContentType), dataSplit[1]));
    }
    public void ResetData()
    {
        InputField.text = "";
    }
    
}
