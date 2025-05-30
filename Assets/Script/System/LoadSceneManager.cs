using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneManager : BySingleton<LoadSceneManager>
{
    public GameObject UI_Loading;
    public Text progress_lb;
    public Image progress_image;
    public void LoadSceneByIndex(int index, Action callback)
    {
        var data = new LoadSceneData { index = index, callback = callback };

        StartCoroutine("ProgressLoadScene", data);
    }

    public void LoadSceneByName(string scene_name, Action callback)
    {
        var data = new LoadSceneData { scene_name = scene_name, callback = callback };
        StartCoroutine("ProgressLoadScene", data);
    }
    IEnumerator ProgressLoadScene(LoadSceneData data)
    {
        yield return new WaitForEndOfFrame();
        progress_lb.text = "0 %";
        progress_image.fillAmount = 0;
        UI_Loading.SetActive(true);
        AsyncOperation asyncOperation = null;
        if (data.index >= 0)
            asyncOperation = SceneManager.LoadSceneAsync(data.index, LoadSceneMode.Single);
        else
            asyncOperation = SceneManager.LoadSceneAsync(data.scene_name, LoadSceneMode.Single);

        while (!asyncOperation.isDone)
        {
            yield return new WaitForSeconds(0.2f);
            progress_lb.text = $"{Mathf.RoundToInt((asyncOperation.progress * 100))}%";
            progress_image.fillAmount = asyncOperation.progress;
        }
        if (data.callback != null)
        {
            data.callback();
        }
        UI_Loading.SetActive(false);
    }
    private class LoadSceneData
    {
        public int index = -1;
        public string scene_name;
        public Action callback;
    }
}
