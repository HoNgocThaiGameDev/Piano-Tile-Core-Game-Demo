using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigManager : BySingleton<ConfigManager>
{
    public ConfigSong configSong;

    public void InitConfig(Action callback)
    {
        StartCoroutine("LoadLocal", callback);
    }
    private IEnumerator LoadLocal(Action callback)
    {
        configSong = Resources.Load("DataTable/ConfigSong", typeof(ScriptableObject)) as ConfigSong;
        yield return new WaitUntil(()=> configSong !=null);
        callback?.Invoke();
    }
}
