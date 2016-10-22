/************************************************************
    File      : EventSys
	Author    : Plane
    Version   : 1.0
    Function  : 事件系统
    Date      : 2016/10/20 14:57:3
    Copyright : 2016 CodeVFX Inc.
*************************************************************/
using UnityEngine;

public class EventSys : ISystem 
{
    ///////////////////////////Data Define//////////////////////////////
    public static EventSys Instance = null;

    //----------------------------------------------------------------//
    
    ///////////////////////////MainFunctions////////////////////////////    
    public override void Init()
    {
        Instance = this;
    }
    //----------------------------------------------------------------//
}