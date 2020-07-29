using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    public List<GameObject> objectsToSawp;
    public List<TabButton> tabButtons;

    public TabButton selectedTab;
    public Sprite TabActive;
    
    public Sprite TabHover;
    public Sprite TabIdle;
    public Animator ScaleAnimator;


    public void Subscribe(TabButton button)
    {
        if (tabButtons == null) tabButtons = new List<TabButton>();
        tabButtons.Add(button);
    }

    public void OnTabEnter(TabButton button)
    {
        ResetTabs();
        if (selectedTab == null || button != selectedTab) button.Background.sprite = TabHover;
    }

    public void OnTabExit(TabButton button)
    {
        ResetTabs();
    }

    public void OnTabSelected(TabButton button)
    {
        if (selectedTab != null) selectedTab.Deselect();
        selectedTab.GetComponent<Animator>().SetInteger("Scaled",0);
        selectedTab = button;

        selectedTab.Select();

        selectedTab = button;

        ResetTabs();
        button.Background.sprite = TabActive;
        var index = button.transform.GetSiblingIndex();
        for (var i = 0; i < objectsToSawp.Count; i++)
            if (i == index)
                objectsToSawp[i].SetActive(true);
            else
                objectsToSawp[i].SetActive(false);
    }

    public void ResetTabs()
    {
        foreach (var button in tabButtons)
        {
            if (selectedTab != null && button == selectedTab) continue;
            button.Background.sprite = TabIdle;
            selectedTab.GetComponent<Animator>().SetInteger("Scaled",1);
            ScaleAnimator.SetBool("Scaled",false);
        }
    }
}