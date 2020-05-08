using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PoolStack", menuName = "ScriptableObjects/PoolStack")]
public class PoolStack : ScriptableObject
{
    public Stack<PoolObject> stack = new Stack<PoolObject>();

    public int Count => stack.Count;

    public void Push(PoolObject obj)
    {
        stack.Push(obj);
    }
    public PoolObject Pop()
    {
        return stack.Pop();
    }
    public void Clear()
    {
        stack.Clear();
    }
}
