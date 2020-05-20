using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopupSystem : MonoBehaviour
{
    [SerializeField] private PopupElement confirm = null;
    [SerializeField] private PopupSaveMonth saveMonth = null;

    [HideInInspector] public AccountingManager manager;

    public void Init(AccountingManager manager)
    {
        this.manager = manager;
        confirm.Init(this);
        saveMonth.Init(this);
    }
    private void Popup()
    {
        gameObject.SetActive(true);
    }
    public void Popdown()
    {
        gameObject.SetActive(false);
        confirm.MischiefManaged();
        saveMonth.MischiefManaged();
    }

    private void PopupBase(PopupElement button, UnityAction confirmationEvent = null, UnityAction cancelEvent = null)
    {
        Popup();
        button.gameObject.SetActive(true);
        if (confirmationEvent != null)
        {
            button.confirmButton.onClick.AddListener(confirmationEvent);
        }
        if (cancelEvent != null)
        {
            button.cancelButton.onClick.AddListener(cancelEvent);
        }
    }

    public void PopupConfirmation(UnityAction confirmationEvent, UnityAction cancelEvent = null)
    {
        PopupBase(confirm, confirmationEvent, cancelEvent);
    }
    public void PopupSaveMonth()
    {
        PopupBase(saveMonth);
    }
}
