using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.Singleton;

public class PoolManager : MonoSingleton<PoolManager>
{
    private Dictionary<int, Queue<GameObject>> poolDictionary;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitPool(GameObject obj)
    {
        if (obj.TryGetComponent(out Poolable poolable))
        {
            poolDictionary[poolable.poolKey] = new Queue<GameObject>();
            for (int i = 0; i < 20; i++)
            {
                var tmp = Instantiate(obj);
                tmp.SetActive(false);
                poolDictionary[poolable.poolKey].Enqueue(tmp);
            }
        }
    }

    public void AddItem(GameObject obj)
    {
        InitPool(obj);
    }

    public GameObject Get(int key)
    {
        if (poolDictionary[key].Count > 0)
        {
            var ret = poolDictionary[key].Dequeue();
            ret.SetActive(true);
            return ret;
        }
        return null;
    }

    public GameObject Get(GameObject obj)
    {
        if (obj.TryGetComponent(out Poolable poolable))
        {
            if (poolDictionary[poolable.poolKey].Count > 0)
            {
                var ret = poolDictionary[poolable.poolKey].Dequeue();
                ret.SetActive(true);
                return ret;
            }
        }
        return null;
    }

    public void Pool(GameObject obj)
    {
        if (obj.TryGetComponent(out Poolable poolable))
        {
            obj.SetActive(false);
            poolDictionary[poolable.poolKey].Enqueue(obj);
        }
        else
        {
            Destroy(obj);
        }
    }
}
