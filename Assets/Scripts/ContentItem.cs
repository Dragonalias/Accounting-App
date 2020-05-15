using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ContentItem : MonoBehaviour
{

    [SerializeField] private RectTransform rectTransform;
    public RectTransform RectTransform { get => rectTransform; }

    private int column;
    private int row;
    private bool savable = true;

    public float Width {  get => RectTransform.sizeDelta.x; }
    public float Height { get => RectTransform.sizeDelta.y; }
    public int Column { get => column; set => column = value; }
    public int Row { get => row; set => row = value; }
    public bool Savable { get => savable; set => savable = value; }

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
