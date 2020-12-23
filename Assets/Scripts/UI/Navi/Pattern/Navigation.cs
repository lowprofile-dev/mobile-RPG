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
        viewStack.Peek().Hide();
        viewStack.Pop();
        if(viewStack.Peek() != null)
        {
            viewStack.Peek().Show();
        }

        return viewStack.Peek();
    }

    public View Pop(string name)
    {
        while(viewStack.Peek() != null)
        {
            viewStack.Pop();

            if(viewStack.Peek().viewName == name)
            {
                viewStack.Pop();
                break;
            }
        }

        return viewStack.Peek();
    }

    public View PopAll()
    {
        while(viewStack.Peek() != null)
        {
            viewStack.Pop();
        }

        return null;
    }
}
