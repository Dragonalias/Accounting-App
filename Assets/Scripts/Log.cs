using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Log : MonoBehaviour
{
    [SerializeField] private Color textColor;
    private void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        GameObject newLog = new GameObject("myTextGO");
        newLog.transform.SetParent(transform);
        var myText = CreateTextComponent(newLog);
        

        if (type == LogType.Exception)
        {
            myText.text = stackTrace;
        }
        else
        {
            myText.text = logString;
        }
        newLog.GetComponent<RectTransform>().sizeDelta = new Vector2(0, myText.preferredHeight);
    }

    private TMPro.TextMeshProUGUI CreateTextComponent(GameObject canvasGameObject)
    {
        var myText = canvasGameObject.AddComponent<TMPro.TextMeshProUGUI>();
        myText.color = textColor;
        myText.enableAutoSizing = true;
        myText.fontSizeMax = 30;

        return myText;
    }
}
