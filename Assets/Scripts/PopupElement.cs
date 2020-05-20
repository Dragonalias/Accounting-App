using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupElement : MonoBehaviour
{
    public Button confirmButton;
    public Button cancelButton;

    public virtual void Init(PopupSystem popupSystem)
    {
        confirmButton.onClick.AddListener(popupSystem.Popdown);
        cancelButton.onClick.AddListener(popupSystem.Popdown);
    }

    public virtual void MischiefManaged()
    {
        gameObject.SetActive(false);
    }
}
