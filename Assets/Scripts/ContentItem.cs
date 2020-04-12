using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ContentItem : MonoBehaviour
{
    [SerializeField]
    private RectTransform rectTransform;
    public RectTransform RectTransform { get => rectTransform; }

    public float Width {  get => RectTransform.sizeDelta.x; }
    public float Height { get => RectTransform.sizeDelta.y; }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public string GetData()
    {
        return this.ToString();
    }
    public void SetData(string data)
    {

    }
    
}
