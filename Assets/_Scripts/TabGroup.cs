using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    public List<TabButton> tabButtons;
    public Color selectedColor;
    private TabButton selectedTab;
    public List<GameObject> menus;

    public void Subscribe(TabButton button)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<TabButton>();
        }

        tabButtons.Add(button);
    }

    public void OnTabSelected(TabButton button)
    {
        selectedTab = button;
        ResetTabs();
        button.background.color = Color.white;
        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < menus.Count; i++)
        {
            menus[i].SetActive(i == index);
        }
    }

    public void ResetTabs()
    {
        foreach (TabButton button in tabButtons)
        {
            if (selectedTab != null && button == selectedTab) continue;
            button.background.color = new Color(1f, 1f, 1f, .5f);
        }
    }
}