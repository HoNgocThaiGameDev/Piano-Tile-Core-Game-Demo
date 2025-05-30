using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseView : MonoBehaviour
{
    public ViewIndex viewIndex;
    private BaseViewAnimation baseViewAnimation;
    // Start is called before the first frame update
    void Awake()
    {
        baseViewAnimation = gameObject.GetComponentInChildren<BaseViewAnimation>();
    }

    // Update is called once per frame
    public virtual void Setup(ViewParam data)
    { }
    private void ShowView(ViewCallback viewCallback)
    {
        baseViewAnimation.OnShowView(() =>
        {

            OnShowView();
            viewCallback.callback?.Invoke();
        });
    }
    public virtual void OnShowView()
    {


    }
    private void HideView(ViewCallback viewCallback)
    {
        baseViewAnimation.OnHideView(() => {

            OnHideView();
            viewCallback.callback?.Invoke();
        });
    }
    public virtual void OnHideView()
    {

    }
}
