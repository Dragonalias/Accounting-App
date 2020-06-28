using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class InterpayInstantiatorButton : MonoBehaviour
{
    public Button thisButton;
    public TMPro.TMP_Text buttonText;

    public void Init()
    {
        transform.localScale = Vector3.one;
    }
    public void Activate()
    {
        gameObject.SetActive(true);
    }
    public void OnDisable()
    {
        thisButton.onClick.RemoveAllListeners();
    }
}
