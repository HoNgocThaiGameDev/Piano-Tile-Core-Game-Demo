using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewManager : BySingleton<ViewManager>
{
    public Transform anchorView;
    private Dictionary<ViewIndex, BaseView> dic_view = new Dictionary<ViewIndex, BaseView>();
    public BaseView current_view;
    // Start is called before the first frame update
    void Start()
    {
        foreach(ViewIndex vi in ViewConfig.viewIndices)
        {
            string view_name = vi.ToString();
            GameObject view_object = Instantiate(Resources.Load("View/" + view_name, typeof(GameObject))) as GameObject;
            view_object.transform.SetParent(anchorView, false);
            BaseView base_view = view_object.GetComponent<BaseView>();
            dic_view.Add(vi, base_view);
            view_object.SetActive(false);
            SwitchView(ViewIndex.EmptyView);
        }
    }

    // Update is called once per frame
    public void SwitchView(ViewIndex viewIndex,ViewParam param=null,Action callback=null)
    {
        if(current_view!=null)
        {
            ViewCallback viewCallback = new ViewCallback();
            viewCallback.callback = () =>
            {
             
                current_view.gameObject.SetActive(false);
                OnShowNewView(viewIndex, param, callback);
            
            };

            current_view.SendMessage("HideView", viewCallback);

        }
        else
        {
            OnShowNewView(viewIndex, param, callback);
        }
    }
    private void OnShowNewView(ViewIndex viewIndex, ViewParam param = null, Action callback = null)
    {
        current_view = dic_view[viewIndex];
        current_view.gameObject.SetActive(true);
        current_view.Setup(param);
        ViewCallback viewCallback = new ViewCallback();
        viewCallback.callback = callback;
      
        current_view.SendMessage("ShowView", viewCallback);
    }
}
public class ViewCallback
{
    public Action callback;

}