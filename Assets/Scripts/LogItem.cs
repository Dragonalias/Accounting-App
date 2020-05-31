using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LogItem : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    public void SetText(string text)
    {
        inputField.text = text;
    }
}
