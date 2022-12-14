using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchArea : MonoBehaviour
{
    public List<SwitchButtons> switchButtons;
    public SwitchButtons selectedPointer;
    public List<GameObject> objectToSwap;

    public void Subscribe(SwitchButtons button)
    {
        if (switchButtons == null)
        {
            switchButtons = new List<SwitchButtons>();
        }

        switchButtons.Add(button);
    }

    public void OnTabSelected(SwitchButtons button)
    {
        if (selectedPointer != null)
        {
            selectedPointer.Deselect();
        }

        selectedPointer = button;

        selectedPointer.Select();

        int index = button.transform.GetSiblingIndex();
        Debug.Log("The index" + index);
        for (int i = 0; i < objectToSwap.Count; i++)
        {
            if (i == index)
            {
                objectToSwap[i].SetActive(true);
            }
            else
            {
                objectToSwap[i].SetActive(false);
            }
        }
    }
}
