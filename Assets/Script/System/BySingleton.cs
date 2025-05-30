using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BySingleton<T> : MonoBehaviour where T :MonoBehaviour
{
    private static T instance_;
    public static T instance
    {
        get
        {
            if(BySingleton<T>.instance_==null)
            {
                BySingleton<T>.instance_ = (T)FindObjectOfType(typeof(T));
                if(BySingleton<T>.instance_ == null)
                {
                    GameObject go = new GameObject();
                    go.name = "@[" + typeof(T).Name + "]";
                    BySingleton<T>.instance_ = go.AddComponent<T>();
                }
            }
            return BySingleton<T>.instance_;
        }
    }

    private void Reset()
    {
        gameObject.name = typeof(T).Name;
    }

}
