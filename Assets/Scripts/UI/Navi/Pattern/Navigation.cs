////////////////////////////////////////////////////
/*
    File Navigation.cs
    class Navigation

    담당자 : 이신홍
    부 담당자 : 

    UI를 스택으로 관리하며 '현재'의 UI만 표시할 수 있도록 하는 설계구조에 사용되는 클래스.
*/
////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigation
{
    Stack<View> viewStack;  // View Stack
    private View _current;

    // property
    public View current { get { return _current; } }




    public Navigation()
    {
        viewStack = new Stack<View>();
        _current = null;
    }

    public void UpdateNavigation()
    {
        if(_current != null) _current.UIUpdate(); // 현재 활성화 된 View만을 업데이트한다.
    }

    public bool Find(string name)
    {
        return viewStack.Contains(UINaviationManager.Instance.viewObj.transform.Find(name).GetComponent<View>());
    }
    
    /// <summary>
    /// View를 Navigation에 넣고 이 View를 표시한다.
    /// </summary>
    public View Push(string name)
    {
        if(_current != null) _current.Hide();

        _current = UINaviationManager.Instance.viewObj.transform.Find(name).GetComponent<View>();
        viewStack.Push(_current);
        _current.Show();
        
        return _current;
    }
    
    /// <summary>
    /// 제일 위의 View를 없앤다.
    /// </summary>
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


    /// <summary>
    /// 해당하는 View가 지워질때까지 축적된 모든 View를 지운다.
    /// </summary>
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

    /// <summary>
    /// 모든 View를 지운다.
    /// </summary>
    public View PopAll()
    {
        while(viewStack.Count > 0)
        {
            viewStack.Pop();
        }

        return _current = null;
    }
}
