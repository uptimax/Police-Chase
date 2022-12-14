using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SwitchButtons : MonoBehaviour, IPointerClickHandler
{
    public SwitchArea switchArea;
    public UnityEvent onPointerSelected;
    public UnityEvent onPointerDeselected;

    // Start is called before the first frame update
    void Start()
    {
        switchArea.Subscribe(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        switchArea.OnTabSelected(this);
        Debug.Log("You selected this pointer");
    }

    public void Select()
    {
        if (onPointerSelected != null)
        {
            onPointerSelected.Invoke();
        }
    }

    public void Deselect()
    {
        if (onPointerSelected != null)
        {
            onPointerDeselected.Invoke();
        }
    }
}
