using UnityEngine;
using UnityEngine.EventSystems;

public class ContentItemPerson : ContentItemInputField, IPointerClickHandler
{
    [SerializeField] private InterpayInstantiator interpayInstantiator;

    private Person connectedPerson;

    public Person ConnectedPerson { get => connectedPerson; set => connectedPerson = value; }

    public void Init(AccountingManager manager, Person person)
    {
        connectedPerson = person;
        interpayInstantiator.Init(manager, this);
    }

    public void UpdateMenuWithNewPerson(Person newPerson)
    {
        interpayInstantiator.UpdateMenuWithNewPerson(newPerson);
    }
    public void RemoveButton(Person deletePerson)
    {
        interpayInstantiator.RemoveButton(deletePerson);
    }
    public void ClearMenu()
    {
        interpayInstantiator.ResetMenu();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            interpayInstantiator.On();
            EventSystem.current.SetSelectedGameObject(interpayInstantiator.gameObject);
        }
    }

    public override void ResetItem()
    {
        base.ResetItem();
        interpayInstantiator.ResetMenu();
    }
}
