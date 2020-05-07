using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpawnManagerScriptableObject", order = 1)]
public class PoolObject : MonoBehaviour
{
    public PoolStack stack;

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
