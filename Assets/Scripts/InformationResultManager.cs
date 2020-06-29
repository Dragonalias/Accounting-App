using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationResultManager : MonoBehaviour
{
    [SerializeField] private PoolObject informationPrefab;

    public void AddResult2People(string personOwes, string isOwnedperson, float amount)
    {
        AddResult(string.Concat(personOwes, " Owes ", isOwnedperson, ":"), amount);
    }
    public void AddResult1Person(string person, float amount)
    {
        AddResult(string.Concat(person, " Spent:"), amount);
    }

    public void AddResult(string text, float amount)
    {
        InformationResult result = informationPrefab.GetInstance(transform).GetComponent<InformationResult>();
        result.Init();
        result.people.text = text;
        result.amount.text = amount.ToString();
        result.amount.textComponent.text = result.amount.text; //Wouldn't show properly unless i did this
    }


    public bool IsShowingResult()
    {
        return transform.childCount != 0;
    }

    public void Clear()
    {
        if (transform.childCount == 0) return;

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
