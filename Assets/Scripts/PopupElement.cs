using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupElement : MonoBehaviour
{
    public Button confirmButton;
    public Button cancelButton;
    public TMPro.TMP_Text aboutText;

    protected PopupSystem popupSystem;

    public virtual void Init(PopupSystem popupSystem)
    {
        this.popupSystem = popupSystem;
        AddEvents();
    }

    public virtual void MischiefManaged()
    {
        gameObject.SetActive(false);
        confirmButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();
        AddEvents();
    }

    private void AddEvents()
    {
        confirmButton.onClick.AddListener(popupSystem.Popdown);
        cancelButton.onClick.AddListener(popupSystem.Popdown);
    }
}
