using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ContentItem : MonoBehaviour
{
    public enum ContentItemType { InputField, Button }

    [SerializeField] private RectTransform rectTransform;
    public RectTransform RectTransform { get => rectTransform; }

    private int column;
    private int row;
    protected ContentItemType contentType;

    public float Width {  get => RectTransform.sizeDelta.x; }
    public float Height { get => RectTransform.sizeDelta.y; }
    public int Column { get => column; set => column = value; }
    public int Row { get => row; set => row = value; }

    private void Start()
    {
        SetContentType();
    }
    public virtual void SetActive(bool active)
    {
        if (active == false) ResetItem();
        else SetContentType();
        gameObject.SetActive(active);
    }
    public abstract void SetContentType();
    public ContentItemType GetContentType()
    {
        return contentType;
    }
    public abstract string GetData();
    public virtual void ResetItem()
    {

    }
}
