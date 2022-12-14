using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TabButton : MonoBehaviour, IPointerClickHandler
{
    public TabGroup tabGroup;
    public UnityEvent onTabSelected;
    public UnityEvent onTabDeselected;

    // Start is called before the first frame update
    //On Start, subscribe button to tabGroup
    void Start()
    {
        tabGroup.Subscribe(this);
    }

    /* public void OnSelect(BaseEventData eventData)
    {
        tabGroup.OnTabEnter(this);
        Debug.Log("You are hovering the button");
    } */

    public void OnPointerClick(PointerEventData eventData)
    {
        tabGroup.OnTabSelected(this);
        Debug.Log("You selected this tab");
    }

    /* public void OnDeselect(BaseEventData eventData)
    {
        tabGroup.OnTabExit(this);
        Debug.Log("You selected the button");
    } */

    // Update is called once per frame

    public void Select()
    {
        if(onTabSelected != null)
        {
            onTabSelected.Invoke();
        }
    }

    public void Deselect()
    {
        if (onTabSelected != null)
        {
            onTabDeselected.Invoke();
        }
    }
}
