using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentItem : MonoBehaviour
{
    [SerializeField]
    private RectTransform rectTransform;
    public RectTransform RectTransform { get => rectTransform; }

    public float Width {  get => RectTransform.sizeDelta.x; }
    public float Height { get => RectTransform.sizeDelta.y; }
    
}
