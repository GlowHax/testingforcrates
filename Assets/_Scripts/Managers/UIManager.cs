using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private Transform viewParent;

    [SerializeField] private Transform hud;
    [SerializeField] private View startingView;
    [SerializeField] private View pauseView;

    [SerializeField] private View currentView;
    private Stack<View> history = new Stack<View>();

    protected override void Awake()
    {
        base.Awake();
        GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
    }

    private void GameManager_OnGameStateChanged(GameState state)
    {
        if(state == GameState.Pause)
        {
            ShowScreen(pauseView);
        }
        else if(state == GameState.Running)
        {
            HideAll();
        }
    }

    public void HideAll()
    {
        currentView.Hide();
        currentView = null;
        while(history.Count > 0)
        {
            history.Pop().Hide();
        }
        hud.gameObject.SetActive(true);
    }

    public void ShowScreen(View view, bool remember = true)
    {
        if(currentView != null)
        {
            currentView.Hide();
            if(remember)
            {
                history.Push(currentView);
            }
        }
        else
        {
            hud.gameObject.SetActive(false);
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
            currentView = null;
            hud.gameObject.SetActive(true);
        }
    }
}