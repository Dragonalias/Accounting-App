using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationResultManager : MonoBehaviour
{
    [SerializeField] private InformationResult informationPrefab;

    public void AddResult(string personOwes, string isOwnedperson, float amount)
    {
        InformationResult result = Instantiate(informationPrefab, transform);
        result.people.text = string.Concat(personOwes, " Owes ", isOwnedperson, ":");
        result.amount.text = amount.ToString();
        result.amount.textComponent.text = result.amount.text; //Wouldn't show properly unless i did this
    }
}
