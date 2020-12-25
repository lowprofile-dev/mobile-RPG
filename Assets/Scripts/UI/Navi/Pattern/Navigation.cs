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
        return viewStack.Peek();
    }

    public View Pop(string name)
    {
        Debug.Log("VIEWSTACK : " + viewStack);
        while(viewStack.Count > 0)
        {
            Debug.Log("currnet : " + viewStack.Peek().viewName + " " + name);
            if (viewStack.Peek().viewName == name)
            {
                viewStack.Pop();
                break;
            }

            viewStack.Pop();
        }

        if (viewStack.Count > 0) viewStack.Peek().Show();
        return viewStack.Peek();
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
