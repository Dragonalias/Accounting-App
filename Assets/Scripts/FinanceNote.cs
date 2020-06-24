using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class FinanceNote : MonoBehaviour, IDeselectHandler
{
    [SerializeField] private TMP_InputField inputField = null;
    public void On()
    {
        gameObject.SetActive(true);

    }
    public void Off()
    {
        gameObject.SetActive(false);
    }

    public void SetText(string text)
    {
        inputField.text = text;
    }
    public string GetText()
    {
        return inputField.text;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        Off();
    }
}
