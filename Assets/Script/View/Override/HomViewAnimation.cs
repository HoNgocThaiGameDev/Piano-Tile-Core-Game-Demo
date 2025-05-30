using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HomViewAnimation : BaseViewAnimation
{
    //public RectTransform top;
    //public RectTransform bottom;
    //public RectTransform left;
    public override void OnHideView(Action callback)
    {
        base.OnHideView(callback);
        //top.DOAnchorPosY(250, 0.5f).OnComplete(() => {
        //    callback();
        //});
        //bottom.DOAnchorPosY(-400, 0.5f);
        //left.DOAnchorPosX(1200, 0.25f);
    }
    public override void OnShowView(Action callback)
    {
        base.OnShowView(callback);
        //top.DOAnchorPosY(0, 0.5f).OnComplete(()=>{
        //    callback();
        //});
        //bottom.DOAnchorPosY(0, 0.5f);
        //left.DOAnchorPosX(0, 0.25f);
    }
}
