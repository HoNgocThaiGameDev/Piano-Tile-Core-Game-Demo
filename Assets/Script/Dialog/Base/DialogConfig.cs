using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum DialogIndex
{
    PauseDialog=0,
    ResultDialog=1
}
public class DialogParam
{

}

public class DialogConfig
{
    public static DialogIndex[] dialogIndices = {
        DialogIndex.PauseDialog,
        DialogIndex.ResultDialog
    };

}
