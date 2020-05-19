using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PopupSystem : MonoBehaviour
{
    [SerializeField] private PopupElement confirm = null;
    [SerializeField] private PopupElement saveMonth = null;

    private void Popup()
    {
        gameObject.SetActive(true);
    }
    private void Popdown()
    {
        gameObject.SetActive(false);
        confirm.MischiefManaged();
        saveMonth.MischiefManaged();
    }

    public void PopupConfirmation(UnityAction confirmationEvent, UnityAction CancelEvent)
    {
        Popup();
        confirm.Init();
        confirm.confirmButton.onClick.AddListener(confirmationEvent);
        confirm.cancelButton.onClick.AddListener(CancelEvent);
    }
    public void PopupSaveMonth(UnityAction confirmationEvent, UnityAction CancelEvent)
    {
        Popup();
        saveMonth.Init();
        saveMonth.confirmButton.onClick.AddListener(confirmationEvent);
        saveMonth.cancelButton.onClick.AddListener(CancelEvent);
    }
}
