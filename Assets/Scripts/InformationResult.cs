using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InformationResult : MonoBehaviour, IPointerClickHandler
{
    public TMPro.TMP_Text people;
    public TMPro.TMP_Text amount;

    public void OnPointerClick(PointerEventData eventData)
    {
        GUIUtility.systemCopyBuffer = amount.text;
        Debug.Log("Text was copied: " + amount.text);
    }
}
