using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootLoader : MonoBehaviour
{
    public DataAPIController dataAPIController;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        ConfigManager.instance.InitConfig(LoadConfigDone);
    }
    private void LoadConfigDone()
    {
        dataAPIController.InitData(() =>
        {
            LoadSceneManager.instance.LoadSceneByIndex(1, () =>
            {
                Debug.Log(" load Buffer done!");
                ViewManager.instance.SwitchView(ViewIndex.HomeView);
            });
        });
    }
}
