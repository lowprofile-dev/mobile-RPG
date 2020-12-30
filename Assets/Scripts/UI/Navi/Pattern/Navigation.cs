using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigation
{
    Stack<View> viewStack;
    private View _current; public View current { get { return _current; } }

    public Navigation()
    {
        viewStack = new Stack<View>();
        _current = null;
    }

    public void UpdateNavigation()
    {
        if(_current != null) _current.UIUpdate();
    }

    public bool Find(string name)
    {
        return viewStack.Contains(UINavationManager.Instance.viewObj.transform.Find(name).GetComponent<View>());
    }

    
    public View Push(string name)
    {
        if(_current != null)
        {
            _current.Hide();
        }

        _current = UINavationManager.Instance.viewObj.transform.Find(name).GetComponent<View>();
        viewStack.Push(_current);
        _current.Show();
        
        return _current;
    }

    public View Pop()
    {
        if(viewStack.Count > 0)
        {
            viewStack.Peek().Hide();
            viewStack.Pop();

            if (viewStack.Count > 0) viewStack.Peek().Show();
        }
        return _current = viewStack.Count > 0 ? viewStack.Peek() : null;
    }

    public View Pop(string name)
    {
        while(viewStack.Count > 0)
        {
            if (viewStack.Peek().viewName == name)
            {
                viewStack.Peek().Hide();
                viewStack.Pop();
                break;
            }

            viewStack.Pop();
        }

        if (viewStack.Count > 0) viewStack.Peek().Show();
        return _current = viewStack.Count > 0 ? viewStack.Peek() : null;
    }

    public View PopAll()
    {
        while(viewStack.Count > 0)
        {
            viewStack.Pop();
        }

        return _current = null;
    }
}
