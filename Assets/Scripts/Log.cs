using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Log : MonoBehaviour
{
    [SerializeField] private LogItem log;
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
        var newLog = Instantiate(log, transform);
        

        if (type == LogType.Exception)
        {
            newLog.SetText(stackTrace);
        }
        else
        {
            newLog.SetText(logString);
        }
    }
}
