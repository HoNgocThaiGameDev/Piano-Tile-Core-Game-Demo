using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class BaseViewAnimation : MonoBehaviour
{
    private CanvasGroup canvasGroup_;
    // Start is called before the first frame update
    void Awake()
    {
        canvasGroup_ = gameObject.GetComponent<CanvasGroup>();

    }

    // Update is called once per frame
    public virtual void OnShowView(Action callback)
    {
        canvasGroup_.DOFade(1, 0.5f).OnComplete(() =>
        {
            callback?.Invoke();
        });
    }
    public virtual void OnHideView(Action callback)
    {
        canvasGroup_.DOFade(0, 0.25f).OnComplete(() =>
        {
            callback?.Invoke();
        });
    }
}
