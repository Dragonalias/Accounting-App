using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InformationResult : MonoBehaviour
{
    public TMPro.TMP_Text people;
    public TMPro.TMP_InputField amount;

    public void Init()
    {
        transform.localScale = Vector3.one;
    }
}
