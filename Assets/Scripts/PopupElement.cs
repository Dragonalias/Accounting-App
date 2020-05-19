using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupElement : MonoBehaviour
{
    public Button confirmButton;
    public Button cancelButton;

    public virtual void Init()
    {
        gameObject.SetActive(true);
    }

    public virtual void MischiefManaged()
    {
        gameObject.SetActive(false);
    }
}
