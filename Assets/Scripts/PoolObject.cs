using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour
{
    private Stack<PoolObject> stack = new Stack<PoolObject>();

    public PoolObject GetInstance(Transform parent)
    {
        if (stack.Count > 0)
        {
            var obj = stack.Pop();
            if (obj == null) return GetInstance(parent);
            return obj;
        }
        return Instantiate(this, parent);
    }

    void OnDisable()
    {
        stack.Push(this);
    }
}
