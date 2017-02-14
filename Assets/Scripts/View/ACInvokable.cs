using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public delegate void VoidDelegate();

public class ACInvokable : UnityEngine.MonoBehaviour
{
    object lockQueue = new object();
    Stack<VoidDelegate> GUIQueue = new Stack<VoidDelegate>();

    void FixedUpdate()
    {
        lock (lockQueue)
        {
            if (GUIQueue.Count > 0)
            {
                GUIQueue.Pop()();
            }
        }
    }

    public void Invoke(VoidDelegate item)
    {
        lock (lockQueue)
        {
            GUIQueue.Push(item);
        }
    }
}

