using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public abstract class ContentItem : MonoBehaviour
{

    [SerializeField] private RectTransform rectTransform;
    public RectTransform RectTransform { get => rectTransform; }

    private int column = -1;
    private int row;
    private bool savable = true;
    private ContentItem parent = null;

    public float Width {  get => RectTransform.sizeDelta.x; }
    public float Height { get => RectTransform.sizeDelta.y; }
    public int Column  { get  { return column != -1 ? column : parent.Column; } set => column = value; }
    public int Row { get => row; set => row = value; }
    public bool Savable { get => savable; set => savable = value; }
    public ContentItem Parent { get => parent; set => parent = value; }

    public virtual void SetActive(bool active)
    {
        if (active == false) ResetItem();
        gameObject.SetActive(active);
    }

    public abstract string GetData();
    public virtual void ResetItem()
    {

    }
}
