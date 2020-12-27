using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigation
{
    Stack<View> viewStack;
    View current;

    public Navigation()
    {
        viewStack = new Stack<View>();
        current = null;
    }
    
    public View Push(string name)
    {
        if(current != null)
        {
            current.Hide();
        }

        current = UINavationManager.Instance.viewObj.transform.Find(name).GetComponent<View>();
        viewStack.Push(current);
        current.Show();
        
        return current;
    }

    public View Pop()
    {
        if(viewStack.Count > 0)
        {
            viewStack.Peek().Hide();
            viewStack.Pop();

            if (viewStack.Count > 0) viewStack.Peek().Show();
        }
        return viewStack.Count > 0 ? viewStack.Peek() : null;
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
        return viewStack.Count > 0 ? viewStack.Peek() : null;
    }

    public View PopAll()
    {
        while(viewStack.Count > 0)
        {
            viewStack.Pop();
        }

        return null;
    }
}
