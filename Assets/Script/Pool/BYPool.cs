using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BYPool
{
    public int total;
    public string name_pool;
    public Transform prefab_;
    [NonSerialized]
    public List<Transform> elements = new List<Transform>();

    private int index = -1;
    public BYPool()
    {

    }
    public BYPool(string name_pool,int total,Transform prefab_)
    {
        this.name_pool = name_pool;
        this.total = total;
        this.prefab_ = prefab_;
    }
    public Transform Spawned()
    {
        index++;
        if(index>=elements.Count)
        {
            index = 0;
        }
        Transform trans = elements[index];
        trans.gameObject.SetActive(true);
        trans.gameObject.SendMessage("Spawned", SendMessageOptions.DontRequireReceiver);
        return trans;
    }
    public void DeSpawned(Transform trans)
    {
        if(elements.Contains(trans))
        {
            trans.gameObject.SendMessage("DeSpawned", SendMessageOptions.DontRequireReceiver);
            trans.gameObject.SetActive(false);
        }
    }
    public void DeSpawnAll()
    {
        foreach (Transform trans_ in elements)
        {
            trans_.SetParent(BYPoolManager.instance.transform);
            trans_.gameObject.SendMessage("DeSpawned", SendMessageOptions.DontRequireReceiver);
            trans_.gameObject.SetActive(false);
        }
    }
}
