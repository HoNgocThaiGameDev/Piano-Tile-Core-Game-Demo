using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ViewIndex
{
    EmptyView=0,
    HomeView=1,
    InGameView=2
}
public class ViewParam
{

}
public class gameViewParam:ViewParam
{
    public int nodeID;
}

public class ViewConfig 
{
    public static ViewIndex[] viewIndices = { 
    
        ViewIndex.EmptyView,
        ViewIndex.HomeView,
        ViewIndex.InGameView
    };

} 
