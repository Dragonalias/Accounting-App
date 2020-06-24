using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ContentItemFinance : ContentItemInputField, IPointerClickHandler
{
    [SerializeField] FinanceNote financeNote;

    public void SetNoteText(string note)
    {
        financeNote.SetText(note);
    }
    public string GetNoteText()
    {
        return financeNote.GetText();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            financeNote.On();
            EventSystem.current.SetSelectedGameObject(financeNote.gameObject);
        }
    }
}
