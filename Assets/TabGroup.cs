using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    public List<TabButton> tabButtons;
    public TabButton selectedTab;
    public List<GameObject> objectToSwap;
    // public Color tabIdle;
    // public Color tabActive;
     
    //Subscribe a TabButton to the TabGroup
    public void Subscribe(TabButton button)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<TabButton>();
        }

        tabButtons.Add(button);
    }

    /* public void OnTabEnter(TabButton button)
    {
        ResetTabs();
    } */

    /* public void OnTabExit(TabButton button)
    {
        ResetTabs();
    } */

    //When a TabButton is clicked, switch the display area by toggling SetActive state true/false.
    public void OnTabSelected(TabButton button)
    {
        if(selectedTab != null)
        {
            selectedTab.Deselect();
        }

        selectedTab = button;

        selectedTab.Select();

        int index = button.transform.GetSiblingIndex();
        Debug.Log("The index" + index);
        for(int i = 0; i<objectToSwap.Count; i++)
        {
            if(i == index)
            {
                objectToSwap[i].SetActive(true);
            }
            else
            {
                objectToSwap[i].SetActive(false);
            }
        }
    }

    /* public void ResetTabs()
    {
    if(selectedTab != null && button = selectedTab) { continue };
        foreach(TabButton button in tabButtons) {
            button.background.color = tabIdle;
        }
    } */
 }
