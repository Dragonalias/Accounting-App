using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour
{
    public PoolStack stack;

    public PoolObject GetInstance(Transform parent)
    {
        if (stack.Count > 0)
        {
            var obj = stack.Pop();
            if (obj == null) return GetInstance(parent);
            obj.transform.SetParent(parent);
            return obj;
        }
        return Instantiate(this, parent);
    }

    public PoolObject GetInstance()
    {
        if (stack.Count > 0)
        {
            var obj = stack.Pop();
            if (obj == null) return GetInstance();
            return obj;
        }
        return Instantiate(this);
    }

    private void OnDisable()
    {
        stack.Push(this);
    }
    private void OnDestroy()
    {
        stack.Clear();
    }
}
