using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private View startingView;
    [SerializeField] private Transform viewParent;
    private View currentView;
    private Stack<View> history = new Stack<View>();

    public void HideAll()
    {
        currentView.Hide();
        foreach (View view in history)
        {
            history.Pop().Hide();
        }
    }

    public void ShowScreen(View view, bool remember = true)
    {
        if(currentView != null)
        {
            currentView.Hide();
            if (remember)
            {
                history.Push(currentView);
            }
        }

        view.Show(viewParent);
        currentView = view;
    }

    public void ShowLast()
    {
        if(history.Count > 0)
        {
            ShowScreen(history.Pop(), false);
        }
        else if(history.Count == 0)
        {
            currentView.Hide();
        }
    }
}