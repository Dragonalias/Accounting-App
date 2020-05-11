using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FixScroll : MonoBehaviour, IScrollHandler
{
    public ScrollRect MainScroll;
    public Transform textArea;

    private Vector2 dummyVector = new Vector2(0,0);

    private void Update()
    {
        if(textArea.GetChild(0).localPosition.y != 0)
        {
            for (int i = 0; i < textArea.childCount; i++)
            {
                textArea.GetChild(i).localPosition = dummyVector;
            }
        }
    }

    public void OnScroll(PointerEventData eventData)
    {
        MainScroll.OnScroll(eventData);
    }
}
