using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BYPoolManager : BySingleton<BYPoolManager>
{
    public List<BYPool> pool_defaults;
    public Dictionary<string, BYPool> dic_pool = new Dictionary<string, BYPool>();
    // Start is called before the first frame update
    void Start()
    {
        foreach(BYPool p in pool_defaults)
        {
            CreatePool(p);
            dic_pool[p.name_pool] = p;
        }
    }
    public void AddNewPool(BYPool pool)
    {
        if(!dic_pool.ContainsKey(pool.name_pool))
        {
            CreatePool(pool);
            dic_pool[pool.name_pool] = pool;
        }
    }
    private void CreatePool(BYPool pool)
    {
        for(int i=0;i<pool.total;i++)
        {
            Transform trans = Instantiate(pool.prefab_);
            pool.elements.Add(trans);
            trans.gameObject.SetActive(false);
        }
    }
    public BYPool GetPool(string name_)
    {
        return dic_pool[name_];
    }
    private void OnDestroy()
    {
        dic_pool.Clear();
    }
}
